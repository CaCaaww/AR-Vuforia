using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugUIPanelController : MonoBehaviour
{

    #region Inspector

    /// <summary>
    /// SO Channel to send debug event to the UI
    /// </summary>
    [Tooltip ("SO Channel to send debug event to the UI")]
    [SerializeField]
    private DebugUIEventChannelSO debugUIEventChannelSO = default;

    [SerializeField]
    private Canvas canvasDebugUI;

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
        
    }

    #endregion

    #region Helper Methods

    #endregion

    #region Callback Methods

    /// <summary>
    /// 
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    private void HandleDebugEventRaised()
    {
        canvasDebugUI.enabled = true;
        Debug.Log("Callback Called");
    }

    #endregion




    // Start is called before the first frame update
    void Start()
    {
        
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
