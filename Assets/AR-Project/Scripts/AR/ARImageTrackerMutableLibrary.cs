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
    #region Classes
    enum EState
        {
            NoImagesAdded,
            AddImagesRequested,
            AddingImages,
            Done,
            Ignore,
        }
    #endregion

    #region Inspector
    [Header("SEND Channels")]
    /// <summary>
    /// The SO channel for the AR events
    /// </summary>
    [Tooltip("The SO channel for the AR events")]
    [SerializeField] private AREventChannelSO aREventChannelSO;

    [Header("References")]
    [SerializeField] private SessionDataSO sessionDataSO;

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

    EState m_State;

    int numberOfPois;
    #endregion

    #region Unity methods
    void Awake()
    {
         // Disable screen dimming
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        trackedImagesManager = GetComponent<ARTrackedImageManager>();
        trackedImagesManager.enabled = false;

        Debug.Log("[ARP] MEGATEST");

        int numberOfPois = sessionDataSO.PointsOfInterest.Points.Count;
        Debug.Log("[ARP] numberOfPois: " + numberOfPois);
    }

    void OnEnable()
    {
        trackedImagesManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    void OnDisable()
    {
        trackedImagesManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    IEnumerator Start()
    {
        yield return new WaitForSeconds(2);
        m_State = EState.AddImagesRequested;
        Debug.Log("[ARP] m_State: " + m_State);
    }

    void Update()
    {
        switch (m_State) 
        {
            case EState.AddImagesRequested: 
                {
                    // You can either add raw image bytes or use the extension method (used below) which accepts
                    // a texture. To use a texture, however, its import settings must have enabled read/write
                    // access to the texture.
                    for (int i = 0; i < numberOfPois; i++)
                    {
                        if (!sessionDataSO.PointsOfInterest.Points[i].image.isReadable) 
                        {
                            Debug.Log($"[ARP] Image {sessionDataSO.PointsOfInterest.Points[i].image.name} must be readable to be added to the image library.");
                            break;
                        }
                    }

                    if (trackedImagesManager == null)
                    {
                        Debug.Log($"No {nameof(ARTrackedImageManager)} available.");
                        break;
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
                        try 
                        {
                            foreach (var point in sessionDataSO.PointsOfInterest.Points) 
                            {
                                Debug.Log("[ARP] Foreach loop");
                                // Note: You do not need to do anything with the returned JobHandle, but it can be
                                // useful if you want to know when the image has been added to the library since it may
                                // take several frames.
                                point.jobState = mutableLibrary.ScheduleAddImageWithValidationJob
                                (
                                    point.image,
                                    point.imageName,
                                    null
                                );
                                Debug.Log("[ARP] " + point.imageName + " jobState: " + point.jobState.status);
                            }

                            m_State = EState.AddingImages;
                        } 
                        catch (InvalidOperationException e) 
                            {
                                Debug.Log($"[ARP] ScheduleAddImageWithValidationJob threw exception: {e.Message}");
                            }
                    } 
                    else
                    {
                        Debug.Log($"[ARP]The reference image library is not mutable.");
                    }

                    break;
                }
            case EState.AddingImages: 
                {
                    // Check for completion
                    bool done = true;
                    foreach (var point in sessionDataSO.PointsOfInterest.Points) 
                    {
                        if (!point.jobState.jobHandle.IsCompleted) 
                        {
                            Debug.Log("[ARP] 2a jobState: " + point.jobState.status);
                            done = false;
                            break;
                        }
                        else
                        {   
                            Debug.Log("[ARP] 2b jobState: " + point.jobState.status);
                        }
                    }

                    if (done) 
                    {
                        Debug.Log("[AFP] Library Count: " + trackedImagesManager.referenceLibrary.count);
                        trackedImagesManager.enabled = true;
                        
                        m_State = EState.Done;
                    }
                    break;
                }
            case EState.Done:
                {
                    Debug.Log("[ARP] State Done");

                    for (int i = 0; i < trackedImagesManager.referenceLibrary.count; i++)
                    {
                        Debug.Log("[ARP] Image name: " + trackedImagesManager.referenceLibrary[i].name);
                    }

                    m_State = EState.Ignore;
                    break;
                }
        }
    }
    #endregion 
    
    #region Helper methods
    private IEnumerator AddAllImagesToMutableReferenceImageLibraryAR(MutableRuntimeReferenceImageLibrary mutableLibrary)
    {   
        int numberOfPois = sessionDataSO.PointsOfInterest.Points.Count;

        Debug.Log("numberOfPois: " + numberOfPois);

        AddReferenceImageJobState job;

        for (int i = 0; i < numberOfPois; i++)
        {
            if (sessionDataSO.PointsOfInterest.Points[i].image.isReadable) 
            {
                job = mutableLibrary.ScheduleAddImageWithValidationJob(sessionDataSO.PointsOfInterest.Points[i].image, sessionDataSO.PointsOfInterest.Points[i].imageName, null);
                yield return new WaitUntil(() => job.jobHandle.IsCompleted);
            }

            Debug.Log("mutableLibrary.count = " + mutableLibrary.count);
        }


        trackedImagesManager.referenceLibrary = mutableLibrary;

        trackedImagesManager.enabled = true;
    } 
    #endregion

    #region Callbacks
    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        // Good reference: https://forum.unity.com/threads/arfoundation-2-image-tracking-with-many-ref-images-and-many-objects.680518/#post-4668326
        // https://github.com/Unity-Technologies/arfoundation-samples/issues/261#issuecomment-555618182

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
    }
    #endregion
}
