using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class EndgameUIController : MonoBehaviour
{
    #region Inspector
    [Header("LISTEN Channels")]
    [SerializeField] UIEventsChannelSO uiEventsChannelsSO;

    [Header("References")]
    [SerializeField] Canvas endgameCanvas;
    [SerializeField] TextMeshProUGUI headerText;
    [SerializeField] TextMeshProUGUI descriptionText;
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] Button exitButton;
    #endregion

    #region Unity methods
    private void OnEnable()
    {
        uiEventsChannelsSO.OnEndgameReachedEventRaised += HandleEndgameReached;
        uiEventsChannelsSO.OnGamePlaytimeReceveidEventRaised += HandleGamePlaytimeReceveidEvent;
    }

    private void OnDisable()
    {
        uiEventsChannelsSO.OnEndgameReachedEventRaised -= HandleEndgameReached;
        uiEventsChannelsSO.OnGamePlaytimeReceveidEventRaised -= HandleGamePlaytimeReceveidEvent;
    }

    void Awake()
    {
        // Disable the canvas just to be sure
        endgameCanvas.enabled = false;
    }

    void Start()
    {
        exitButton.GetComponent<Button>();
    }
    #endregion

    #region Callbacks
    private void HandleEndgameReached(bool isVictory, string endgameText, TimeSpan timePlaying) 
    {
        // Set the header text
        headerText.text = isVictory ? "Victory" : "Defeat";

        // Set the description text
        descriptionText.text = endgameText;

        // Set the timer text
        //timerText.text = "You played for " + timePlaying.Minutes + " minutes and " + timePlaying.Seconds + " seconds";

        // Add a quit function to the exit button
        //exitButton.onClick.AddListener(() => Utils.QuitGame());
        exitButton.onClick.AddListener(() => SceneManager.LoadSceneAsync("01-LoginScene"));

        // Enable the canvas
        endgameCanvas.enabled = true;       
    }

    private void HandleGamePlaytimeReceveidEvent(int playtime)
    {
        // Create a TimeSpan from the total playtime.
        TimeSpan timePlaying = TimeSpan.FromSeconds(playtime);

        // Set the timer text
        timerText.text = "You played for " + timePlaying.Minutes + " minutes and " + timePlaying.Seconds + " seconds";
    }
    #endregion
}
