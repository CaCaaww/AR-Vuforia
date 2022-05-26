using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Data/New Trackable Objects SO")]
public class TrackableObjectSO : ScriptableObject
{
    [SerializeField]
    public List<TrackableObject> trackableObjects = new List<TrackableObject>();

}

[Serializable]
public class TrackableObject
{

    public string objectName;
    public string objectDescription;
    public RawImage objectImage;

}