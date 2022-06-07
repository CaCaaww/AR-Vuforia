using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;


public class Utils : MonoBehaviour
{
    public static void QuitGame()
    {
        Application.Quit();
    }

    /// <summary>
    /// Get a remote texture via web request
    /// </summary>
    public static async Task<Texture2D> GetRemoteTexture(string url)
    {
        Debug.Log("url image: " + url);
        
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);

        request.timeout = 10;

        var operation = request.SendWebRequest();

        while (!operation.isDone)
            await Task.Yield();

        if(request.result == UnityWebRequest.Result.Success) 
        {
            return DownloadHandlerTexture.GetContent(request);   
        }
        else 
        {
            #if UNITY_EDITOR
            Debug.Log(request.error);
            #endif
            return null;
        }
    }
}


