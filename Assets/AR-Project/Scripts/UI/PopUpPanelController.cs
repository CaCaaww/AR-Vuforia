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
    [SerializeField] private AREventChannelSO arEventChannelSO;

    [Header("SEND Channels")]
    /// <summary>
    /// The SO channel for the DebugUI events
    /// </summary>
    //[Tooltip("The SO channel for the DebugUI events")]
    //[SerializeField] private DebugUIEventChannelSO debugUIEventChannelSO;

    [Header("References")]
    [SerializeField] private PointsOfInterestSO pointsOfInterestSO;
    [SerializeField] private Canvas canvas;
    [SerializeField] private TextMeshProUGUI objectTitle;
    [SerializeField] private TextMeshProUGUI objectDescription;
    [SerializeField] private RawImage objectImage;
    [SerializeField] private Button closeButton;
    #endregion

    #region UnityMethods
    private void OnEnable()
    {
        arEventChannelSO.OnPOIDetected += HandlePOIDetection;
    }

    private void OnDisable()
    {
        arEventChannelSO.OnPOIDetected -= HandlePOIDetection;
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
    private void HandlePOIDetection(string imageName)
    {
        canvas.enabled = true;
        foreach (var point in pointsOfInterestSO.Points)
        {
            if (point.imageName == imageName)
            {
                objectTitle.text = point.imageName;
                objectImage.texture = point.image;

                break;
            }
        }
    }
    #endregion
}
