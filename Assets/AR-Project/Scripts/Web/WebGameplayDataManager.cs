using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class WebGameplayDataManager : MonoBehaviour
{   
    #region Constants
    private const string dateTimeFormat = "yyyy-MM-dd HH:mm:ss";
    private const string PRODUCTION_REMOTE_SETTINGS_SO = "RemoteWebConsoleProduction";
    private const string TESTING_REMOTE_SETTINGS_SO = "RemoteWebConsoleTesting";
    #endregion

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

    #region Unity methods
    void Awake()
    {
        #if !UNITY_EDITOR && TESTING_BUILD
        RemoteWebConsoleSO remoteSettingsSO = Resources.Load<RemoteWebConsoleSO>(TESTING_REMOTE_SETTINGS_SO);
        remoteWebConsoleSO = remoteSettingsSO;
        #elif !UNITY_EDITOR && PRODUCTION_BUILD
        RemoteWebConsoleSO remoteSettingsSO = Resources.Load<RemoteWebConsoleSO>(PRODUCTION_REMOTE_SETTINGS_SO);
        remoteWebConsoleSO = remoteSettingsSO;
        #endif
    }

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
    private async Task<UnityWebRequest> SendRemoteData(string url, WWWForm form)
    {
        var www = UnityWebRequest.Post(url, form);

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
        WWWForm form = new();       
        form.AddField("player_id", sessionDataSO.PlayerId.ToString());
        form.AddField("session_id", sessionDataSO.SessionId.ToString());
        form.AddField("timestamp", DateTime.Now.ToString(dateTimeFormat));

        #if UNITY_EDITOR
        string url = remoteWebConsoleSO.StartGameGate;
        #elif UNITY_ANDROID
        string url = remoteWebConsoleSO.StartGameGate;
        #endif
       
        //string dataToSend = "\"player_id\":2";

        var www = await SendRemoteData(url, form);

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log($"[GAME STARTED SEND] Error: {www.downloadHandler.text}");
            Debug.Log("Error sending the timestamp for the start of the game.");
        }
        else
        {
            Debug.Log($"[GAME STARTED SEND] Success: {www.downloadHandler.text}");
            Debug.Log("The timestamp for the start of the game was sent successfully.");
        }

        www.Dispose();
    }

    private async void HandleOnPOIFoundEvent(PointOfInterest poi)
    {
        WWWForm form = new();
        form.AddField("player_id", sessionDataSO.PlayerId.ToString());
        form.AddField("session_id", sessionDataSO.SessionId.ToString());
        form.AddField("timestamp", DateTime.Now.ToString(dateTimeFormat));
        form.AddField("poi_id", poi.id.ToString());

        #if UNITY_EDITOR
        string url = remoteWebConsoleSO.POIFoundGate;
        #elif UNITY_ANDROID
        string url = remoteWebConsoleSO.POIFoundGate;
        #endif

        var www = await SendRemoteData(url, form);

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log($"[POI FOUND SEND] Error: {www.downloadHandler.text}");
            Debug.Log("Error sending the timestamp for the POI found event.");
        }
        else
        {
            Debug.Log($"[POI FOUND SEND] Success: {www.downloadHandler.text}");
            Debug.Log("The timestamp for the POI found event was sent successfully.");
        }

        www.Dispose();
    }

    private async void HandlePOIDeletedByHintEvent(int wherePoiId, int whenPoiId, int howPoiId)
    {
        WWWForm form = new();

        form.AddField("player_id", sessionDataSO.PlayerId.ToString());
        form.AddField("session_id", sessionDataSO.SessionId.ToString());
        form.AddField("timestamp", DateTime.Now.ToString(dateTimeFormat));
        form.AddField("where_poi_id", wherePoiId.ToString());
        form.AddField("when_poi_id", whenPoiId.ToString());
        form.AddField("how_poi_id", howPoiId.ToString());
        form.AddField("remaining_hints", sessionDataSO.Hints);

        #if UNITY_EDITOR
        string url = remoteWebConsoleSO.HintUsedGate;
        #elif UNITY_ANDROID
        string url = remoteWebConsoleSO.HintUsedGate;
        #endif

        var www = await SendRemoteData(url, form);

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log($"[HINT USED SEND] Error: {www.downloadHandler.text}");
            Debug.Log("Error sending the timestamp for the hint used event.");
        }
        else
        {
            Debug.Log($"[HINT USED SEND] Success: {www.downloadHandler.text}");
            Debug.Log("The timestamp for the hint used found event was sent successfully.");
        }

        www.Dispose();
    }
    
    private async void HandleOnSolutionGivenEvent()
    {
        WWWForm form = new();

        form.AddField("player_id", sessionDataSO.PlayerId.ToString());
        form.AddField("session_id", sessionDataSO.SessionId.ToString());
        form.AddField("timestamp", DateTime.Now.ToString(dateTimeFormat));
        form.AddField("where_poi_id", sessionDataSO.PointsOfInterest.WherePOIChosenAsSolutionId.ToString());
        form.AddField("when_poi_id", sessionDataSO.PointsOfInterest.WhenPOIChosenAsSolutionId.ToString());
        form.AddField("how_poi_id", sessionDataSO.PointsOfInterest.HowPOIChosenAsSolutionId.ToString());

        #if UNITY_EDITOR
        string url = remoteWebConsoleSO.SolutionGivenGate;
        #elif UNITY_ANDROID
        string url = remoteWebConsoleSO.SolutionGivenGate;
        #endif

        var www = await SendRemoteData(url, form);

        Dictionary<string, string> response = new();

        //Debug.Log("END MESSAGE: " + response["message"]);

        //Debug.Log("END TIME: " + response["time"]);

        if (www.result != UnityWebRequest.Result.Success)
        {
            response = JsonConvert.DeserializeObject<Dictionary<string, string>>(www.downloadHandler.text);

            Debug.Log($"[SOLUTION SEND] Error: {response["message"]}");
            Debug.Log("Error sending the timestamp for the solution given event.");

            uiEventsChannelSO.RaiseGamePlaytimeReceveidEvent(0);
        }
        else
        {
            response = JsonConvert.DeserializeObject<Dictionary<string, string>>(www.downloadHandler.text);

            Debug.Log($"[SOLUTION SEND] Success: {response["message"]}");
            Debug.Log("The timestamp for the solution given event was sent successfully.");

            uiEventsChannelSO.RaiseGamePlaytimeReceveidEvent(int.Parse(response["time"]));
        }

        www.Dispose();
    }
    #endregion
}
