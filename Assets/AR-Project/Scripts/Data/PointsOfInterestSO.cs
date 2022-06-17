using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.XR.ARSubsystems;

#region Static classes
public enum EPOIType
{
    Where,
    When,
    How
}

[Serializable]
public class PointOfInterest
{
    public string title;
    public string description;
    public EPOIType type;
    public string imageName;
    public Texture2D image;
    public string imageUrl;
    public bool isUseful;
    public AddReferenceImageJobState jobState;
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
    /// The list of all the AR points of interest for the session
    /// </summary>
    [Tooltip("The list of all the AR points of interest for the session")]
    [SerializeField] private List<PointOfInterest> points = new List<PointOfInterest>();
    /// <summary>
    /// The list of all the "where" P.O.Is found for the session
    /// </summary>
    [Tooltip("The list of all the _where_ P.O.Is found for the session")]
    [SerializeField] private List<PointOfInterest> wherePoisFound = new List<PointOfInterest>();
    [SerializeField] private List<PointOfInterest> whenPoisFound = new List<PointOfInterest>();
    [SerializeField] private List<PointOfInterest> howPoisFound = new List<PointOfInterest>();
    #endregion

    #region Private variables
    private PointOfInterest wherePOIChosenAsSolution;
    private PointOfInterest whenPOIChosenAsSolution;
    private PointOfInterest howPOIChosenAsSolution;
    #endregion

    #region Public properties
    public List<PointOfInterest> Points  { get => points; }
    public List<PointOfInterest> WherePois { get => wherePoisFound; }
    public List<PointOfInterest> WhenPois { get => whenPoisFound; }
    public List<PointOfInterest> HowPois { get => howPoisFound; }
    public PointOfInterest WherePOIChosenAsSolution { get => wherePOIChosenAsSolution; set => wherePOIChosenAsSolution = value; }
    public PointOfInterest WhenPOIChosenAsSolution { get => whenPOIChosenAsSolution; set => whenPOIChosenAsSolution = value; }
    public PointOfInterest HowPOIChosenAsSolution { get => howPOIChosenAsSolution; set => howPOIChosenAsSolution = value; }
    #endregion 

    #region Public Methods
    #endregion
}

