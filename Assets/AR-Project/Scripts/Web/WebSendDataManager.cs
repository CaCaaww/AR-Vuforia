using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class WebSendDataManager : MonoBehaviour
{
    #region Inspector
    [Header("SO Listen Channels")]
    [SerializeField]
    private UIEventsChannelSO uiEventsChannelSO;
    
    [Header("References")]
    [SerializeField]
    private RemoteWebConsoleSO remoteWebConsoleSO;
    [SerializeField]
    private SessionDataSO sessionDataSO;
    #endregion

    #region Constants
    private const string dateTimeFormat = "yyyy-MM-dd HH:mm:ss";
    #endregion

    #region Unity methods
    void OnEnable()
    {
        uiEventsChannelSO.OnStartGameEventRaised += HandleOnStartGameEvent;
        uiEventsChannelSO.OnPOIFoundEventRaised += HandleOnPOIFoundEvent;
        uiEventsChannelSO.OnPOIDeletedByHintEventRaised += HandlePOIDeletedByHintEvent;
        uiEventsChannelSO.OnSolutionGivenEventRaised += HandleOnSolutionGivenEvent;
    }


    void OnDisable()
    {
        uiEventsChannelSO.OnStartGameEventRaised -= HandleOnStartGameEvent;
        uiEventsChannelSO.OnPOIFoundEventRaised -= HandleOnPOIFoundEvent;
        uiEventsChannelSO.OnPOIDeletedByHintEventRaised -= HandlePOIDeletedByHintEvent;
        uiEventsChannelSO.OnSolutionGivenEventRaised -= HandleOnSolutionGivenEvent;
    }
    #endregion

    #region Helper Methods
    private async Task<UnityWebRequest> SendRemoteData(string data, string url)
    {
        Debug.Log(url);

        string postData = data;

        var www = UnityWebRequest.Post(url, postData);

        // Set the request timeout
        www.timeout = 10;

        var operation = www.SendWebRequest();

        while (!operation.isDone)
            await Task.Yield();

        return www;
    }
    #endregion

    #region Callbacks
    private async void HandleOnStartGameEvent()
    {
        StartGameDataStructure startGameDataStructure = new();

        startGameDataStructure.player_id = sessionDataSO.PlayerId;
        startGameDataStructure.session_id = sessionDataSO.SessionId;
        startGameDataStructure.timestamp = DateTime.Now.ToString(dateTimeFormat);

        #if UNITY_EDITOR
        string url = String.Concat(
            remoteWebConsoleSO.StarGameGate,
            remoteWebConsoleSO.NicknameParameter,
            remoteWebConsoleSO.NicknameValue,
            remoteWebConsoleSO.PasswordParameter,
            remoteWebConsoleSO.PasswordValue);
        #elif UNITY_ANDROID
        string url = String.Concat(
            remoteWebConsoleSO.StarGameGate,
            remoteWebConsoleSO.NicknameParameter,
            nickname,
            remoteWebConsoleSO.PasswordParameter,
            password);
        #endif

        string dataToSend = JsonConvert.SerializeObject(startGameDataStructure);

        var www = await SendRemoteData(dataToSend, url);

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("The timestamp for the start of the game was sent successfully.");
        }
        else
        {
            Debug.Log("Error sending the timestamp for the start of the game.");
        }
    }

    private async void HandleOnPOIFoundEvent(PointOfInterest poi)
    {
        POIFoundDataStructure poiFoundGameDataStructure = new();

        poiFoundGameDataStructure.player_id = sessionDataSO.PlayerId;
        poiFoundGameDataStructure.session_id = sessionDataSO.SessionId;
        poiFoundGameDataStructure.timestamp = DateTime.Now.ToString(dateTimeFormat);
        poiFoundGameDataStructure.poi_id = poi.id;

        #if UNITY_EDITOR
        string url = String.Concat(
            remoteWebConsoleSO.POIFoundGate,
            remoteWebConsoleSO.NicknameParameter,
            remoteWebConsoleSO.NicknameValue,
            remoteWebConsoleSO.PasswordParameter,
            remoteWebConsoleSO.PasswordValue);
        #elif UNITY_ANDROID
        string url = String.Concat(
            remoteWebConsoleSO.POIFoundGate,
            remoteWebConsoleSO.NicknameParameter,
            nickname,
            remoteWebConsoleSO.PasswordParameter,
            password);
        #endif

        string dataToSend = JsonConvert.SerializeObject(poiFoundGameDataStructure);

        var www = await SendRemoteData(dataToSend, url);

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("The timestamp for the POI found event was sent successfully.");
        }
        else
        {
            Debug.Log("Error sending the timestamp for the POI found event.");
        }
    }

    private async void HandlePOIDeletedByHintEvent(int wherePoiId, int whenPoiId, int howPoiId)
    {
        HintUsedDataStructure hintUsedGameDataStructure = new();

        hintUsedGameDataStructure.player_id = sessionDataSO.PlayerId;
        hintUsedGameDataStructure.session_id = sessionDataSO.SessionId;
        hintUsedGameDataStructure.timestamp = DateTime.Now.ToString(dateTimeFormat);
        hintUsedGameDataStructure.where_poi_id = wherePoiId;
        hintUsedGameDataStructure.when_poi_id = whenPoiId;
        hintUsedGameDataStructure.how_poi_id = howPoiId;

        #if UNITY_EDITOR
        string url = String.Concat(
            remoteWebConsoleSO.HintUsedGate,
            remoteWebConsoleSO.NicknameParameter,
            remoteWebConsoleSO.NicknameValue,
            remoteWebConsoleSO.PasswordParameter,
            remoteWebConsoleSO.PasswordValue);
        #elif UNITY_ANDROID
        string url = String.Concat(
            remoteWebConsoleSO.HintUsedGate,
            remoteWebConsoleSO.NicknameParameter,
            nickname,
            remoteWebConsoleSO.PasswordParameter,
            password);
        #endif

        string dataToSend = JsonConvert.SerializeObject(hintUsedGameDataStructure);

        var www = await SendRemoteData(dataToSend, url);

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("The timestamp for the POI found event was sent successfully.");
        }
        else
        {
            Debug.Log("Error sending the timestamp for the POI found event.");
        }
    }
    
    private async void HandleOnSolutionGivenEvent()
    {
        SolutionGivenDataStructure solutionGivenDataStructure = new();

        solutionGivenDataStructure.player_id = sessionDataSO.PlayerId;
        solutionGivenDataStructure.session_id = sessionDataSO.SessionId;
        solutionGivenDataStructure.timestamp = DateTime.Now.ToString(dateTimeFormat);
        solutionGivenDataStructure.where_poi_id = sessionDataSO.PointsOfInterest.WherePOISolutionId;
        solutionGivenDataStructure.when_poi_id = sessionDataSO.PointsOfInterest.WhenPOIChosenAsSolutionId;
        solutionGivenDataStructure.how_poi_id = sessionDataSO.PointsOfInterest.HowPOIChosenAsSolutionId;

        #if UNITY_EDITOR
        string url = String.Concat(
            remoteWebConsoleSO.SolutionGivenGate,
            remoteWebConsoleSO.NicknameParameter,
            remoteWebConsoleSO.NicknameValue,
            remoteWebConsoleSO.PasswordParameter,
            remoteWebConsoleSO.PasswordValue);
        #elif UNITY_ANDROID
        string url = String.Concat(
            remoteWebConsoleSO.SolutionGivenGate,
            remoteWebConsoleSO.NicknameParameter,
            nickname,
            remoteWebConsoleSO.PasswordParameter,
            password);
        #endif

        string dataToSend = JsonConvert.SerializeObject(solutionGivenDataStructure);

        var www = await SendRemoteData(dataToSend, url);

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("The timestamp for the solution given event was sent successfully.");
        }
        else
        {
            Debug.Log("Error sending the timestamp for the solution given event.");
        }
    }
    #endregion
}
