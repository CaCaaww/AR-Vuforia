using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;


public class Utils : MonoBehaviour
{
    //public static Color32 buttonSelectedColor = new Color32(200, 171, 116, 255);
    public static Color32 buttonSelectedColor = new Color32(140, 69, 0, 255);

    public static void QuitGame()
    {
        Application.Quit();
    }

    /// <summary>
    /// Get a remote texture via web request
    /// </summary>
    public static async Task<Texture2D> GetRemoteTexture(string url)
    {
        Debug.Log("[WEB] URL image: " + url);
        
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
            Debug.Log(request.error + ": " + url);
            #endif
            return null;
        }
    }

    // Convert a texture to a sprite
    public static Sprite Texture2DToSprite(Texture2D texture)
    {   
        return Sprite.Create(texture, 
                            new Rect(0f, 0f, texture.width, texture.height), 
                            new Vector2(0.5f, 0.5f), 50f, 0, SpriteMeshType.FullRect);
    }
    

    public static T Attach<T>(GameObject go, int arg1, int arg2, string arg3) where T : Component
    {
            T componentToAttach = go.AddComponent<T>();
            //componentToAttach.SetupSomeStuffWithTheArgs(arg1, arg2, arg3);
            // do any other Awake() or Start() like stuff you want here
            return componentToAttach;
    }

    public static T Create<T>(int arg1, int arg2, string arg3) where T : Component
    {
        return Attach<T>(new GameObject("ClassExample.Create();"), arg1, arg2, arg3);
    }
}


