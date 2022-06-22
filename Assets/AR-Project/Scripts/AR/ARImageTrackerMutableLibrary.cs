using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

/// <summary>
/// Class to handle the 2D image detection and tracking
/// </summary>
[RequireComponent(typeof(ARTrackedImageManager))]
public class ARImageTrackerMutableLibrary : MonoBehaviour 
{
    #region Inspector
    [Header("SEND Channels")]
    /// <summary>
    /// The SO channel for the AR events
    /// </summary>
    [Tooltip("The SO channel for the AR events")]
    [SerializeField] private AREventChannelSO aREventChannelSO;

    [Header("References")]
    [SerializeField] private SessionDataSO sessionDataSO;
    [SerializeField] private GameStateSO gameStateSO;

    #endregion

    #region Private variables 
    /// <summary>
    /// The hashset for keeping track of the detected images
    /// </summary>
    private HashSet<string> detectedImages = new HashSet<string>();
    /// <summary>
    /// Reference for the ARTrackedImageManager from ARCoreSession
    /// </summary>
    private ARTrackedImageManager trackedImagesManager;
    bool isTracking;
    #endregion

    #region Unity methods
    void Awake() 
    {
        // Disable screen dimming
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        trackedImagesManager = GetComponent<ARTrackedImageManager>();
        trackedImagesManager.enabled = false;

    }

    void OnEnable() 
    {
        trackedImagesManager.trackedImagesChanged += OnTrackedImagesChanged;
        
        ARSession.stateChanged += OnARSessionStateChanged;
    }

    void OnDisable() 
    {
        trackedImagesManager.trackedImagesChanged -= OnTrackedImagesChanged;
        
        ARSession.stateChanged -= OnARSessionStateChanged;
    }
    #endregion

    #region Helper methods
    private IEnumerator AddAllImagesToMutableReferenceImageLibraryAR()
    {
        // You can either add raw image bytes or use the extension method (used below) which accepts
        // a texture. To use a texture, however, its import settings must have enabled read/write
        // access to the texture.

        if (trackedImagesManager == null) 
        {
            Debug.Log($"No {nameof(ARTrackedImageManager)} available.");
            
            yield break;
        }

        if (trackedImagesManager.referenceLibrary == null) 
        {
            Debug.Log("[AFP] PRE Library is null");
        } 
        else 
        {
            Debug.Log("[AFP] PRE Library exists");
        }

        trackedImagesManager.referenceLibrary = trackedImagesManager.CreateRuntimeLibrary();

        if (trackedImagesManager.referenceLibrary == null) 
        {
            Debug.Log("[AFP] POST Library is null");
        } 
        else 
        {
            Debug.Log("[AFP] POST Library exists");
        }

        if (trackedImagesManager.referenceLibrary is MutableRuntimeReferenceImageLibrary mutableLibrary)
        {
            foreach (var point in sessionDataSO.PointsOfInterest.Points)
            {
                if (point.image.isReadable)
                {
                    point.jobState = mutableLibrary.ScheduleAddImageWithValidationJob
                    (   
                        point.image, 
                        point.imageName, 
                        null
                    );

                    yield return new WaitUntil(() => point.jobState.jobHandle.IsCompleted);

                    Debug.Log("[ARP] " + point.title + " jobState: " + point.jobState.status);
                }
                else
                {
                    Debug.Log($"[ARP] Image {point.image.name} must be readable to be added to the image library.");
                    
                    yield return null;
                }
            }

            Debug.Log("[AFP] Library Count: " + trackedImagesManager.referenceLibrary.count);

            gameStateSO.UpdateGameState(GameState.Tracking);

            trackedImagesManager.enabled = true;
        }
        else
        {
            Debug.Log($"[ARP]The reference image library is not mutable.");
        }     
    }
    #endregion

    #region Callbacks
    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs) 
    {
        if(gameStateSO.CurrentGameState == GameState.Tracking)
        {
            // Go through all tracked images that have been added
            // (-> new markers detected)
            foreach (var trackedImage in eventArgs.added) 
            {
                // Get the name of the reference image to search for its hash inside the detectedImages hashset
                var imageName = trackedImage.referenceImage.name;

                // If the hash is NOT inside the hashset (the image was never detected)
                if (!detectedImages.Contains(trackedImage.referenceImage.name)) 
                {
                    // Raise an ARImageRecognized event passing the name of the image
                    aREventChannelSO.RaisePOIDetectionEvent(trackedImage.referenceImage.name);

                    // Change the current game state
                    gameStateSO.UpdateGameState(GameState.POIPopUp);

                    Debug.Log("[AFP] IMAGE DETECTED");

                    // Add the hash to the hashset
                    detectedImages.Add(trackedImage.referenceImage.name);
                }
                // If the hash IS inside the hashset (the image was already detected)
                else 
                {
                    // Continue the loop
                    continue;
                }
            }

            foreach (var trackedImage in eventArgs.removed)
            {
                Debug.Log("[ARP] Image removed: " + trackedImage.name);
            }
        } 
    }

    private void OnARSessionStateChanged(ARSessionStateChangedEventArgs obj) 
    {
        Debug.Log("[ARP] ARSession.state: " + ARSession.state);
        Debug.Log("[ARP] CurrentGameState: " + gameStateSO.CurrentGameState);

        if (gameStateSO.CurrentGameState == GameState.Loading)
        {
            if ((ARSession.state == ARSessionState.SessionTracking) && !isTracking) 
            {
                isTracking = true;

                StartCoroutine(AddAllImagesToMutableReferenceImageLibraryAR());
            }
        }   
    }
    #endregion
}
