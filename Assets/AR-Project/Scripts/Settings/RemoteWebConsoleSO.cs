using System;
using UnityEngine;

/// <summary>
/// A list of settings to connect to the remote webapp console
/// </summary>
[CreateAssetMenu(fileName = "New RemoteWebConsole", menuName = "Settings/RemoteWebConsole")]
public class RemoteWebConsoleSO : ScriptableObject
{
    #region  Inspector
    /// <summary>
    /// Webapp host server, remember to add complete URL
    /// </summary>
    [Tooltip("Webapp host server, remember to add complete URL")]
    [SerializeField] private string host = "https://test1.dedalus.cc/arGame/public/api/";
    
    /// <summary>
    /// API gate for the join operation
    /// </summary>
    [Tooltip("API gate for the join operation")]
    [SerializeField] private string joinGate = "join";

    /// <summary>
    /// Access code for the current session
    /// </summary>
    [Tooltip("Access code for the current session")]
    [SerializeField] private string accessCode = "A8gk0";
    #endregion 

    #region Properties
    /// <summary>
    /// Return the webapp host server URL
    /// </summary>
    public string Host => host;

    /// <summary>
    /// Return the complete URL for the join gate
    /// </summary>
    public string JoinGate => String.Concat(host, joinGate);

    /// <summary>
    /// Return the access code for joining the session
    /// </summary>
    public string AccessCode => accessCode;
    #endregion
}
