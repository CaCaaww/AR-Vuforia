using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class WebRequestManager : MonoBehaviour
{
    #region Inspector
    [Header("SO Send Channels")]
    [SerializeField]
    private UIEventsChannelSO uiEventsChannelSO;

    [Header("References")]
    [SerializeField]
    private RemoteWebConsoleSO remoteWebConsoleSO;
    [SerializeField]
    private SessionDataSO sessionDataSO;
    #endregion

    #region Variables
    #endregion

    #region Properties
    private DataStructure dataStructure;
    #endregion

    #region Unity methods
    void OnEnable()
    {
        uiEventsChannelSO.OnLoginCredentialsSentEventRaised += Login;
    }

    void OnDisable()
    {
        uiEventsChannelSO.OnLoginCredentialsSentEventRaised -= Login;
    }

    void Start()
    {
        Login("aaa", "bbb");
    }
    #endregion
    
    #region Helper methods
    private async Task<string> GetRemoteData(string nicknameText, string passwordText)
    {
        string url = String.Concat(remoteWebConsoleSO.JoinGate, "?code=", remoteWebConsoleSO.AccessCode);

        //string url = String.Concat(remoteWebConsoleSO.JoinGate, "?code=", passwordText);

        Debug.Log(url);

        var www = UnityWebRequest.Get(url);

        // Set the request timeout
        www.timeout = 10;

        var operation = www.SendWebRequest();

        while (!operation.isDone)
            await Task.Yield();

        if (www.result == UnityWebRequest.Result.Success)
        {
            Debug.Log($"[WEB] Success: {www.downloadHandler.text}");
            return www.downloadHandler.text;
        }
        else
        {
            Debug.Log($"[WEB] Failed: {www.error}");
            return www.error;
        }         
    }
    #endregion

    #region Callbacks
    private async void Login(string nicknameText, string passwordText)
    {
        var result = await GetRemoteData(nicknameText, passwordText);

        dataStructure = JsonConvert.DeserializeObject<DataStructure>(result);

        sessionDataSO.Hints = dataStructure.hints;
        Debug.Log("[WEB] Number of hints: " + sessionDataSO.Hints);

        sessionDataSO.TitleText = dataStructure.title;
        Debug.Log("[WEB] Intro text: " + sessionDataSO.IntroText);

        sessionDataSO.IntroText = dataStructure.intro;
        Debug.Log("[WEB] Intro text: " + sessionDataSO.TitleText);

        sessionDataSO.VictoryText = dataStructure.victory_text;
        Debug.Log("[WEB] Victory text: " + sessionDataSO.VictoryText);

        sessionDataSO.DefeatText = dataStructure.defeat_text;
        Debug.Log("[WEB] Defeat text: " + sessionDataSO.DefeatText);

        //sessionDataSO.Autoreveal = dataStructure.autoreveal_pois;
        //Debug.Log("[WEB] Autoreveal POIs: " + sessionDataSO.Autoreveal);

        //sessionDataSO.AutorevealPercentage = dataStructure.autoreveal_percentage;
        //Debug.Log("[WEB] Autoreveal percentage: " + sessionDataSO.AutorevealPercentage);

        //sessionDataSO.AutorevealNumber = dataStructure.autoreveal_number_of_pois;
        //Debug.Log("[WEB] Autoreveal number of pois: " + sessionDataSO.AutorevealNumber);

        //sessionDataSO.AutorevealTimer = dataStructure.autoreveal_timer;
        //Debug.Log("[WEB] Autoreveal timer: " + sessionDataSO.AutorevealTimer);

        int numberOfPois = dataStructure.ar_pois.Count;
        Debug.Log("[WEB] Number of POIs: " + numberOfPois);

        // Clear the POI list
        sessionDataSO.PointsOfInterest.Points.Clear();

        // Clear the helper dictionaries
        sessionDataSO.PointsOfInterest.ImageNameAndPOI_Dict.Clear();
        sessionDataSO.PointsOfInterest.IDAndPOI_Dict.Clear();
        sessionDataSO.PointsOfInterest.IDAndARPOI_Dict.Clear();
        sessionDataSO.PointsOfInterest.IDAndNOARPOI_Dict.Clear();

        for (int i = 0; i < numberOfPois; i++)
        {
            // Add a new p.o.i. object
            sessionDataSO.PointsOfInterest.Points.Add(new PointOfInterest());

            // Set the p.o.i. id
            sessionDataSO.PointsOfInterest.Points[i].id = dataStructure.ar_pois[i].id;
            Debug.Log("[WEB] ID: " + dataStructure.ar_pois[i].id);

            // Set the p.o.i. title
            sessionDataSO.PointsOfInterest.Points[i].title = dataStructure.ar_pois[i].title;
            Debug.Log("[WEB] Title: " + dataStructure.ar_pois[i].title);

            // Set the p.o.i. type
            sessionDataSO.PointsOfInterest.Points[i].type = (EPOIType)dataStructure.ar_pois[i].type;
            Debug.Log("[WEB] Clue type: " + (EPOIType)dataStructure.ar_pois[i].type);

            // Set the p.o.i. description
            sessionDataSO.PointsOfInterest.Points[i].description = dataStructure.ar_pois[i].description;
            Debug.Log("[WEB] Description: " + dataStructure.ar_pois[i].description);

            // Create an index to loop the images
            int k = 0;

            // For every remote image name/url pair
            foreach (KeyValuePair<string, string> image in dataStructure.ar_pois[i].images)
            {
                // Store the image name inside an array
                sessionDataSO.PointsOfInterest.Points[i].imageNames[k] = image.Key;
                Debug.Log("[WEB] Image name: " + image.Key);

                // Store the image url inside an array
                sessionDataSO.PointsOfInterest.Points[i].imageUrls[k] = image.Value;
                Debug.Log("[WEB] Image Url: " + image.Value);

                // Store the pair (key = image name, value = image url) in a dictionary
                sessionDataSO.PointsOfInterest.Points[i].imageNameAndUrl.Add(image.Key, image.Value);

                // and then add them to an helper dictionary for the reference image library rebuilding process
                sessionDataSO.PointsOfInterest.AddToImageNameAndPOI_Dict(image.Key, sessionDataSO.PointsOfInterest.Points[i]);

                // Retrieve the image as a texture2D
                sessionDataSO.PointsOfInterest.Points[i].images[k] = await Utils.GetRemoteTexture(image.Value);

                k += 1;
            }

            Debug.Log("K: " + k);

            //sessionDataSO.PointsOfInterest.Points[i].isAR = dataStructure.ar_pois[i].is_ar;
            //Debug.Log("[WEB] Is AR: " + dataStructure.ar_pois[i].is_ar);


            sessionDataSO.PointsOfInterest.Points[i].isUseful = dataStructure.ar_pois[i].is_useful;
            Debug.Log("[WEB] Is useful to to the solution: " + dataStructure.ar_pois[i].is_useful);

            sessionDataSO.PointsOfInterest.Points[i].iconType = (EIconType)dataStructure.ar_pois[i].icon_type;
            Debug.Log("[WEB] Icon type: " + (EIconType)dataStructure.ar_pois[i].icon_type);

            sessionDataSO.PointsOfInterest.Points[i].avatarID = dataStructure.ar_pois[i].avatar_id;
            Debug.Log("[WEB] Avatar id: " + dataStructure.ar_pois[i].avatar_id);

            sessionDataSO.PointsOfInterest.Points[i].avatarName = dataStructure.ar_pois[i].avatar_name;
            Debug.Log("[WEB] Avatar name: " + dataStructure.ar_pois[i].avatar_name);

            //sessionDataSO.PointsOfInterest.Points[i].timer = dataStructure.ar_pois[i].timer;
            //Debug.Log("[WEB] Timer to wait before revealing: " + dataStructure.ar_pois[i].timer);

            //sessionDataSO.PointsOfInterest.Points[i].linkedTo = dataStructure.ar_pois[i].linked_poi;
            //Debug.Log("[WEB] The ID of the linked POI: " + dataStructure.ar_pois[i].linked_poi);
       
            //sessionDataSO.PointsOfInterest.AddToIDAndPOI_Dict(sessionDataSO.PointsOfInterest.Points[i].id, sessionDataSO.PointsOfInterest.Points[i]);
            
            /*if (sessionDataSO.PointsOfInterest.Points[i].isAR)
            {
                // Waiting for Simo to add the id to the json
                //sessionDataSO.PointsOfInterest.AddToIDAndARPOI_Dict(sessionDataSO.PointsOfInterest.Points[i].id, sessionDataSO.PointsOfInterest.Points[i]);
            }
            else
            {
                // Waiting for Simo to add the id to the json
                //sessionDataSO.PointsOfInterest.AddToIDAndNOARPOI_Dict(sessionDataSO.PointsOfInterest.Points[i].id, sessionDataSO.PointsOfInterest.Points[i]);
            }*/
            
            // Retrieve the actual image as a texture2D
            //sessionDataSO.PointsOfInterest.Points[i].image = await Utils.GetRemoteTexture(sessionDataSO.PointsOfInterest.Points[i].imageUrl);      
        }

        uiEventsChannelSO.RaiseSessionDataLoadedEvent();
    }
    #endregion
}
