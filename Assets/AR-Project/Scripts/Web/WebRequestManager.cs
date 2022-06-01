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
    #endregion

    #region Unity methods

    async void Start()
    {
        var result = await TestPost();

        Debug.Log("Result: " + result);
    }
    
    #endregion
    
    #region Helper methods
    private async Task<string> TestPost()
    {
        WWWForm form = new WWWForm();
        form.AddField("code", remoteWebConsoleSO.AccessCode);

        var www = UnityWebRequest.Get(remoteWebConsoleSO.JoinGate);

        www.SetRequestHeader("Content-Type", "application/json");

        www.SetRequestHeader("code", remoteWebConsoleSO.AccessCode);

        Debug.Log(www.GetRequestHeader("code"));

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
