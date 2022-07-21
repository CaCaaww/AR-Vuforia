using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndgameUIController : MonoBehaviour
{
    #region Inspector
    [Header("LISTEN Channels")]
    [SerializeField] UIEventsChannelSO uiEventsChannelsSO;

    [Header("References")]
    [SerializeField] Canvas endgameCanvas;
    [SerializeField] TextMeshProUGUI endgameTimerText;
    [SerializeField] TextMeshProUGUI endgameDescription;
    #endregion

    #region Unity methods
    private void OnEnable()
    {
        uiEventsChannelsSO.OnEndgameReachedEventRaised += HandleEndgameReached;
    }

    private void OnDisable()
    {
        uiEventsChannelsSO.OnEndgameReachedEventRaised -= HandleEndgameReached;
    }

    void Awake()
    {
        // Disable the canvas just to be sure
        endgameCanvas.enabled = false;
    }
    #endregion

    #region Callbacks
    private void HandleEndgameReached(bool isVictory, string endgameText, TimeSpan timePlaying) 
    {
        endgameTimerText.text = "You played for " + timePlaying.Minutes + " minutes and " + timePlaying.Seconds + " seconds";
        endgameDescription.text = endgameText;
        endgameCanvas.enabled = true;       
    }
    #endregion
}
