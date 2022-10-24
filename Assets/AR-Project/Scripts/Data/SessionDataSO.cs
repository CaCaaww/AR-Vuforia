using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New SessionData", menuName = "Data/Session Data SO")]
public class SessionDataSO : ScriptableObject
{
    #region Inspector
    [Header("References")]
    /// <summary>
    /// SO containing all the AR trackable points of interest
    /// </summary>
    [Tooltip("SO containing all the AR trackable points of interest")]
    [SerializeField] PointsOfInterestSO pointsOfInterestSO;
    [Header("Session Parameters")]
    /// <summary>
    /// The player ID
    /// </summary>
    [Tooltip("The player ID")]
    [SerializeField] int playerId;
    /// <summary>
    /// The session ID
    /// </summary>
    [Tooltip("The session ID")]
    [SerializeField] int sessionId;
    /// <summary>
    /// The number of hints at the start of the session
    /// </summary>
    [Tooltip("The number of hints at the start of the session")]
    [SerializeField] int hints;
    /// <summary>
    /// The title text for the session
    /// </summary>
    [Tooltip("The title text for the session")]
    [SerializeField] string titleText;
    /// <summary>
    /// The intro text for the session
    /// </summary>
    [Tooltip("The intro text for the session")]
    [SerializeField] string introText;
    /// <summary>
    /// The victory text for the session
    /// </summary>
    [Tooltip("The victory text for the session")]
    [SerializeField] string victoryText;
    /// <summary>
    /// The defeat text for the session
    /// </summary>
    [Tooltip("The defeat text for the session")]
    [SerializeField] string defeatText;
    /// <summary>
    /// True if we are resuming an unifished game session
    /// </summary>
    [Tooltip("True if we are resuming an unifished game session")]
    [SerializeField] bool resumeSession;
    #endregion

    #region Variables
    #endregion

    #region Properties
    public int PlayerId { get => playerId; set => playerId = value; }
    public int SessionId { get => sessionId; set => sessionId = value; }
    public int Hints { get => hints; set => hints = value; }
    public string TitleText { get => titleText; set => titleText = value; }
    public string IntroText { get => introText; set => introText = value; }
    public string VictoryText { get => victoryText; set => victoryText = value; }
    public string DefeatText { get => defeatText; set => defeatText = value; }
    public bool ResumeSession { get => resumeSession; set => resumeSession = value; }
    public PointsOfInterestSO PointsOfInterest { get => pointsOfInterestSO; } 
    #endregion
}
