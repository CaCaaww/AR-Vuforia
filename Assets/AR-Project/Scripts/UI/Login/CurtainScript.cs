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

    public float curtainTime = .25f; // Time for the curtain to fall/rise
    private bool curtainOnScreen = false; // If true, the curtain is covering the screen
    private float length; // height of the screen. Used for transforming the curtain up by just the right length.
    private void Start() {
        length = curtainRectTransform.rect.height; // get the length of the screen
        // if the scene is the first scene, it doesn't show the curtain rising animation
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("01-LoginScene")) {
            curtainOnScreen = false;
        } else { // else, it does
            curtainRectTransform.anchoredPosition = Vector2.zero; // make sure the curtain is covering the screen.
            curtainOnScreen = true;
            Invoke("toggleCurtain", 1); // delaying it for a second so it is smooth.
        }
    }

    public void toggleCurtain() { // called just before we swap scenes in Login UI Controller. Determines whether to move
        // the curtain up or down, then calls the correct function for that.
        if (curtainOnScreen) {
            curtainUp();
        } else {
            curtainDown();
        }
        curtainOnScreen = !curtainOnScreen;
        //Debug.Log("TOGGLING CURTAIN ");
    }
    private void curtainUp() { // moves the curtain up by the length of the screen
        curtainRectTransform.DOAnchorPos(new Vector2(0, length), curtainTime, false).SetEase(Ease.InSine);
    }
    private void curtainDown() { // moves the curtain down to a Y of zero.
        curtainRectTransform.DOAnchorPos(Vector2.zero, curtainTime, false).SetEase(Ease.InSine);
    }
}
