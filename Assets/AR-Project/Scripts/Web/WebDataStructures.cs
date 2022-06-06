using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARPOI
{
    // The name of the AR p.o.i.
    public string name;
    // The type of the AR p.o.i. (where, when, how)
    public string type;
    // The description of the AR p.o.i.
    public string description;
    // The image name for the AR p.o.i
    public string image_name;
    // The the url of the image for the AR p.o.i
    public string image_url;
    // Return true if this AR p.o.i is part of the solution, false if not
    public bool is_useful;
}

public class QRPOI
{
    // The name of the QR p.o.i.
    public string name;
    // The description of the QR p.o.i.
    public string description;
    // The QR code name for the QR p.o.i
    public string qrcode_name;
    // The url of the QR code for the p.o.i
    public string qrcode_url;
    // The image name for the QR p.o.i
    public string image_name;
    // The image url for the QR p.o.i.
    public string image_url;
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
    public List<ARPOI> arPOIs = new List<ARPOI>();
    // The list of all the QR p.o.i.
    public List<QRPOI> qrPOIs = new List<QRPOI>();
}
