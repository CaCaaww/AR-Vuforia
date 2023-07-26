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
    private TMP_InputField nicknameInputField;
    [SerializeField]
    private TMP_InputField passwordInputField;
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
    /// <summary>
    /// The message to show when the login is not successful
    /// </summary>
    private const string errorMessage = "Login denied: check nickname/password or the network connection";
    #endregion 

    #region Unity methods
    private void OnEnable()
    {
        uiEventsChannelSO.OnSessionDataLoadedEventRaised += HandleSessionDataLoadedEvent;
    }

    private void Start()
    {
        // Hide the log text when selecting the nickname input field
        nicknameInputField.onSelect.AddListener((string text) =>
        {
            if (logText.enabled)
                logText.enabled = false;
        });

        // Hide the log text when selecting the password input field
        passwordInputField.onSelect.AddListener((string text) =>
        {
            if (logText.enabled)
                logText.enabled = false;
        });

        loginButton.onClick.AddListener(Login);

        // Assign the error message to the log text
        logText.text = errorMessage;
    }

    private void OnDisable()
    {
        uiEventsChannelSO.OnSessionDataLoadedEventRaised -= HandleSessionDataLoadedEvent;
    }
    #endregion

    #region Helper Methods
    /// <summary>
    /// The login method to execute when tapping con the login button
    /// </summary>
    private void Login()
    {
        // Raise an LoginCredentialsSentEvent
        uiEventsChannelSO.RaiseLoginCredentialsSentEvent(nicknameInputField.text, passwordInputField.text);

        // Hide the log text
        if (logText.enabled)
                logText.enabled = false;

        // Make the login button not interactable        
        loginButton.interactable = false;

        // Change the login button text
        loginButtonText.text = "LOADING";
        
        // Enable the loading circle
        loadingCircle.SetActive(true);

        Debug.Log("[WEB] Nickname: " + nicknameInputField.text);
        Debug.Log("[WEB] Password: " + passwordInputField.text);
    }
    #endregion

    #region Callback methods
    /// <summary>
    /// Callback to handle if the session data was successfully retrieved or not
    /// </summary>
    /// <param name="success">True if the login was successful, false if not</param>
    private void HandleSessionDataLoadedEvent(bool success) 
    {
        // If it was successsful
        if (success)
        {
            // Remove the listener from the login button
            loginButton.onClick.RemoveListener(Login);

            // Add a lister to the login button to load the AR scene
            loginButton.onClick.AddListener(() => 
            {
                SceneManager.LoadScene("02-AR-Project");

                // Change the login button text   
                loginButtonText.text = "LOADING"; 
            });

            // Hide the log text
            logText.enabled = false;

            // Destroy the loading circle
            Destroy(loadingCircle);

            // Change the login button text
            loginButtonText.text = "CONTINUE";

            // Make the login button interactable
            loginButton.interactable = true;
        }
        else
        {
            Debug.Log("[WEB] Login Denied");
            
            // Hide the loading circle
            loadingCircle.SetActive(false);

            // Enable the log text
            logText.enabled = true;
            
            // Make the login button interactable
            loginButton.interactable = true;

            // Change the login button text
            loginButtonText.text = "LOGIN";
        }     
    }
    #endregion
}
