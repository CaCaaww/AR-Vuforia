using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#region Static classes
public enum EClueType
{
    Where,
    When,
    How
}

[Serializable]
public class PointOfInterest
{
    public string name;
    public string description;
    public EClueType poiType;
    public string imageName;
    public Texture2D image;
}
#endregion

/// <summary>
/// Data structure to store all the points of interest
/// </summary>
[CreateAssetMenu(fileName = "New PointsOfInterestSO", menuName = "Data/Points Of Interest SO")]
public class PointsOfInterestSO : ScriptableObject
{
    #region Inspector
    [Header("References")]
    /// <summary>
    /// The list of all the points of interest for the session
    /// </summary>
    [Tooltip("The list of all the points of interest for the session")]
    [SerializeField]
    private List<PointOfInterest> points = new List<PointOfInterest>();
    #endregion

    #region Public properties
    public List<PointOfInterest> Points { get { return points; } }
    #endregion 
}

