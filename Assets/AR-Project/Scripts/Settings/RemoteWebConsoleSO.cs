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
    /// The nickname parameter name expected by the join gate
    /// </summary>
    [Tooltip("Nickname for the current session")]
    [SerializeField] private string nicknameParameter = "?nickname";

    /// <summary>
    /// Nickname value for the current session
    /// </summary>
    [Tooltip("Nickname for the current session")]
    [SerializeField] private string nicknameValue = "";

    /// <summary>
    /// The password parameter name expected for the current session
    /// </summary>
    [Tooltip("The password parameter name expected for the current session")]
    [SerializeField] private string passwordParameter = "?code";

    /// <summary>
    /// Password value for the current session
    /// </summary>
    [Tooltip("Password value for the current session")]
    [SerializeField] private string passwordValue = "";
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
    /// Return the nickname parameter name
    /// </summary>
    public string NicknameParameter => nicknameParameter;

    /// <summary>
    /// Return the nickname value for joining the session
    /// </summary>
    public string NicknameValue { get => nicknameValue; set => nicknameValue = value; }

    /// <summary>
    /// Return the access code parameter name
    /// </summary>
    public string PasswordParameter => passwordParameter;

    /// <summary>
    /// Return the access code value for joining the session
    /// </summary>
    public string PasswordValue { get => passwordValue; set => passwordValue = value; }
    #endregion
}
