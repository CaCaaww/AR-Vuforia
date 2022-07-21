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
    private async Task<string> GetRemoteData(string nicknameText, string passwordText)
    {
        //string url = String.Concat(remoteWebConsoleSO.JoinGate, "?code=", remoteWebConsoleSO.AccessCode);

        string url = String.Concat(remoteWebConsoleSO.JoinGate, "?code=", passwordText);

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
        Debug.Log("[WEB] Intro text: " + sessionDataSO.IntroText);

        sessionDataSO.VictoryText = dataStructure.victory_text;
        Debug.Log("[WEB] Victory text: " + sessionDataSO.VictoryText);

        sessionDataSO.DefeatText = dataStructure.defeat_text;
        Debug.Log("[WEB] Defeat text: " + sessionDataSO.DefeatText);

        int numberOfPois = dataStructure.ar_pois.Count;
        Debug.Log("[WEB] Number of POIs: " + numberOfPois);

        // Clear the POI list
        sessionDataSO.PointsOfInterest.Points.Clear();

        // Clear the helper dictionary
        sessionDataSO.PointsOfInterest.ImageNameAndPOI_Dict.Clear();

        for (int i = 0; i < numberOfPois; i++)
        {
            sessionDataSO.PointsOfInterest.Points.Add(new PointOfInterest());

            sessionDataSO.PointsOfInterest.Points[i].title = dataStructure.ar_pois[i].title;
            Debug.Log("[WEB] Title: " + dataStructure.ar_pois[i].title);
            
            sessionDataSO.PointsOfInterest.Points[i].type = (EPOIType)dataStructure.ar_pois[i].type;
            Debug.Log("[WEB] Clue type: " + dataStructure.ar_pois[i].type);
            
            sessionDataSO.PointsOfInterest.Points[i].description = dataStructure.ar_pois[i].description;
            Debug.Log("[WEB] Description: " + dataStructure.ar_pois[i].description);
            
            sessionDataSO.PointsOfInterest.Points[i].imageName = dataStructure.ar_pois[i].image_name;
            Debug.Log("[WEB] Image name: " + dataStructure.ar_pois[i].image_name);
            
            sessionDataSO.PointsOfInterest.Points[i].imageUrl = dataStructure.ar_pois[i].image_url;
            Debug.Log("[WEB] Image Url: " + dataStructure.ar_pois[i].image_url);
           
            sessionDataSO.PointsOfInterest.AddToImageNameAndPOI_Dict(sessionDataSO.PointsOfInterest.Points[i].imageName, sessionDataSO.PointsOfInterest.Points[i]);

            // Retrieve the actual image as a texture2D
            sessionDataSO.PointsOfInterest.Points[i].image = await Utils.GetRemoteTexture(sessionDataSO.PointsOfInterest.Points[i].imageUrl);      
        }

        uiEventsChannelSO.RaiseSessionDataLoadedEvent();
    }
    #endregion
}
