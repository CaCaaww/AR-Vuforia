using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUIController : MonoBehaviour
{
    #region Inspector
    [Header("SEND Channels")]
    [SerializeField] private UIEventsChannelSO uiEventsChannelSO;

    [Header("SO References")]
    [SerializeField] private GameStateSO gameStateSO;
    [SerializeField] private SessionDataSO sessionDataSO;

    [Header("Inventory UI References")]
    [SerializeField] private Canvas inventoryCanvas;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button hintButton;
    [SerializeField] private TextMeshProUGUI hintLabel;
    [SerializeField] private TextMeshProUGUI poiCount;

    [Header("Where References")]
    [SerializeField] private Canvas whereCanvas;
    [SerializeField] private Button whereButton;
    [SerializeField] private Image whereButtonBackground;

    [Header("When References")]
    [SerializeField] private Canvas whenCanvas;
    [SerializeField] private Button whenButton;
    [SerializeField] private Image whenButtonBackground;

    [Header("How References")]
    [SerializeField] private Canvas howCanvas;
    [SerializeField] private Button howButton;
    [SerializeField] private Image howButtonBackground;
    #endregion

    #region Variables
    EPOIType currentPOITypeMenu;
    
    /// <summary>
    /// The message to show when the login is not successful
    /// </summary>
    private FMOD.Studio.EventInstance uiConfirm;
    #endregion

    #region Unity methods
    void Awake()
    {
        #if UNITY_EDITOR
        //sessionDataSO.Hints = 5;
        #endif

        SetupUI();
    }

    void OnEnable()
    {
        uiEventsChannelSO.OnOpeningUIEventRaised += HandleOpenInventoryUI;
    }

    void OnDisable()
    {
        uiEventsChannelSO.OnOpeningUIEventRaised -= HandleOpenInventoryUI;
    }
    #endregion
    
    #region Helper methods
    private void SetupUI()
    {
        // Select the where canvas as the default one when opening the inventory (disable the others)
        whereCanvas.enabled = false;
        whenCanvas.enabled = false;
        howCanvas.enabled = false;

        whereButtonBackground.color = Utils.buttonSelectedColor;

        currentPOITypeMenu = EPOIType.Where;

        whereButton.onClick.AddListener(WhereButtonBehaviour);
        whenButton.onClick.AddListener(WhenButtonBehaviour);
        howButton.onClick.AddListener(HowButtonBehaviour);

        closeButton.onClick.AddListener(CloseButtonBehaviour);

        hintLabel.text = "Get Hint (" + sessionDataSO.Hints + ")";

        poiCount.text = "0/" + sessionDataSO.PointsOfInterest.Points.Count + " Items Found";
        
        if (sessionDataSO.Hints == 0)
            {
                hintButton.interactable = false;
            }
        
        hintButton.onClick.AddListener(HintButtonBehaviour);
    }

    private void WhereButtonBehaviour()
    {
        currentPOITypeMenu = EPOIType.Where;

        whereCanvas.enabled = true;
        whereButtonBackground.color = Utils.buttonSelectedColor;

        whenCanvas.enabled = false;
        whenButtonBackground.color = Color.white;

        howCanvas.enabled = false;
        howButtonBackground.color = Color.white;
        
        //Sound for UI Confirmation
        uiConfirm = FMODUnity.RuntimeManager.CreateInstance("event:/UI/UI_Confirm");
        uiConfirm.start();
        uiConfirm.release();
        
    }

    private void WhenButtonBehaviour()
    {
        currentPOITypeMenu = EPOIType.When;

        whenCanvas.enabled = true;
        whenButtonBackground.color = Utils.buttonSelectedColor;
        
        whereCanvas.enabled = false;
        whereButtonBackground.color = Color.white;

        howCanvas.enabled = false; 
        howButtonBackground.color = Color.white;
        
        //Sound for UI Confirmation
        uiConfirm = FMODUnity.RuntimeManager.CreateInstance("event:/UI/UI_Confirm");
        uiConfirm.start();
        uiConfirm.release();
    }

    private void HowButtonBehaviour()
    {
        currentPOITypeMenu = EPOIType.How;

        howCanvas.enabled = true;
        howButtonBackground.color = Utils.buttonSelectedColor;

        whenCanvas.enabled = false;
        whenButtonBackground.color = Color.white;

        whereCanvas.enabled = false;
        whereButtonBackground.color = Color.white;
        
        //Sound for UI Confirmation
        uiConfirm = FMODUnity.RuntimeManager.CreateInstance("event:/UI/UI_Confirm");
        uiConfirm.start();
        uiConfirm.release();
    }
    
    private void CloseButtonBehaviour()
    {
        gameStateSO.UpdateGameState(GameState.Tracking);
        uiEventsChannelSO.RaiseClosingUIEvent();
        
        inventoryCanvas.enabled = false;
        whereCanvas.enabled = false;
        whenCanvas.enabled = false;
        howCanvas.enabled = false;
        
        //Sound for UI Confirmation
        uiConfirm = FMODUnity.RuntimeManager.CreateInstance("event:/UI/UI_Confirm");
        uiConfirm.start();
        uiConfirm.release();
    }

    private void HintButtonBehaviour()
    {
        if (sessionDataSO.Hints > 0f)
        {
            sessionDataSO.Hints--;

            hintLabel.text = "Get Hint (" + sessionDataSO.Hints + ")";

            if (sessionDataSO.Hints == 0)
            {
                hintButton.interactable = false;
            }

            uiEventsChannelSO.RaiseHintRequestedEvent();
            
            //Sound for UI Confirmation
            uiConfirm = FMODUnity.RuntimeManager.CreateInstance("event:/UI/UI_Confirm");
            uiConfirm.start();
            uiConfirm.release();
        } 
    }
    #endregion

    #region Callbacks
    private void HandleOpenInventoryUI()
    {
        if(!inventoryCanvas.enabled)
            return;

        //change the text for the poi count
        poiCount.text = (sessionDataSO.PointsOfInterest.HowPOIsFound.Count + sessionDataSO.PointsOfInterest.WhenPOIsFound.Count + sessionDataSO.PointsOfInterest.WherePOIsFound.Count)  + "/" + sessionDataSO.PointsOfInterest.Points.Count + " Items Found";
        switch (currentPOITypeMenu)
        {
            case EPOIType.Where:
                {
                    whereCanvas.enabled = true;
                    whenCanvas.enabled = false;
                    howCanvas.enabled = false;
                }
                break;
            case EPOIType.When:
                {
                    whereCanvas.enabled = false;
                    whenCanvas.enabled = true;
                    howCanvas.enabled = false;
                }
                break;
            case EPOIType.How:
                {
                    whereCanvas.enabled = false;
                    whenCanvas.enabled = false;
                    howCanvas.enabled = true;
                }
                break;
        }
    }
    #endregion
}
