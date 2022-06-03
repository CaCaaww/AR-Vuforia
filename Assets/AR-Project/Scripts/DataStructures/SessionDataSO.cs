using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New SessionData", menuName = "Data/Session Data SO")]
public class SessionDataSO : ScriptableObject
{
    #region Inspector
    [Header("References")]
    /// <summary>
    /// SO containing all the trackable points of interest
    /// </summary>
    [Tooltip("SO containing all the trackable points of interest")]
    [SerializeField] PointsOfInterestSO pointsOfInterestSO;
    [Header("Session Parameters")]
    /// <summary>
    /// The number of boosts at the start of the session
    /// </summary>
    [Tooltip("The number of boosts at the start of the session")]
    [SerializeField] int boostsAtStart;
    #endregion

    #region Variables
    #endregion

    #region Properties
    #endregion
}
