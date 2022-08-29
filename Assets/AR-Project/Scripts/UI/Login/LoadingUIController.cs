using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LoadingUIController : MonoBehaviour
{
    #region Inspector
    [Header("SO Listen Channels")]
    [SerializeField]
    private UIEventsChannelSO uiEventsChannelSO;

    [Header("References")]
    [SerializeField]
    private TMP_InputField accessCodeText;
    [SerializeField]
    private GameObject loadingLabel;
    [SerializeField]
    private GameObject loginButton;
    [SerializeField]
    private GameObject continueButton;
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

    private void Start()
    {
        loginButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            uiEventsChannelSO.RaiseLoginCredentialsSentEvent("aaa", accessCodeText.text);
            loginButton.SetActive(false);
            loadingLabel.SetActive(true);
            Debug.Log(accessCodeText.text);
        });
    }

    #region Callback methods
    private void HandleSessionDataLoadedEvent() 
    {
        continueButton.GetComponent<Button>().onClick.AddListener(() => 
        {
            SceneManager.LoadScene("02-AR-Project");
        });
        loadingLabel.SetActive(false);
        continueButton.SetActive(true);
    }
    #endregion
}
