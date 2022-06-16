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
    [SerializeField]
    private List<PointOfInterest> points = new List<PointOfInterest>();
    /// <summary>
    /// The list of all the "where" P.O.Is found for the session
    /// </summary>
    [Tooltip("The list of all the _where_ P.O.Is found for the session")]
    [SerializeField]
    private List<PointOfInterest> wherePoisFound = new List<PointOfInterest>();
    [SerializeField]
    private List<PointOfInterest> whenPoisFound = new List<PointOfInterest>();
    [SerializeField]
    private List<PointOfInterest> howPoisFound = new List<PointOfInterest>();
    #endregion

    #region Private variables
    //Dictionary<string, string> imageNameAndTitle = new Dictionary<string, string>();
    //Dictionary<string, Texture2D> imageNameAndTexture = new Dictionary<string, Texture2D>();
    #endregion

    #region Public properties
    public List<PointOfInterest> Points  { get => points; }
    public List<PointOfInterest> WherePois { get => wherePoisFound; }
    public List<PointOfInterest> WhenPois { get => whenPoisFound; }
    public List<PointOfInterest> HowPois { get => howPoisFound; }
    //public Dictionary<string, string> ImageNameAndTitle { get => imageNameAndTitle; } 
    //public Dictionary<string, Texture2D> ImageNameAndTexture { get => imageNameAndTexture; }
    #endregion 

    #region Public Methods
    /*public void AddImageNameAndTitle(string imageName, string title)
    {
        if (!imageNameAndTitle.ContainsKey(imageName))
        {
            imageNameAndTitle.Add(imageName, title);
        }
    }*/

    /*public void AddImageNameAndTexture(string imageName, Texture2D texture)
    {
        if (!imageNameAndTexture.ContainsKey(imageName))
        {
            imageNameAndTexture.Add(imageName, texture);
        }
    }*/
    #endregion
}

