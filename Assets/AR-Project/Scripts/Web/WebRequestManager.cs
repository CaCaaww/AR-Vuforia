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
    #endregion
    
    #region Helper methods
    private async Task<UnityWebRequest> GetRemoteData(string nickname, string password)
    {
        #if UNITY_EDITOR
        //string url = String.Concat(
        //    remoteWebConsoleSO.JoinGate,
        //    remoteWebConsoleSO.NicknameParameter,
        //    remoteWebConsoleSO.NicknameValue,
        //    remoteWebConsoleSO.PasswordParameter,
        //    remoteWebConsoleSO.PasswordValue);
        string url = String.Concat(
            remoteWebConsoleSO.JoinGate,
            remoteWebConsoleSO.PasswordParameter,
            remoteWebConsoleSO.PasswordValue);
        #elif UNITY_ANDROID
        string url = String.Concat(
            remoteWebConsoleSO.JoinGate,
            remoteWebConsoleSO.NicknameParameter,
            nicknameText,
            remoteWebConsoleSO.PasswordParameter,
            passwordText);
        #endif

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

            #if !UNITY_EDITOR && UNITY_ANDROID
            remoteWebConsoleSO.NicknameValue = nickname;
            remoteWebConsoleSO.PasswordValue = password;
            #endif
        }
        else
        {
            Debug.Log($"[WEB] Failed: {www.error}");
        }

        return www;
    }
#endregion

#region Callbacks
    private async void Login(string nicknameText, string passwordText)
    {
        var www = await GetRemoteData(nicknameText, passwordText);

        if (www.result != UnityWebRequest.Result.Success)
        {
            uiEventsChannelSO.RaiseSessionDataLoadedEvent(false);
            return;
        }

        dataStructure = JsonConvert.DeserializeObject<DataStructure>(www.downloadHandler.text);

        Debug.Log("DATASTRUCTURE");
        foreach(KeyValuePair<string, string> entry in dataStructure.pois[0].images)
        {
            Debug.Log("Key: " + entry.Key);
            Debug.Log("Value: " + entry.Value);
        }

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

        int numberOfPois = dataStructure.pois.Count;
        Debug.Log("[WEB] Number of POIs: " + numberOfPois);

        // Clear the POI list
        sessionDataSO.PointsOfInterest.Points.Clear();

        // Clear the helper dictionaries
        sessionDataSO.PointsOfInterest.ImageNameAndPOI.Clear();
        //sessionDataSO.PointsOfInterest.IDAndPOI_Dict.Clear();
        //sessionDataSO.PointsOfInterest.IDAndARPOI_Dict.Clear();
        //sessionDataSO.PointsOfInterest.IDAndNOARPOI_Dict.Clear();
        
    
        for (int i = 0; i < numberOfPois; i++)
        {
            // Add a new p.o.i. object
            sessionDataSO.PointsOfInterest.Points.Add(new PointOfInterest());

            // Set the p.o.i. id
            sessionDataSO.PointsOfInterest.Points[i].id = dataStructure.pois[i].id;
            Debug.Log("[WEB] ID: " + dataStructure.pois[i].id);

            // Set the p.o.i. title
            sessionDataSO.PointsOfInterest.Points[i].title = dataStructure.pois[i].title;
            Debug.Log("[WEB] Title: " + dataStructure.pois[i].title);

            // Set the p.o.i. type
            sessionDataSO.PointsOfInterest.Points[i].type = (EPOIType)dataStructure.pois[i].type;
            Debug.Log("[WEB] Clue type: " + (EPOIType)dataStructure.pois[i].type);

            // Set the p.o.i. description
            sessionDataSO.PointsOfInterest.Points[i].description = dataStructure.pois[i].description;
            Debug.Log("[WEB] Description: " + dataStructure.pois[i].description);

            // For every remote image name/url pair
            foreach (KeyValuePair<string, string> image in dataStructure.pois[i].images)
            {
                // Store the pair (key = image name, value = image url) in a dictionary
                sessionDataSO.PointsOfInterest.Points[i].imageNameAndUrl.Add(image.Key, image.Value);
                Debug.Log("[WEB] Image name: " + image.Key);
                Debug.Log("[WEB] Image Url: " + image.Value);

                // and then add them to an helper dictionary for the reference image library rebuilding process
                sessionDataSO.PointsOfInterest.AddToImageNameAndPOI(image.Key, sessionDataSO.PointsOfInterest.Points[i]);
                Debug.Log("[WEB VUFORIA] Image name: " + image.Key);
                Debug.Log("[WEB VUFORIA] ImageNameAndPOI: " + sessionDataSO.PointsOfInterest.ImageNameAndPOI[image.Key].title);

                // Retrieve the image as a texture2D
                sessionDataSO.PointsOfInterest.Points[i].imageNameAndTexture.Add(image.Key, await Utils.GetRemoteTexture(image.Value));
                #if UNITY_EDITOR
                sessionDataSO.PointsOfInterest.Points[i].images.Add(sessionDataSO.PointsOfInterest.Points[i].imageNameAndTexture[image.Key]);
                #endif
            }

            //sessionDataSO.PointsOfInterest.Points[i].isAR = dataStructure.ar_pois[i].is_ar;
            //Debug.Log("[WEB] Is AR: " + dataStructure.ar_pois[i].is_ar);

            sessionDataSO.PointsOfInterest.Points[i].isUseful = dataStructure.pois[i].is_useful;
            Debug.Log("[WEB] Is useful to to the solution: " + dataStructure.pois[i].is_useful);

            sessionDataSO.PointsOfInterest.Points[i].iconType = (EIconType)dataStructure.pois[i].icon_type;
            Debug.Log("[WEB] Icon type: " + (EIconType)dataStructure.pois[i].icon_type);

            sessionDataSO.PointsOfInterest.Points[i].avatarID = dataStructure.pois[i].avatar_id;
            Debug.Log("[WEB] Avatar id: " + dataStructure.pois[i].avatar_id);

            sessionDataSO.PointsOfInterest.Points[i].avatarName = dataStructure.pois[i].avatar_name;
            Debug.Log("[WEB] Avatar name: " + dataStructure.pois[i].avatar_name);
            

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

        uiEventsChannelSO.RaiseSessionDataLoadedEvent(true);
    }
#endregion
}
