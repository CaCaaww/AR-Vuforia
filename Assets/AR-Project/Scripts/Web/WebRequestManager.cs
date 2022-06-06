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
    [Header("References")]
    [SerializeField]
    private RemoteWebConsoleSO remoteWebConsoleSO;
    #endregion

    #region Variables
    #endregion

    #region Properties
    private DataStructure dataStructure;
    #endregion

    #region Unity methods

    async void Start()
    {
        var result = await TestPost();

        dataStructure = JsonConvert.DeserializeObject<DataStructure>(result);

        Debug.Log("Number of hints: " + dataStructure.hints);

        Debug.Log("Number of images: " + dataStructure.arPOIs.Count);

        for (int i = 0; i < dataStructure.arPOIs.Count; i++)
        {
            Debug.Log(dataStructure.arPOIs[i].name);
            Debug.Log(dataStructure.arPOIs[i].type);
            Debug.Log(dataStructure.arPOIs[i].description);
            Debug.Log(dataStructure.arPOIs[i].image_name);
            //Debug.Log(dataStructure.arPOIs[i].image_url);
        }
    }
    #endregion
    
    #region Helper methods
    private async Task<string> TestPost()
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
