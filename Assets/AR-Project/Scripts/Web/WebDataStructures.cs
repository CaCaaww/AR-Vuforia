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
    // The image name of the AR p.o.i
    public string image_name;
    // The the url of the image for the AR p.o.i
    public string image_url;
    // True if this AR p.o.i is part of the solution, false if not
    public bool is_useful;
    // The icon type of the p.o.i.
    public int icon_type;
    // The id of the avatar
    public int avatar_id;
    // The name of the avatar
    public string avatar_name;
}

public class DataStructure
{
    // The number of boosts at the start
    public int hints;
    // The title text for the story
    public string title;
    // The introduction text for the story
    public string intro;
    // The endgame victory text 
    public string victory_text;
    // The endgame defeat text 
    public string defeat_text;
    // The list of all the AR p.o.i.
    public List<ARPOI> ar_pois = new List<ARPOI>();
}
