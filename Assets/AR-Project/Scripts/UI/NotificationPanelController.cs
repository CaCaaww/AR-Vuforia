using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotificationPanelController : MonoBehaviour
{
    #region Inspector
    [Header("LISTEN Channels")]
    /// <summary>
    /// The SO channel for the AR events
    /// </summary>
    [Tooltip("The SO channel for the AR events")]
    [SerializeField] private AREventChannelSO arEventChannelSO;

    [Header("SEND Channels")]
    /// <summary>
    /// The SO channel for the DebugUI events
    /// </summary>
    [Tooltip("The SO channel for the DebugUI events")]
    [SerializeField] private DebugUIEventChannelSO debugUIEventChannelSO;

    [Header("References")]
    [SerializeField] private Button alertButton;
    #endregion

    #region Private variables
    /// <summary>
    /// 
    /// </summary>
    private Canvas canvas;
    #endregion

    #region Unity methods
    private void OnEnable()
    {
        arEventChannelSO.OnARImageRecognized += HandleARImageRecognized;
    }

    private void OnDisable()
    {
        arEventChannelSO.OnARImageRecognized -= HandleARImageRecognized;
    }

    void Awake()
    {
        canvas = GetComponent<Canvas>();

        alertButton.onClick.AddListener(() => 
        { 
            debugUIEventChannelSO.RaiseDebugEvent();
            canvas.enabled = false;
        });
    }
    #endregion

    #region Internal Methods
    #endregion

    #region Callbacks
    private void HandleARImageRecognized(string imageName)
    {
        Debug.Log("Image Detected: " + imageName);

        canvas.enabled = true;
    }
    #endregion

}
