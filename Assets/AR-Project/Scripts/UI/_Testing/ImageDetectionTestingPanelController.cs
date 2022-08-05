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
    [Header("SO References")]
    [SerializeField] private GameStateSO gameStateSO;
    [SerializeField] private PointsOfInterestSO pointsOfInterestSO;
    
    [Header("Panel References")]
    [SerializeField] private Canvas canvas;
    [SerializeField] private TextMeshProUGUI objectTitle;
    [SerializeField] private RawImage objectImage;
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
            gameStateSO.UpdateGameState(GameState.Tracking);
            canvas.enabled = false;
            
        });

        gameStateSO.UpdateGameState(GameState.Tracking);
    }
    #endregion

    #region Internal Methods
    #endregion

    #region Callbacks
    private void HandleARImageRecognized(string imageName)
    {
        for (int i = 0; i < pointsOfInterestSO.Points.Count; i++)
        {
            if (pointsOfInterestSO.Points[i].imageNames[0] == imageName)
            {
                canvas.enabled = true;

                objectTitle.text = pointsOfInterestSO.Points[i].title;
                objectImage.texture = pointsOfInterestSO.Points[i].images[0];

                Debug.Log("[ARP] Image detected: " + objectTitle.text);

                return;
            }
        }    
    }
    #endregion
}
