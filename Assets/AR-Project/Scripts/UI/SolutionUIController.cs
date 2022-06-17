using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SolutionUIController : MonoBehaviour
{
    #region Inspector
    [Header("SEND Channels")]
    [SerializeField] private GameStateSO gameStateSO;
    [SerializeField] private UIEventsChannelSO uiEventsChannelSO;

    [Header("Solution UI References")]
    [SerializeField] private Canvas solutionCanvas;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button solveButton;

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

    #region Properties
    #endregion

    #region Unity methods
    private void Awake()
    {
        SetupUI();
    }
    #endregion

    #region Helper methods
    private void SetupUI()
    {
        // Select the where canvas as the default one when opening the inventory (disable the others)
        whereCanvas.enabled = true;
        whenCanvas.enabled = false;
        howCanvas.enabled = false;

        whereButtonBackground.color = Utils.buttonSelectedColor;

        whereButton.onClick.AddListener(WhereButtonBehaviour);
        whenButton.onClick.AddListener(WhenButtonBehaviour);
        howButton.onClick.AddListener(HowButtonBehaviour);

        closeButton.onClick.AddListener(CloseButtonBehaviour);

        solveButton.onClick.AddListener(SolveButtonBehaviour);
    }

    private void WhereButtonBehaviour()
    {
        whereCanvas.enabled = true;
        whereButtonBackground.color = Utils.buttonSelectedColor;

        whenCanvas.enabled = false;
        whenButtonBackground.color = Color.white;

        howCanvas.enabled = false;
        howButtonBackground.color = Color.white;
    }

    private void WhenButtonBehaviour()
    {
        whenCanvas.enabled = true;
        whenButtonBackground.color = Utils.buttonSelectedColor;
        
        whereCanvas.enabled = false;
        whereButtonBackground.color = Color.white;

        howCanvas.enabled = false; 
        howButtonBackground.color = Color.white;
    }

    private void HowButtonBehaviour()
    {
        howCanvas.enabled = true;
        howButtonBackground.color = Utils.buttonSelectedColor;

        whenCanvas.enabled = false;
        whenButtonBackground.color = Color.white;

        whereCanvas.enabled = false;
        whereButtonBackground.color = Color.white;
    }
    
    private void CloseButtonBehaviour()
    {
        gameStateSO.UpdateGameState(GameState.Tracking);
        solutionCanvas.enabled = false;
    }

    private void SolveButtonBehaviour()
    {
        uiEventsChannelSO.RaiseSolutionGivenEvent();
    }
    #endregion
}
