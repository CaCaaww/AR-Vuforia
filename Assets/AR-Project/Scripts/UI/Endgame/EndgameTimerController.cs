using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndgameTimerController : MonoBehaviour
{

    #region Inspector
    
    [SerializeField]
    public TextMeshProUGUI timeCounter;
    
    #endregion

    #region Variables

    public static EndgameTimerController instance;

    private TimeSpan timePlaying;

    private bool timerGoing;

    private float elapsedTime;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        timeCounter.text = "You played for 00:00 minutes!";
        //timerGoing = false;

        //This is just for testing, once a start button is implemented this should go in there
        BeginTimer();
    }

    public void BeginTimer()
    {
        timerGoing = true;
        elapsedTime = 0f;

        StartCoroutine(UpdateTimer());
    }

    public void EndTimer()
    {
        timerGoing= false;
    }

    private IEnumerator UpdateTimer()
    {

        while (timerGoing)
        {
            //Debug.Log(timePlaying.ToString("mm:ss"));
            elapsedTime += Time.deltaTime;
            timePlaying = TimeSpan.FromSeconds(elapsedTime);
            //string timePlayingStr = "You played for " + timePlaying.ToString("mm ':' ss") + " minutes";
            timeCounter.SetText("You played for " + timePlaying.Minutes + " minutes and " + timePlaying.Seconds + " seconds");

            yield return null;
        }

    }

    #endregion

    #region Callback Methods

    #endregion

}
