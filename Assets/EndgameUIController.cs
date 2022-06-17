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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #endregion

    #region Callbacks
    private void HandleEndgameReached(bool isVictory, string endgameText) 
    {
        endgameDescription.text = endgameText;
        endgameCanvas.enabled = true;
        
    }
    #endregion
}
