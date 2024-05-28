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
    private void Start() {
        length = curtainRectTransform.rect.height;
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("01-LoginScene")) {
            curtainOnScreen = false;
        } else {
            curtainRectTransform.anchoredPosition = Vector2.zero;
            curtainOnScreen = true;
            Invoke("toggleCurtain", 1);
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
        //Debug.Log("||||||| Y Position: " + curtainRectTransform.anchoredPosition.y);
        curtainRectTransform.DOAnchorPos(new Vector2(0, length), curtainTime, false).SetEase(Ease.InSine);
    }
    private void curtainDown() { 
        curtainRectTransform.DOAnchorPos(Vector2.zero, curtainTime, false);
    }
}
