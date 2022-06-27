using System;
using System.Collections;
using UnityEngine;

public class EndgameTimerController : MonoBehaviour
{
    #region Private variables
    private TimeSpan timePlaying;
    private bool timerGoing;
    private float elapsedTime;
    #endregion

    #region Public properties
    public TimeSpan TimePlaying => timePlaying;
    #endregion

    #region Helper methods
    public void StartTimer()
    {
        timerGoing = true;
        elapsedTime = 0f;

        StartCoroutine(UpdateTimer());
    }

    public void StopTimer()
    {
        timerGoing = false;
    }
    #endregion

    #region Coroutines
    private IEnumerator UpdateTimer()
    {
        while (timerGoing)
        {
            //Debug.Log(timePlaying.ToString("mm:ss"));
            elapsedTime += Time.deltaTime;
            timePlaying = TimeSpan.FromSeconds(elapsedTime);
            yield return null;
        }
    }
    #endregion
}