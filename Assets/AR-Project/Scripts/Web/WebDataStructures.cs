using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class POI
{
    // The id of the POI.
    public int id;
    // The name of the POI.
    public string title;
    // The type of the POI (0 = where, 1 = when, 2 = how)
    public int type;
    // The description of the POI
    public string description;
    //True if the this is an AR POI
    public bool is_ar;
    // The dictionary to correlate the image name with the image url
    public Dictionary<string, string> images = new Dictionary<string, string>();
    // True if this POI is part of the solution, false if not  
    public bool is_useful;
    // The icon type of the POI (0 = event, 1 = dialogue, 2 = object)
    public int icon_type;
    // The id of the avatar
    public int avatar_id;
    // The name of the avatar
    public string avatar_name;
    // The state of a POI (0 = undetected, 1 = detected, 2 = detected and deleted)
    public int detected; 
}

public class LoginGetDataStructure
{
    // The player id
    public int player_id;
    // The session id
    public int session_id;
    // The title text for the story
    public string title;
    // The introduction text for the story
    public string intro;
    // The endgame victory text 
    public string victory_text;
    // The endgame defeat text 
    public string defeat_text;
    // The number of hints at the start
    public int hints;
    // If we are resuming an unfinished session or not
    public bool resume_session;
    // The list of all the POIs
    public List<POI> pois = new List<POI>();
}

public class POIFoundDataStructure
{
    // The POI 
    public int poi_id;
    // The time in seconds from the start of the session (nope, just the timestamp)
    public int time_elapsed;

    public int player_id;
    public int session_id;
}

public class HintUsedDataStructure
{
    // The where POI id deleted by the hint (0 if none was deleted) 
    public int where_poi_id;
    // The POI 
    public int when_poi_id;
    // The POI 
    public int how_poi_id;
    // The time in seconds from the start of the session (nope, just the timestamp)
    public int time_elapsed;

    public int player_id;
    public int session_id;
}

public class SolutionGivenDataStructure
{
    // Where POI id 
    public int where_poi_id;
    // When POI id 
    public int when_poi_id;
    // How POI id 
    public int how_poi_id;
    // The time in seconds from the start of the session
    public int time_elapsed;

    public int player_id;
    public int session_id;
}