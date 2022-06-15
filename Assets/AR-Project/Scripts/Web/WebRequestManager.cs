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

    async void Start()
    {
        var result = await GetRemoteData();

        dataStructure = JsonConvert.DeserializeObject<DataStructure>(result);

        sessionDataSO.Hints = dataStructure.hints;
        Debug.Log("[ARP] Number of hints: " + sessionDataSO.Hints);

        sessionDataSO.IntroText = dataStructure.intro_text;
        Debug.Log("[ARP] Intro text: " + sessionDataSO.IntroText);

        sessionDataSO.VictoryText = dataStructure.victory_text;
        Debug.Log("[ARP] Victory text: " + sessionDataSO.VictoryText);

        sessionDataSO.DefeatText = dataStructure.defeat_text;
        Debug.Log("[ARP] Defeat text: " + sessionDataSO.DefeatText);

        int numberOfPois = dataStructure.ar_pois.Count;
        Debug.Log("[ARP] Number of images: " + numberOfPois);

        // Clear the POI list
        sessionDataSO.PointsOfInterest.Points.Clear();
        
        for (int i = 0; i < numberOfPois; i++)
        {
            sessionDataSO.PointsOfInterest.Points.Add(new PointOfInterest());

            sessionDataSO.PointsOfInterest.Points[i].title = dataStructure.ar_pois[i].title;
            Debug.Log("[ARP] Title: " + dataStructure.ar_pois[i].title);
            
            sessionDataSO.PointsOfInterest.Points[i].clueType = (EPOIType)dataStructure.ar_pois[i].type;
            Debug.Log("[ARP] Clue type: " + dataStructure.ar_pois[i].type);
            
            sessionDataSO.PointsOfInterest.Points[i].description = dataStructure.ar_pois[i].description;
            Debug.Log("[ARP] Description: " + dataStructure.ar_pois[i].description);
            
            sessionDataSO.PointsOfInterest.Points[i].imageName = dataStructure.ar_pois[i].image_name;
            Debug.Log("[ARP] Image name: " + dataStructure.ar_pois[i].image_name);
            
            sessionDataSO.PointsOfInterest.Points[i].imageUrl = dataStructure.ar_pois[i].image_url;
            Debug.Log("[ARP] Image Url: " + dataStructure.ar_pois[i].image_url);

            //sessionDataSO.PointsOfInterest.AddImageNameAndTitle(sessionDataSO.PointsOfInterest.Points[i].imageName, sessionDataSO.PointsOfInterest.Points[i].title);

            // TO DO: retrieve the actual image as a texture2D
            sessionDataSO.PointsOfInterest.Points[i].image = await Utils.GetRemoteTexture(sessionDataSO.PointsOfInterest.Points[i].imageUrl);      

            //sessionDataSO.PointsOfInterest.AddImageNameAndTexture(sessionDataSO.PointsOfInterest.Points[i].imageName, sessionDataSO.PointsOfInterest.Points[i].image);
        }

        uiEventsChannelSO.RaiseSessionDataLoadedEvent();
    }
    #endregion
    
    #region Helper methods
    private async Task<string> GetRemoteData()
    {
        string url = String.Concat(remoteWebConsoleSO.JoinGate, "?code=", remoteWebConsoleSO.AccessCode);

        Debug.Log(url);

        var www = UnityWebRequest.Get(url);

        // Set the request timeout
        www.timeout = 10;

        var operation = www.SendWebRequest();

        while (!operation.isDone)
            await Task.Yield();

        if (www.result == UnityWebRequest.Result.Success)
        {
            Debug.Log($"Success: {www.downloadHandler.text}");
            return www.downloadHandler.text;
        }
        else
        {
            Debug.Log($"Failed: {www.error}");
            return www.error;
        }         
    }
    #endregion
}
