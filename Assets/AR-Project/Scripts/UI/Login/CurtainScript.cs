using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurtainScript : MonoBehaviour
{
    [SerializeField]
    private Animator curtain;  
    static bool curtainDown = false;
    public float transitionTime = 1f;
    public void toggleCurtain() {
        curtainDown = !curtainDown;
        Debug.Log("TOGGLING CURTAIN");
        curtain.SetBool("CurtainDown", curtainDown);
    }
}
