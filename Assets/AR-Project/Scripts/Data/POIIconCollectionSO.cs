using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#region External classes

public enum EIconType
{
    Event,
    Dialog,
    Object
}

[Serializable]
public class POIIcon
{
    public EIconType iconType;
    public Sprite sprite;
}

#endregion

[CreateAssetMenu(fileName = "New POIIconCollection", menuName = "Data/POI Icon Collection SO")]
public class POIIconCollectionSO : ScriptableObject
{
    #region Inspector
    [SerializeField] private POIIcon[] poiIcons;
    #endregion

    #region Variables
    #endregion

    #region Properties
    public POIIcon[] POIIcons { get => poiIcons; }
    #endregion
}
