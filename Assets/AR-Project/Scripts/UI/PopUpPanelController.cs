using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopUpPanelController : MonoBehaviour
{
    #region Inspector
    [Header("LISTEN Channels")]
    /// <summary>
    /// The SO channel for the AR events
    /// </summary>
    [Tooltip("The SO channel for the AR events")]
    [SerializeField] private AREventChannelSO arEventChannelSO;

    [Header("SEND Channels")]
    
    [Header("References")]
    [Header("SO References")]
    [SerializeField] private GameStateSO gameStateSO;
    [SerializeField] private PointsOfInterestSO pointsOfInterestSO;

    [Header("Panel References")]
    [SerializeField] private Canvas canvas;
    [SerializeField] private TextMeshProUGUI poiTitle;
    [SerializeField] private RawImage poiTellerImage;
    [SerializeField] private RawImage poiImage;
    [SerializeField] private TextMeshProUGUI poiDescription;
    [SerializeField] private Button closeButton;
    #endregion

    #region UnityMethods
    private void OnEnable()
    {
        arEventChannelSO.OnPOIDetected += HandlePOIDetection;
    }

    private void OnDisable()
    {
        arEventChannelSO.OnPOIDetected -= HandlePOIDetection;
    }

    private void Awake()
    {
        // Disable the canvas (just to be sure)
        canvas.enabled = false;

        closeButton.onClick.AddListener(() =>
        {
            gameStateSO.UpdateGameState(GameState.Tracking);
            canvas.enabled = false;
        });
    }
    #endregion

    #region Callbacks
    private void HandlePOIDetection(string imageName)
    {
        Debug.Log("[ARP] Image detected: " + pointsOfInterestSO.ImageNameAndTitle[imageName]);

        gameStateSO.UpdateGameState(GameState.POIPopUp);

        canvas.enabled = true;

        poiTitle.text = pointsOfInterestSO.ImageNameAndTitle[imageName];
        poiImage.texture = pointsOfInterestSO.ImageNameAndTexture[imageName];
    }
    #endregion
}
