using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginUIController : MonoBehaviour
{
    #region Inspector
    [Header("SO Listen Channels")]
    [SerializeField]
    private UIEventsChannelSO uiEventsChannelSO;

    [Header("References")]
    [SerializeField]
    private GameObject loginButton;
    #endregion

    #region Unity methods
    private void OnEnable()
    {
        uiEventsChannelSO.OnSessionDataLoadedEventRaised += HandleSessionDataLoadedEvent;
    }

    private void OnDisable()
    {
        uiEventsChannelSO.OnSessionDataLoadedEventRaised -= HandleSessionDataLoadedEvent;
    }
    #endregion

    #region Callback methods
    private void HandleSessionDataLoadedEvent() 
    {
        loginButton.GetComponent<Button>().onClick.AddListener(() => 
        {
            SceneManager.LoadScene("02-AR-Project");
        });

        loginButton.SetActive(true);
    }
    #endregion
}
