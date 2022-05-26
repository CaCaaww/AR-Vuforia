using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopUpPanelController : MonoBehaviour
{
    #region Inspector
    [Header("LISTEN Channels")]
    /// <summary>
    /// The SO channel for the AR events
    /// </summary>
    [Tooltip("The SO channel for the AR events")]
    [SerializeField] private UIEventsChannelSO uiEventsChannelSO;

    [Header("SEND Channels")]
    /// <summary>
    /// The SO channel for the DebugUI events
    /// </summary>
    //[Tooltip("The SO channel for the DebugUI events")]
    //[SerializeField] private DebugUIEventChannelSO debugUIEventChannelSO;

    [Header("References")]
    [SerializeField] private Canvas canvas;
    [SerializeField] private TextMeshProUGUI objectTitle;
    [SerializeField] private TextMeshProUGUI objectDescription;
    [SerializeField] private RawImage objectImage;
    [SerializeField] private TrackableObjectSO trackableObjectSO;
    [SerializeField] private Button closeButton;
    #endregion



    #region UnityMethods

    private void OnEnable()
    {
        uiEventsChannelSO.OnClueFoundNotificationEventRaised += HandleClueFoundNotification;
    }

    private void OnDisable()
    {
        uiEventsChannelSO.OnClueFoundNotificationEventRaised -= HandleClueFoundNotification;
    }

    private void Awake()
    {
        closeButton.onClick.AddListener(() =>
        {
            canvas.enabled = false;
        });
    }

    #endregion

    #region Callbacks

    private void HandleClueFoundNotification(string imageName)
    {
        canvas.enabled = true;
        foreach (var trackedObject in trackableObjectSO.trackableObjects)
        {
            if (trackedObject.objectName == imageName)
            {

                objectTitle.text = trackedObject.objectName;
                objectDescription.text = trackedObject.objectDescription;
                objectImage = trackedObject.objectImage;

                break;
            }
        }
    }

    #endregion
}
