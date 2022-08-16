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
    /// <summary>
    /// Flag to check if it's the first time that the AR is in tracking mode
    /// </summary>
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
    /// <summary>
    /// Coroutine to build the Reference Image Library
    /// </summary>
    private IEnumerator BuildMutableReferenceImageLibraryAR()
    {
        // You can either add raw image bytes or use the extension method (used below) which accepts
        // a texture. To use a texture, however, its import settings must have enabled read/write
        // access to the texture.

        // Disable the component
        DisableTrackedImageManager();

        // Check if the image tracker is null
        if (trackedImageManager == null) 
        {
            Debug.Log($"No {nameof(ARTrackedImageManager)} available.");
            
            yield break;
        }

        // Check if the reference library is null
        if (trackedImageManager.referenceLibrary == null) 
        {
            Debug.Log("[AFP] PRE Library is null");
        } 
        else 
        {
            Debug.Log("[AFP] PRE Library exists");
        }

        // Build the reference library at runtime
        trackedImageManager.referenceLibrary = trackedImageManager.CreateRuntimeLibrary();

        if (trackedImageManager.referenceLibrary == null) 
        {
            Debug.Log("[AFP] POST Library is null");
        } 
        else 
        {
            Debug.Log("[AFP] POST Library exists");
        }

        // If the library is a mutable library
        if (trackedImageManager.referenceLibrary is MutableRuntimeReferenceImageLibrary mutableLibrary)
        {
            // For every p.o.i. in the session
            for (int i = 0; i < sessionDataSO.PointsOfInterest.Points.Count; i++)
            {
                // Check if the poi was already detected, if not
                if (!sessionDataSO.PointsOfInterest.Points[i].alreadyDetected)
                {
                    // Loop through every image inside the p.o.i.
                    foreach(KeyValuePair<string, Texture2D> image in sessionDataSO.PointsOfInterest.Points[i].imageNameAndTexture)
                    {
                        // If the image is readable
                        if (image.Value.isReadable)
                        {
                            // Schedule a job to add the image to the library
                            sessionDataSO.PointsOfInterest.Points[i].jobState = mutableLibrary.ScheduleAddImageWithValidationJob
                            (
                                image.Value,
                                image.Key,
                                null
                            );

                            // Yield until the the image is added to the library
                            yield return new WaitUntil(() => sessionDataSO.PointsOfInterest.Points[i].jobState.jobHandle.IsCompleted);

                            Debug.Log("[ARP] " + image.Key + " jobState: " + sessionDataSO.PointsOfInterest.Points[i].jobState.status);
                        }
                        // if The image is not readable
                        else
                        {
                            Debug.Log($"[ARP] Image {image.Key} must be readable to be added to the image library.");

                            yield return null;
                        }    
                    }
                }
            }

            Debug.Log("[AFP] Library Count: " + trackedImageManager.referenceLibrary.count);

            // If we are still in the loading state it means that the scene it's just starting to run
            // so we can notify that the reference library creation is completed 
            if (gameStateSO.CurrentGameState == GameState.Loading)
            {
                //EnableTrackedImageManager();
                //gameStateSO.UpdateGameState(GameState.Tracking);

                // Raise an event to notify that the reference library was created for the first time
                arEventChannelSO.RaiseReferenceLibraryFirstTimeCreatedEvent();
            }        
        }
        // If the reference library is not mutable
        else
        {
            Debug.LogError($"[ARP]The reference image library is not mutable.");
        }     
    }
    #endregion

    #region Callbacks
    /// <summary>
    /// Callback to enable the AR image tracker
    /// </summary>
    private void EnableTrackedImageManager()
    {
        trackedImageManager.enabled = true;
        debugUIEventChannelSO.RaiseDebugEvent(trackedImageManager.enabled.ToString());
    }

    /// <summary>
    /// Callback to disable the AR image tracker
    /// </summary>
    private void DisableTrackedImageManager()
    {
        trackedImageManager.enabled = false;
        debugUIEventChannelSO.RaiseDebugEvent(trackedImageManager.enabled.ToString());
    }

    /// <summary>
    /// Callback for when a tracked image status changes
    /// </summary>
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

                // Check if the dictionary is not null
                if (sessionDataSO.PointsOfInterest.ImageNameAndPOI == null) 
                {
                    Debug.Log("ImageNameAndPOI_Dict is null");
                }

                // Flag this image as already detected
                sessionDataSO.PointsOfInterest.ImageNameAndPOI[trackedImage.referenceImage.name].alreadyDetected = true;

                // Temporarily disable the ARTrackedImageManager
                DisableTrackedImageManager();

                // Refresh the image library
                StartCoroutine(BuildMutableReferenceImageLibraryAR());
            }
            // If the hash IS inside the hashset (the image was already detected)
            else 
            {
                // Continue the loop
                continue;
            }
        }

        //Go through all tracked images that have been removed
        foreach (var trackedImage in eventArgs.removed)
        {
            Debug.Log("[ARP] Image removed: " + trackedImage.name);
        }      
    }

    /// <summary>
    /// Callback for when the session state changes
    /// </summary>
    private void OnARSessionStateChanged(ARSessionStateChangedEventArgs eventArgs) 
    {
        Debug.Log("[ARP] ARSession.state: " + eventArgs.state);
        Debug.Log("[ARP] CurrentGameState: " + gameStateSO.CurrentGameState);

        // If the game is in the loading state
        if (gameStateSO.CurrentGameState == GameState.Loading)
        {
            // If the session is in the SessionTracking state and if it's first time that the game is tracking
            if ((eventArgs.state == ARSessionState.SessionTracking) && !sessionTrackingFirstTimeDone) 
            {
                // Disable the flag
                sessionTrackingFirstTimeDone = true;

                Debug.Log("[AFP] START BUILDING THE LIBRARY");

                // Build the reference library
                StartCoroutine(BuildMutableReferenceImageLibraryAR());
            }
        }   
    }
    #endregion
}
