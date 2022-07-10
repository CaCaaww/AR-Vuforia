using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARPOI
{
    // The name of the AR p.o.i.
    public string title;
    // The type of the AR p.o.i. (0 = where, 1 = when, 2 = how)
    public int type;
    // The description of the AR p.o.i.
    public string description;
    // The image name for the AR p.o.i
    public string image_name;
    // The the url of the image for the AR p.o.i
    public string image_url;
    // Return true if this AR p.o.i is part of the solution, false if not
    public bool is_useful;
}

public class DataStructure
{
    // The number of boosts at the start
    public int hints;
    // The introduction text for the story
    public string intro_text;
    // The endgame victory text 
    public string victory_text;
    // The endgame defeat text 
    public string defeat_text;
    // The list of all the AR p.o.i.
    public List<ARPOI> ar_pois = new List<ARPOI>();
}
