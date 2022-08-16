using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.XR.ARSubsystems;

#region External classes
public enum EPOIType
{
    Where,
    When,
    How
}

[Serializable]
public class PointOfInterest
{
    public int id;
    public string title;
    public EPOIType type;
    public string description;
    public bool isAR;
    //public string[] imageNames;
    //public string[] imageUrls;
    #if UNITY_EDITOR
    public List<Texture2D> images = new List<Texture2D>();
    #endif
    public Dictionary<string, Texture2D> imageNameAndTexture = new Dictionary<string, Texture2D>();
    public Dictionary<string, string> imageNameAndUrl = new Dictionary<string, string>();
    public bool isUseful;
    public AddReferenceImageJobState jobState;
    public EIconType iconType;
    public bool alreadyDetected;
    public int avatarID;
    public string avatarName;
    public int timer;
    public int linkedTo;
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
    /// The list of all the "where" POIs found for the session
    /// </summary>
    [Tooltip("The list of all the _where_ POIs found for the session")]
    [SerializeField] private List<PointOfInterest> wherePoisFound = new List<PointOfInterest>();
    /// <summary>
    /// The list of all the "when" POIs found for the session
    /// </summary>
    [Tooltip("The list of all the _when_ POIs found for the session")]
    [SerializeField] private List<PointOfInterest> whenPoisFound = new List<PointOfInterest>();
    /// <summary>
    /// The list of all the "how" POIs found for the session
    /// </summary>
    [Tooltip("The list of all the _how_ POIs found for the session")]
    [SerializeField] private List<PointOfInterest> howPoisFound = new List<PointOfInterest>();
    #endregion

    #region Private variables
    /// <summary>
    /// Reference to the where POIs chosen as part of the solution
    /// </summary>
    private PointOfInterest wherePOIChosenAsSolution;
    /// <summary>
    /// Reference to the when POIs chosen as part of the solution
    /// </summary>
    private PointOfInterest whenPOIChosenAsSolution;
    /// <summary>
    /// Reference to the how POIs chosen as part of the solution
    /// </summary>
    private PointOfInterest howPOIChosenAsSolution;

    /// <summary>
    /// Dictionary to store the relation between image name and POI
    /// </summary>
    private Dictionary<string, PointOfInterest> imageNameAndPOI = new Dictionary<string, PointOfInterest>();
    
    //private Dictionary<int, PointOfInterest> idAndPOI_Dict = new Dictionary<int, PointOfInterest>();
    //private Dictionary<int, PointOfInterest> idAndARPOI_Dict = new Dictionary<int, PointOfInterest>();
    //private Dictionary<int, PointOfInterest> idAndNOARPOI_Dict = new Dictionary<int, PointOfInterest>();
    
    #endregion

    #region Public properties
    public List<PointOfInterest> Points  { get => points; }
    public List<PointOfInterest> WherePois { get => wherePoisFound; }
    public List<PointOfInterest> WhenPois { get => whenPoisFound; }
    public List<PointOfInterest> HowPois { get => howPoisFound; }

    public PointOfInterest WherePOIChosenAsSolution { get => wherePOIChosenAsSolution; set => wherePOIChosenAsSolution = value; }
    public PointOfInterest WhenPOIChosenAsSolution { get => whenPOIChosenAsSolution; set => whenPOIChosenAsSolution = value; }
    public PointOfInterest HowPOIChosenAsSolution { get => howPOIChosenAsSolution; set => howPOIChosenAsSolution = value; }
    
    public Dictionary<string, PointOfInterest> ImageNameAndPOI { get => imageNameAndPOI; }
    
    //public Dictionary<int, PointOfInterest> IDAndPOI_Dict { get => idAndPOI_Dict; }
    //public Dictionary<int, PointOfInterest> IDAndARPOI_Dict { get => idAndARPOI_Dict; }
    //public Dictionary<int, PointOfInterest> IDAndNOARPOI_Dict { get => idAndNOARPOI_Dict; }
    #endregion

    #region Public Methods
    public void AddToImageNameAndPOI(string imageName, PointOfInterest poi)
    {
        imageNameAndPOI.Add(imageName, poi);
    }

    /*
    public void AddToIDAndPOI_Dict(int id, PointOfInterest poi)
    {
        idAndPOI_Dict.Add(id, poi);
    }

    public void AddToIDAndARPOI_Dict(int id, PointOfInterest poi)
    {
        idAndARPOI_Dict.Add(id, poi);
    }

    public void AddToIDAndNOARPOI_Dict(int id, PointOfInterest poi)
    {
        idAndNOARPOI_Dict.Add(id, poi);
    }
    */
    #endregion
}

