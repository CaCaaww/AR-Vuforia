using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopUpPanelController : MonoBehaviour
{
    #region Inspector
    [Header("LISTEN/SEND Channels")]
    /// <summary>
    /// The SO channel for the UI events
    /// </summary>
    [Tooltip("The SO channel for the UI events")]
    [SerializeField] private UIEventsChannelSO uiEventChannelSO;
    
    [Header("References")]
    [Header("SO References")]
    [SerializeField] private GameStateSO gameStateSO;
    [SerializeField] private PointsOfInterestSO pointsOfInterestSO;
    [SerializeField] private POIIconCollectionSO poiIconCollectionSO;
    [SerializeField] private NPCAvatarCollectionSO npcAvatarCollectionSO;

    [Header("Panel References")]
    [SerializeField] private Canvas canvas;
    [SerializeField] private TextMeshProUGUI poiTitle;
    [SerializeField] private Image poiIconImage;
    [SerializeField] private Image poiTellerImage;
    [SerializeField] private TextMeshProUGUI poiDescription;
    [SerializeField] private Button closeButton;
    #endregion

    #region UnityMethods
    private void OnEnable()
    {
        uiEventChannelSO.OnPOIFoundEventRaised += ViewPOI;
        uiEventChannelSO.OnPOIViewEventRaised += ViewPOI;
    }

    private void OnDisable()
    {
        uiEventChannelSO.OnPOIFoundEventRaised -= ViewPOI;
        uiEventChannelSO.OnPOIViewEventRaised -= ViewPOI;
    }

    private void Awake()
    {
        // Disable the canvas (just to be sure)
        canvas.enabled = false;

        closeButton.onClick.AddListener(CloseButtonBehaviour);
    }
    #endregion

    #region Helper methods
    private void CloseButtonBehaviour()
    {
        switch (gameStateSO.CurrentGameState)
        {
            case GameState.POIPopUp:
                {
                    gameStateSO.UpdateGameState(GameState.Tracking);
                    uiEventChannelSO.RaiseClosingUIEvent();
                }
                break;
            case GameState.UI:
                {
                    gameStateSO.UpdateGameState(GameState.UI);
                }
                break;
        }

        canvas.enabled = false;     
    }
    #endregion

    #region Callbacks
    private void ViewPOI(PointOfInterest poi)
    {
        Debug.Log("[ARP] Image detected/viewing: " + poi.title);

        canvas.enabled = true;

        poiTitle.text = poi.title;

        poiDescription.text = poi.description;

        poiIconImage.sprite = poiIconCollectionSO.GetIconByType(poi.iconType);

        poiTellerImage.sprite = npcAvatarCollectionSO.GetAvatarSpriteByID(poi.avatarID);

    }
    #endregion
}
