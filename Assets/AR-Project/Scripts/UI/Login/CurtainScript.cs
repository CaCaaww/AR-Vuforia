using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class CurtainScript : MonoBehaviour
{

    [SerializeField]
    private Animator curtain;
    [SerializeField]
    private GameObject curtainPrefab;
    private void Awake() {
        DontDestroyOnLoad(curtainPrefab);
    }
    bool curtainDown = false;
    
    public void toggleCurtain() {
        curtainDown = !curtainDown;
        Debug.Log("TOGGLING CURTAIN ");
        curtain.SetBool("CurtainDown", curtainDown);
    }
}
