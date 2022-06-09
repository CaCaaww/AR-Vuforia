using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ImageDetectionTestingPanelController : MonoBehaviour
{
    #region Inspector
    [Header("LISTEN Channels")]
    /// <summary>
    /// The SO channel for the AR events
    /// </summary>
    [Tooltip("The SO channel for the AR events")]
    [SerializeField] private AREventChannelSO arEventChannelSO;

    [Header("References")]
    [SerializeField] private Canvas canvas;
    [SerializeField] private TextMeshProUGUI objectTitle;
    [SerializeField] private RawImage objectImage;
    [SerializeField] private PointsOfInterestSO pointsOfInterestSO;
    [SerializeField] private Button closeButton;
    #endregion

    #region Unity methods
    private void OnEnable()
    {
        arEventChannelSO.OnPOIDetected += HandleARImageRecognized;
    }

    private void OnDisable()
    {
        arEventChannelSO.OnPOIDetected -= HandleARImageRecognized;
    }

    void Awake()
    {
        canvas = GetComponent<Canvas>();

        closeButton.onClick.AddListener(() =>
        {   
            canvas.enabled = false;
        });
    }
    #endregion

    #region Internal Methods
    #endregion

    #region Callbacks
    private void HandleARImageRecognized(string imageName)
    {

        Debug.Log("[ARP] Image detected: " + pointsOfInterestSO.ImageNameAndTitle[imageName]);

        canvas.enabled = true;

        objectTitle.text = pointsOfInterestSO.ImageNameAndTitle[imageName];
        objectImage.texture = pointsOfInterestSO.ImageNameAndTexture[imageName];

        /*foreach (var point in pointsOfInterestSO.Points)
        {
            if (point.imageName == imageName)
            {
                objectTitle.text = point.imageName;
                objectImage.texture = point.image;

                break;
            }
        }*/
    }
    #endregion
}
