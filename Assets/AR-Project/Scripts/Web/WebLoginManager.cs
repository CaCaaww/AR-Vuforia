using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class WebLoginManager : MonoBehaviour
{
    #region Constants
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

    #region Variables
    #endregion

    #region Properties
    private LoginGetDataStructure dataStructure;
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
        sessionDataSO.PointsOfInterest.ResetVariables();
        #endif


    }

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
        string url = String.Concat(
            remoteWebConsoleSO.JoinGate,
            remoteWebConsoleSO.NicknameParameter,
            remoteWebConsoleSO.NicknameValue,
            remoteWebConsoleSO.PasswordParameter,
            remoteWebConsoleSO.PasswordValue);
        #elif UNITY_ANDROID
        nickname = nickname.Replace("#", "%23");
        string url = String.Concat(
            remoteWebConsoleSO.JoinGate,
            remoteWebConsoleSO.NicknameParameter,
            nickname,
            remoteWebConsoleSO.PasswordParameter,
            password);
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

        dataStructure = JsonConvert.DeserializeObject<LoginGetDataStructure>(www.downloadHandler.text);

        if (dataStructure == null )
        {
            Debug.LogError("The object downloaded from the server is null");
        }

        #if UNITY_EDITOR
        Debug.Log("DATASTRUCTURE");
        foreach(KeyValuePair<string, string> entry in dataStructure.pois[0].images)
        {
            Debug.Log("Key: " + entry.Key);
            Debug.Log("Value: " + entry.Value);
        }
        #endif

        sessionDataSO.PlayerId = dataStructure.player_id;
        Debug.Log("[WEB] Player ID: " + sessionDataSO.PlayerId);

        sessionDataSO.SessionId = dataStructure.session_id;
        Debug.Log("[WEB] Session ID: " + sessionDataSO.SessionId);

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

        sessionDataSO.ResumeSession = dataStructure.resume_session;
        Debug.Log("[WEB] Resuming a previous session: " + sessionDataSO.ResumeSession);

        int numberOfPois = dataStructure.pois.Count;
        Debug.Log("[WEB] Number of POIs: " + numberOfPois);

        #if UNITY_EDITOR
        // Reset the variables in the editor 
        sessionDataSO.PointsOfInterest.ResetVariables();
        #endif

        for (int i = 0; i < numberOfPois; i++)
        {
            // Add a new POI object
            sessionDataSO.PointsOfInterest.Points.Add(new PointOfInterest());

            // Set the POI id
            sessionDataSO.PointsOfInterest.Points[i].id = dataStructure.pois[i].id;
            Debug.Log("[WEB] ID: " + dataStructure.pois[i].id);

            // Set the POI title
            sessionDataSO.PointsOfInterest.Points[i].title = dataStructure.pois[i].title;
            Debug.Log("[WEB] Title: " + dataStructure.pois[i].title);

            // Set the POI short title
            sessionDataSO.PointsOfInterest.Points[i].short_title = dataStructure.pois[i].short_title;
            Debug.Log("[WEB] Title: " + dataStructure.pois[i].short_title);

            // Set the POI type
            sessionDataSO.PointsOfInterest.Points[i].type = (EPOIType)dataStructure.pois[i].type;
            Debug.Log("[WEB] Clue type: " + (EPOIType)dataStructure.pois[i].type);

            // Set the POI description
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
                
                // For debug in the Unity Editor only
                #if UNITY_EDITOR && !UNITY_ANDROID
                sessionDataSO.PointsOfInterest.Points[i].images.Add(sessionDataSO.PointsOfInterest.Points[i].imageNameAndTexture[image.Key]);
                #endif
            }

            // Set if the POI is useful on not for the solution
            sessionDataSO.PointsOfInterest.Points[i].isUseful = dataStructure.pois[i].is_useful;
            Debug.Log("[WEB] Is useful to to the solution: " + dataStructure.pois[i].is_useful);

            // If useful store the id in a variable (taking account of the POI type)
            if (dataStructure.pois[i].is_useful)
            {
                switch (sessionDataSO.PointsOfInterest.Points[i].type)
                {
                    case EPOIType.Where:
                        {
                            sessionDataSO.PointsOfInterest.WherePOISolutionId = sessionDataSO.PointsOfInterest.Points[i].id;
                        }
                        break;
                    case EPOIType.When:
                        {
                            sessionDataSO.PointsOfInterest.WhenPOISolutionId = sessionDataSO.PointsOfInterest.Points[i].id;
                        }
                        break;
                    case EPOIType.How:
                        {
                            sessionDataSO.PointsOfInterest.HowPOISolutionId = sessionDataSO.PointsOfInterest.Points[i].id;
                        }
                        break;
                }
            }

            // Set the icon type for the POI
            sessionDataSO.PointsOfInterest.Points[i].iconType = (EIconType)dataStructure.pois[i].icon_type;
            Debug.Log("[WEB] Icon type: " + (EIconType)dataStructure.pois[i].icon_type);

            // Set the avatar id for the POI
            //sessionDataSO.PointsOfInterest.Points[i].avatarID = dataStructure.pois[i].avatar_id;
            //Debug.Log("[WEB] Avatar id: " + dataStructure.pois[i].avatar_id);

            // Set the avatar name for the POI
            //sessionDataSO.PointsOfInterest.Points[i].avatarName = dataStructure.pois[i].avatar_name;
            //Debug.Log("[WEB] Avatar name: " + dataStructure.pois[i].avatar_name);

            // Set the avatar image name for the POI
            sessionDataSO.PointsOfInterest.Points[i].avatarImage = Utils.Texture2DToSprite(await Utils.GetRemoteTexture(dataStructure.pois[i].avatar_image));

            // Set the state Enum from the remote data
            sessionDataSO.PointsOfInterest.Points[i].state = (EPOIState)dataStructure.pois[i].detected;

            // If we are resuming a previous session
            if (sessionDataSO.ResumeSession)
            {
                // If the POI was previously detected
                if (dataStructure.pois[i].detected == (int)EPOIState.Detected)
                {
                    // Check the type and add the POI to the respective list
                    switch (sessionDataSO.PointsOfInterest.Points[i].type)
                    {
                        case EPOIType.Where:
                            {
                                sessionDataSO.PointsOfInterest.WherePOIsFound.Add(sessionDataSO.PointsOfInterest.Points[i]);
                            }
                            break;
                        case EPOIType.When:
                            {
                                sessionDataSO.PointsOfInterest.WhenPOIsFound.Add(sessionDataSO.PointsOfInterest.Points[i]);
                            }
                            break;
                        case EPOIType.How:
                            {
                                sessionDataSO.PointsOfInterest.HowPOIsFound.Add(sessionDataSO.PointsOfInterest.Points[i]);
                            }
                            break;
                    }
                }   
            }
        }

        // Raise an event for finishing the loading of the session data
        uiEventsChannelSO.RaiseSessionDataLoadedEvent(true);

        www.Dispose();
    }
#endregion
}
