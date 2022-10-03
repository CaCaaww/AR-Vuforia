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
    /// <summary>
    /// True if the AR POIs are revealed automatically after some time
    /// </summary>
    //[Tooltip("True if the AR POIs are revealed automatically after some time")]
    //[SerializeField] bool autoreveal;
    /// <summary>
    /// Logical duration percentage for when starting to autoreveal AR and "not AR" POIs together
    /// </summary>
    //[Tooltip("Logical duration percentage for when starting to autoreveal AR and \"not AR\" POIs together")]
    //[SerializeField] int autorevealPercentage;
    /// <summary>
    /// How many POIs to autoreveal at the same time
    /// </summary>
    //[Tooltip("How many POIs to autoreveal at the same time")]
    //[SerializeField] int autorevealNumber;
    /// <summary>
    /// Interval in seconds to autoreveal the POIs
    /// </summary>
    //[Tooltip("How many POIs to autoreveal at the same time")]
    //[SerializeField] int autorevealTimer;
    #endregion

    #region Variables
    #endregion

    #region Properties
    public int Hints { get => hints; set => hints = value; }
    public string TitleText { get => titleText; set => titleText = value; }
    public string IntroText { get => introText; set => introText = value; }
    public string VictoryText { get => victoryText; set => victoryText = value; }
    public string DefeatText { get => defeatText; set => defeatText = value; }
    public bool ResumeSession { get => resumeSession; set => resumeSession = value; }
    //public bool Autoreveal { get => autoreveal; set => autoreveal = value; }
    //public int AutorevealPercentage { get => autorevealPercentage; set => autorevealPercentage = value; }
    //public int AutorevealNumber { get => autorevealNumber; set => autorevealNumber = value; }
    //public int AutorevealTimer { get => autorevealTimer; set => autorevealTimer = value; }
    public PointsOfInterestSO PointsOfInterest { get => pointsOfInterestSO; } 
    #endregion
}
