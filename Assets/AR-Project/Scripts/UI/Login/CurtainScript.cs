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
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("01-LoginScene")) {
            curtain.SetBool("NotFirstScene", false);
        } else {
            curtain.SetBool("NotFirstScene", true);

        }
        //DontDestroyOnLoad(curtainPrefab);
    }
    bool curtainDown = false;
    
    public void toggleCurtain() {
        curtain.SetTrigger("CurtainToggle");
        //curtainDown = !curtainDown;
        Debug.Log("TOGGLING CURTAIN ");
        //curtain.SetBool("CurtainDown", curtainDown);
    }
}
