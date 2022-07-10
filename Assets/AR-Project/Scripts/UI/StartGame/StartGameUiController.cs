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

    [Header("SO References")]
    [SerializeField] private GameStateSO gameStateSO;
    [SerializeField] private SessionDataSO sessionDataSO;

    [Header("Start Game UI")]
    [SerializeField] private TextMeshProUGUI introText;
    [SerializeField] private Button startButton;
    #endregion

    #region Variables
    private Canvas startGameCanvas;
    #endregion
    
    #region Unity methods
    private void Awake()
    {
        startGameCanvas = GetComponent<Canvas>();

        // Enable the canvas just to be sure
        startGameCanvas.enabled = true;

        introText.text = sessionDataSO.IntroText;

        startButton.onClick.AddListener(() => 
        { 
            gameStateSO.UpdateGameState(GameState.Tracking);

            startGameCanvas.enabled = false;

            uiEventsChannelSO.RaiseClosingUIEvent();

            uiEventsChannelSO.RaiseStartGameEvent();
        });
    }
    #endregion
}
