using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugUIController : MonoBehaviour
{

    #region Inspector
    /// <summary>
    /// SO Channel to send debug event to the UI
    /// </summary>
    [Tooltip ("SO Channel to send debug event to the UI")]
    [SerializeField]
    private DebugUIEventChannelSO debugUIEventChannelSO = default;

    [SerializeField]
    private TextMeshProUGUI debugText;
    #endregion

    #region Variables

    #endregion

    #region Properties

    #endregion

    #region Unity Methods

    private void OnEnable()
    {
        debugUIEventChannelSO.OnDebugEventRaised += HandleDebugEventRaised;
    }

    private void OnDisable()
    {
        debugUIEventChannelSO.OnDebugEventRaised -= HandleDebugEventRaised;
    }

    #endregion

    #region Helper Methods

    #endregion

    #region Callback Methods
    /// <summary>
    /// 
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    private void HandleDebugEventRaised(string text)
    {
        debugText.enabled = true;
        debugText.text = text;
        //Debug.Log("Debug Callback Called");
    }
    #endregion
}
