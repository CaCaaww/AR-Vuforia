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
    /// The number of hints at the start of the session
    /// </summary>
    [Tooltip("The number of hints at the start of the session")]
    [SerializeField] int hints;
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
    [Tooltip("The defaeat text for the session")]
    [SerializeField] string defeatText;
    #endregion

    #region Variables
    #endregion

    #region Properties
    public int Hints { get => hints; set => hints = value; }
    public string IntroText { get => introText; set => introText = value; }
    public string VictoryText { get => victoryText; set => victoryText = value; }
    public string DefeatText { get => defeatText; set => defeatText = value; }
    public PointsOfInterestSO PointsOfInterest { get => pointsOfInterestSO; } 
    #endregion
    }
