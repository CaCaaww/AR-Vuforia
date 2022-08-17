using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StartGameUiController : MonoBehaviour
{
    #region Inspector
    [Header("SEND Event Channels")]
    [SerializeField] private UIEventsChannelSO uiEventsChannelSO;
    [SerializeField] private AREventChannelSO arEventChannelSO;

    [Header("SO References")]
    [SerializeField] private GameStateSO gameStateSO;
    [SerializeField] private SessionDataSO sessionDataSO;

    [Header("Start Game UI")]
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI introText;
    [SerializeField] private Button startButton;
    #endregion

    #region Variables
    private Canvas startGameCanvas;
    #endregion
    
    #region Unity methods
    private void OnEnable()
    {
        arEventChannelSO.OnFinishedCreatingImageTargets += HandleImageTargetsCreationFinishedEvent;
    }

    private void OnDisable()
    {
        arEventChannelSO.OnFinishedCreatingImageTargets -= HandleImageTargetsCreationFinishedEvent;
    }


    private void Awake()
    {
        startGameCanvas = GetComponent<Canvas>();

        // Enable the canvas just to be sure
        startGameCanvas.enabled = true;
    }
    #endregion

    #region Callbacks
    private void HandleImageTargetsCreationFinishedEvent()
    {
        // Switch to the intro GameState
        gameStateSO.UpdateGameState(GameState.Intro);

        // Set the title text for the current session 
        titleText.text = sessionDataSO.TitleText;

        // Set the intro text for the current session  
        introText.text = sessionDataSO.IntroText;

        // Add a behaviour to the button through code 
        startButton.onClick.AddListener(() => 
        { 
            // Go to the tracking GameState
            gameStateSO.UpdateGameState(GameState.Tracking);

            // Disable the start game canvas 
            startGameCanvas.enabled = false;

            // Notify that the UI it's closing
            uiEventsChannelSO.RaiseClosingUIEvent();

            // Notify that the game is starting
            uiEventsChannelSO.RaiseStartGameEvent();
        });

        // Set the button as interactable
        startButton.interactable = true;
    }
    #endregion
}
