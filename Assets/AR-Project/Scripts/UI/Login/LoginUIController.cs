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
    private TextMeshProUGUI logText;
    [SerializeField]
    private GameObject loadingCircle;
    [SerializeField]
    private Button loginButton;
    [SerializeField]
    private TextMeshProUGUI loginButtonText;
    #endregion

    #region Private variables
    private const string errorMessage = "Login denied: check nickname/password or the network connection";
    #endregion 

    #region Unity methods
    private void OnEnable()
    {
        uiEventsChannelSO.OnSessionDataLoadedEventRaised += HandleSessionDataLoadedEvent;
    }

    private void Start()
    {
        loginButton.GetComponent<Button>().onClick.AddListener(Login);
    }

    private void OnDisable()
    {
        uiEventsChannelSO.OnSessionDataLoadedEventRaised -= HandleSessionDataLoadedEvent;
    }
    #endregion

    #region Helper Methods
    private void Login()
    {
        uiEventsChannelSO.RaiseLoginCredentialsSentEvent(nicknameText.text, passwordText.text);
        loginButton.interactable = false;
        //loadingLabel.SetActive(true);
        loadingCircle.SetActive(true);
        Debug.Log("[WEB] Nickname: " + nicknameText.text);
        Debug.Log("[WEB] Password: " + passwordText.text);
    }
    #endregion

    #region Callback methods
    private void HandleSessionDataLoadedEvent(bool success) 
    {
        if (success)
        {
            loginButton.onClick.RemoveListener(Login);

            loginButton.onClick.AddListener(() => 
            {
                SceneManager.LoadScene("02-AR-Project");
            });

            logText.enabled = false;
            Destroy(loadingCircle);
            loginButtonText.text = "CONTINUE";
            loginButton.interactable = true;
        }
        else
        {
            loadingCircle.SetActive(false);
            logText.enabled = true;
            logText.text = errorMessage;
            loginButton.interactable = true;
        }     
    }
    #endregion

    
}
