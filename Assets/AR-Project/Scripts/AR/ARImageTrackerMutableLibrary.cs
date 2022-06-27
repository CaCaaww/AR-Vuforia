using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using TMPro;

/// <summary>
/// Class to handle the 2D image detection and tracking
/// </summary>
[RequireComponent(typeof(ARTrackedImageManager))]
public class ARImageTrackerMutableLibrary : MonoBehaviour 
{
    #region Inspector
    [Header("LISTEN Channels")]
    /// <summary>
    /// The SO channel for the UI events
    /// </summary>
    [Tooltip("The SO channel for the UI events")]
    [SerializeField] private UIEventsChannelSO uiEventsChannelSO;

    [Header("SEND Channels")]
    /// <summary>
    /// The SO channel for the AR events
    /// </summary>
    [Tooltip("The SO channel for the AR events")]
    [SerializeField] private AREventChannelSO arEventChannelSO;

    [Header("References")]
    [SerializeField] private SessionDataSO sessionDataSO;
    [SerializeField] private GameStateSO gameStateSO;

    [Header("Debug SO")]
    [SerializeField] private DebugUIEventChannelSO debugUIEventChannelSO;
    #endregion

    #region Private variables 
    /// <summary>
    /// The hashset for keeping track of the detected images
    /// </summary>
    private HashSet<string> detectedImages = new HashSet<string>();
    /// <summary>
    /// Reference for the ARTrackedImageManager from ARCoreSession
    /// </summary>
    private ARTrackedImageManager trackedImageManager;
    bool sessionTrackingFirstTimeDone;
    #endregion

    #region Unity methods
    void Awake() 
    {
        trackedImageManager = GetComponent<ARTrackedImageManager>();
        DisableTrackedImageManager();
    }

    void OnEnable() 
    {
        trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;

        uiEventsChannelSO.OnClosingUIEventRaised += EnableTrackedImageManager;
        uiEventsChannelSO.OnOpeningUIEventRaised += DisableTrackedImageManager;

        ARSession.stateChanged += OnARSessionStateChanged;
    }

    void OnDisable() 
    {
        trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;

        uiEventsChannelSO.OnClosingUIEventRaised -= EnableTrackedImageManager;
        uiEventsChannelSO.OnOpeningUIEventRaised -= DisableTrackedImageManager;
        
        ARSession.stateChanged -= OnARSessionStateChanged;
    }
    #endregion

    #region Coroutines
    private IEnumerator AddAllImagesToMutableReferenceImageLibraryAR()
    {
        // You can either add raw image bytes or use the extension method (used below) which accepts
        // a texture. To use a texture, however, its import settings must have enabled read/write
        // access to the texture.

        // Disable the component
        DisableTrackedImageManager();

        if (trackedImageManager == null) 
        {
            Debug.Log($"No {nameof(ARTrackedImageManager)} available.");
            
            yield break;
        }

        if (trackedImageManager.referenceLibrary == null) 
        {
            Debug.Log("[AFP] PRE Library is null");
        } 
        else 
        {
            Debug.Log("[AFP] PRE Library exists");
        }

        trackedImageManager.referenceLibrary = trackedImageManager.CreateRuntimeLibrary();

        if (trackedImageManager.referenceLibrary == null) 
        {
            Debug.Log("[AFP] POST Library is null");
        } 
        else 
        {
            Debug.Log("[AFP] POST Library exists");
        }

        if (trackedImageManager.referenceLibrary is MutableRuntimeReferenceImageLibrary mutableLibrary)
        {
            foreach (var point in sessionDataSO.PointsOfInterest.Points)
            {
                if (!point.alreadyDetected)
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
            }

            Debug.Log("[AFP] Library Count: " + trackedImageManager.referenceLibrary.count);

            // If we are still in the loading state it means that the scene it's just starting to run
            // so we can directly enable the ARTrackedImageManager
            if (gameStateSO.CurrentGameState == GameState.Loading)
            {
                EnableTrackedImageManager();
                gameStateSO.UpdateGameState(GameState.Tracking);    
            }        
        }
        else
        {
            Debug.LogError($"[ARP]The reference image library is not mutable.");
        }     
    }
    #endregion

    #region Callbacks
    private void EnableTrackedImageManager()
    {
        trackedImageManager.enabled = true;
        debugUIEventChannelSO.RaiseDebugEvent(trackedImageManager.enabled.ToString());
    }

    private void DisableTrackedImageManager()
    {
        trackedImageManager.enabled = false;
        debugUIEventChannelSO.RaiseDebugEvent(trackedImageManager.enabled.ToString());
    }

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs) 
    {
        // Go through all tracked images that have been added (-> new markers detected)
        foreach (var trackedImage in eventArgs.added) 
        {
            // Get the name of the reference image to search for its hash inside the detectedImages hashset
            var imageName = trackedImage.referenceImage.name;

            // If the hash is NOT inside the hashset (the image was never detected) MAYBE IT'S NOT NECESSARY
            if (!detectedImages.Contains(trackedImage.referenceImage.name)) 
            {
                // Raise an ARImageRecognized event passing the name of the image
                arEventChannelSO.RaisePOIDetectionEvent(trackedImage.referenceImage.name);

                // Change the current game state
                gameStateSO.UpdateGameState(GameState.POIPopUp);

                Debug.Log("[AFP] IMAGE DETECTED");

                // Add the hash to the hashset
                detectedImages.Add(trackedImage.referenceImage.name);

                // Check if the dictionay is not null
                if (sessionDataSO.PointsOfInterest.ImageNameAndPOI_Dict == null) 
                {
                    Debug.Log("ImageNameAndPOI_Dict is null");
                }

                // Flag this image as already detected
                sessionDataSO.PointsOfInterest.ImageNameAndPOI_Dict[trackedImage.referenceImage.name].alreadyDetected = true;

                // Temporarily disable the ARTrackedImageManager
                DisableTrackedImageManager();

                // Refresh the image library
                StartCoroutine(AddAllImagesToMutableReferenceImageLibraryAR());
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

    private void OnARSessionStateChanged(ARSessionStateChangedEventArgs eventArgs) 
    {
        Debug.Log("[ARP] ARSession.state: " + eventArgs.state);
        Debug.Log("[ARP] CurrentGameState: " + gameStateSO.CurrentGameState);

        if (gameStateSO.CurrentGameState == GameState.Loading)
        {
            if ((eventArgs.state == ARSessionState.SessionTracking) && !sessionTrackingFirstTimeDone) 
            {
                sessionTrackingFirstTimeDone = true;

                StartCoroutine(AddAllImagesToMutableReferenceImageLibraryAR());
            }
        }   
    }
    #endregion
}
