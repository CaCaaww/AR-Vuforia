using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class POI
{
    public string name;
    public string type;
    public string description;
    public string image_name;
    public string image_url;
    //public string is_useful;
}

public class DataStructure
{
    //The number of boosts at the start
    public string hints;
    public List<POI> pois = new List<POI>();
}
