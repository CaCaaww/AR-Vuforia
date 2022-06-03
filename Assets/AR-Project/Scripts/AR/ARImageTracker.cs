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
public class ARImageTracker : MonoBehaviour
{
    #region Inspector
    [Header("SEND Channels")]
    /// <summary>
    /// The SO channel for the AR events
    /// </summary>
    [Tooltip("The SO channel for the AR events")]
    [SerializeField] private AREventChannelSO aREventChannelSO;
    #endregion

    #region Private variables 
    /// <summary>
    /// The hashset for keeping track of the detected images
    /// </summary>
    private HashSet<string> detectedImages = new HashSet<string>();
    /// <summary>
    /// Reference for the ARTrackedImageManager from ARCoreSession
    /// </summary>
    private ARTrackedImageManager _trackedImagesManager;
    #endregion
         
    #region Unity methods
    void Awake()
    {
        _trackedImagesManager = GetComponent<ARTrackedImageManager>();
    }

    void OnEnable()
    {
        _trackedImagesManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    void OnDisable()
    {
        _trackedImagesManager.trackedImagesChanged -= OnTrackedImagesChanged;
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
