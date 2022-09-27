using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
    #if UNITY_EDITOR
    public List<Texture2D> images = new();
    #endif
    public Dictionary<string, Texture2D> imageNameAndTexture = new();
    public Dictionary<string, string> imageNameAndUrl = new();
    public bool isUseful;
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
    [SerializeField] private List<PointOfInterest> points = new();
    /// <summary>
    /// The list of all the "where" POIs found for the session
    /// </summary>
    [Tooltip("The list of all the _where_ POIs found for the session")]
    [SerializeField] private List<PointOfInterest> wherePoisFound = new();
    /// <summary>
    /// The list of all the "when" POIs found for the session
    /// </summary>
    [Tooltip("The list of all the _when_ POIs found for the session")]
    [SerializeField] private List<PointOfInterest> whenPoisFound = new();
    /// <summary>
    /// The list of all the "how" POIs found for the session
    /// </summary>
    [Tooltip("The list of all the _how_ POIs found for the session")]
    [SerializeField] private List<PointOfInterest> howPoisFound = new();
    
    [Header("Solution")]
    /// <summary>
    /// The id of the _where_ POI that is part of the solution
    /// </summary>
    [Tooltip("The id of the _where_ POI that is part of the solution")]
    [SerializeField]
    private int wherePOISolutionId = 0;
    /// <summary>
    /// The id of the _when_ POI that is part of the solution
    /// </summary>
    [Tooltip("The id of the _when_ POI that is part of the solution")]
    [SerializeField]
    private int whenPOISolutionId = 0;
    /// <summary>
    /// The id of the _how_ POI that is part of the solution
    /// </summary>
    [Tooltip("The id of the _how_ POI that is part of the solution")]
    [SerializeField]
    private int howPOISolutionId = 0;
    /// <summary>
    /// The id of the "where" POI chosen as part of the solution
    /// </summary>
    [Tooltip("The id of the _where_ POI chosen as part of the solution")]
    [SerializeField]
    private int wherePOIChosenAsSolutionId = 0;
    /// <summary>
    /// The id of the "when" POI chosen as part of the solution
    /// </summary>
    [Tooltip("The id of the _when_ POI chosen as part of the solution")]
    [SerializeField]
    private int whenPOIChosenAsSolutionId = 0;
    /// <summary>
    /// The id of the "how" POI chosen as part of the solution
    /// </summary>
    [Tooltip("The id of the _how_ POI chosen as part of the solution")]
    [SerializeField]
    private int howPOIChosenAsSolutionId = 0;
    #endregion

    #region Private variables
    /// <summary>
    /// Dictionary to store the relation between image name and POI
    /// </summary>
    private readonly Dictionary<string, PointOfInterest> imageNameAndPOI = new();

    /// <summary>
    /// Dictionary to store the relation between image name and Vuforia ImageTarget object
    /// </summary>
    private readonly Dictionary<string, GameObject> imageNameAndImageTargetObject = new();

    //private Dictionary<int, PointOfInterest> idAndARPOI_Dict = new Dictionary<int, PointOfInterest>();
    //private Dictionary<int, PointOfInterest> idAndNOARPOI_Dict = new Dictionary<int, PointOfInterest>();
    
    #endregion

    #region Public properties
    public List<PointOfInterest> Points  { get => points; }
    public List<PointOfInterest> WherePois { get => wherePoisFound; }
    public List<PointOfInterest> WhenPois { get => whenPoisFound; }
    public List<PointOfInterest> HowPois { get => howPoisFound; }


    public int WherePOISolutionId { get => wherePOISolutionId; set => wherePOISolutionId = value; }
    public int WhenPOISolutionId { get => whenPOISolutionId; set => whenPOISolutionId = value; }
    public int HowPOISolutionId { get => howPOISolutionId; set => howPOISolutionId = value; }

    public int WherePOIChosenAsSolutionId { get => wherePOIChosenAsSolutionId; set => wherePOIChosenAsSolutionId = value; }
    public int WhenPOIChosenAsSolutionId { get => whenPOIChosenAsSolutionId; set => whenPOIChosenAsSolutionId = value; }
    public int HowPOIChosenAsSolutionId { get => howPOIChosenAsSolutionId; set => howPOIChosenAsSolutionId = value; }


    public Dictionary<string, PointOfInterest> ImageNameAndPOI { get => imageNameAndPOI; }
    public Dictionary<string, GameObject> ImageNameAndImageTargetObject { get => imageNameAndImageTargetObject; }
    
    //public Dictionary<int, PointOfInterest> IDAndPOI_Dict { get => idAndPOI_Dict; }
    //public Dictionary<int, PointOfInterest> IDAndARPOI_Dict { get => idAndARPOI_Dict; }
    //public Dictionary<int, PointOfInterest> IDAndNOARPOI_Dict { get => idAndNOARPOI_Dict; }
    #endregion

    #region Public Methods
    public void AddToImageNameAndPOI(string imageName, PointOfInterest poi)
    {
        imageNameAndPOI.Add(imageName, poi);
    }

    /// <summary>
    /// Method to reset some variables before the start of the game (in editor mode)
    /// </summary>
    public void ResetVariables()
    {
        wherePoisFound.Clear();
        whenPoisFound.Clear();
        howPoisFound.Clear();

        wherePOIChosenAsSolutionId = 0;
        whenPOIChosenAsSolutionId = 0;
        howPOIChosenAsSolutionId = 0;
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

