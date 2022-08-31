using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LoginUIController : MonoBehaviour
{
    #region Inspector
    [Header("SO Listen Channels")]
    [SerializeField]
    private UIEventsChannelSO uiEventsChannelSO;

    [Header("References")]
    [SerializeField]
    private TextMeshProUGUI headerText;
    [SerializeField]
    private TextMeshProUGUI nicknameText;
    [SerializeField]
    private TextMeshProUGUI passwordText;
    [SerializeField]
    private GameObject loadingLabel;
    [SerializeField]
    private GameObject loadingCircle;
    [SerializeField]
    private Button loginButton;
    [SerializeField]
    private TextMeshProUGUI loginButtonText;

    
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
            uiEventsChannelSO.RaiseLoginCredentialsSentEvent("aaa", passwordText.text);
            loginButton.interactable = false;
            //loadingLabel.SetActive(true);
            loadingCircle.SetActive(true);
            Debug.Log(passwordText.text);

        });
    }

    #region Helper Methods

    #endregion

    #region Callback methods
    private void HandleSessionDataLoadedEvent() 
    {
        loginButton.GetComponent<Button>().onClick.AddListener(() => 
        {
            SceneManager.LoadScene("02-AR-Project");
        });

        //loadingLabel.SetActive(false);
        Destroy(loadingCircle);
        loginButtonText.text = "CONTINUE";
        loginButton.interactable = true;
    }
    #endregion
}
