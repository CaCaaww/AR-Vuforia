using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class SolutionUIController : MonoBehaviour
{
    #region Inspector
    [Header("SEND Channels")]
    [SerializeField] private UIEventsChannelSO uiEventsChannelSO;

    [Header("SO References")]
    [SerializeField] private GameStateSO gameStateSO;
    [SerializeField] private PointsOfInterestSO pointOfInterestSO;
    

    [Header("Solution UI References")]
    [SerializeField] private Canvas solutionCanvas;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button solveButton;

    [Header("Warning Popup UI References")] 
    [SerializeField] private Canvas warningCanvas;


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
    private void Awake()
    {
        SetupUI();
    }

    void OnEnable()
    {
        uiEventsChannelSO.OnOpeningUIEventRaised += HandleOpenSolutionUI;
    }

    void OnDisable()
    {
        uiEventsChannelSO.OnOpeningUIEventRaised -= HandleOpenSolutionUI;
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

        solveButton.onClick.AddListener(SolveButtonBehaviour);
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
        
        solutionCanvas.enabled = false;
        whereCanvas.enabled = false;
        whenCanvas.enabled = false;
        howCanvas.enabled = false;
        
        //Sound for UI Confirmation
        uiConfirm = FMODUnity.RuntimeManager.CreateInstance("event:/UI/UI_Confirm");
        uiConfirm.start();
        uiConfirm.release();
    }

    private void SolveButtonBehaviour()
    {
        
        //check if each category has a selection, if not enable the popup
        if (pointOfInterestSO.HowPOIChosenAsSolutionId != 0 && pointOfInterestSO.WherePOIChosenAsSolutionId != 0 &&
            pointOfInterestSO.WhenPOIChosenAsSolutionId != 0)
        {
            uiEventsChannelSO.RaiseSolutionGivenEvent();
        
            solutionCanvas.enabled = false;
            whereCanvas.enabled = false;
            whenCanvas.enabled = false;
            howCanvas.enabled = false;
        
            //Sound for UI Confirmation
            uiConfirm = FMODUnity.RuntimeManager.CreateInstance("event:/UI/UI_Confirm");
            uiConfirm.start();
            uiConfirm.release();
        }
        else
        {
            warningCanvas.enabled = true;
            whereCanvas.enabled = false;
            whenCanvas.enabled = false;
            howCanvas.enabled = false;
        }
        
    }
    #endregion

    #region Callbacks
    private void HandleOpenSolutionUI()
    {
        if (!solutionCanvas.enabled)
            return;

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
