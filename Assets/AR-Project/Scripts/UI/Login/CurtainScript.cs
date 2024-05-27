using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class CurtainScript : MonoBehaviour {
    [SerializeField]
    private GameObject curtainPrefab;
    [SerializeField]
    private RectTransform curtainRectTransform;

    public float curtainTime = .25f;
    private bool curtainOnScreen = false;
    private float length;
    private void Awake() {
        length = curtainRectTransform.rect.height;
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("01-LoginScene")) {
            curtainOnScreen = false;
        } else {
            curtainOnScreen = true;
            toggleCurtain();
        }
    }
    
    public void toggleCurtain() {
        if (curtainOnScreen) {
            curtainUp();
        } else {
            curtainDown();
        }
        curtainOnScreen = !curtainOnScreen;
        //Debug.Log("TOGGLING CURTAIN ");
    }
    private void curtainUp() {
        curtainRectTransform.DOAnchorPos(new Vector2(0, length), curtainTime, false);
    }
    private void curtainDown() { 
        curtainRectTransform.DOAnchorPos(Vector2.zero, curtainTime, false);
    }
}
