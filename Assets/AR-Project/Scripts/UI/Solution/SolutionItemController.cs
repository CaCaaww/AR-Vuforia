using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class SolutionItemController : MonoBehaviour
{
    #region Inspector
    [Header("LISTEN/SEND Channels")]
    [SerializeField] UIEventsChannelSO uIEventsChannelSO;

    [Header("References")]
    [SerializeField]
    private Button itemButton;
    [SerializeField]
    private Image itemImage;
    [SerializeField]
    private TextMeshProUGUI itemText;
    #endregion

    #region Variables
    private PointOfInterest poi;
    [SerializeField]
    private bool isSelected;
    #endregion

    #region Properties
    public PointOfInterest POI { get => poi; set => poi = value; }
    public bool IsSelected { get => isSelected; set => isSelected = value; }
    #endregion

    #region Unity methods
    void OnEnable()
    {
        uIEventsChannelSO.OnSolutionItemSelectedEventRaised += HandleOtherItemSelected;
    }

    void OnDisable()
    {
        uIEventsChannelSO.OnSolutionItemSelectedEventRaised -= HandleOtherItemSelected;
    }

    void Start()
    {
        itemText.text = poi.title;

        itemButton.onClick.AddListener(SelectPOI);
    }
    #endregion

    #region Helper methods
    private void SelectPOI()
    {
        if(!isSelected)
        {
            isSelected = true;
            itemImage.color = Utils.buttonSelectedColor;

            uIEventsChannelSO.RaiseSolutionItemSelectedEvent(this);
        }
        else
        {
            isSelected = false;
            itemImage.color = Color.white;
        }
    }
    #endregion

    #region Callbacks
    private void HandleOtherItemSelected(SolutionItemController controller) 
    {
        if (controller == this)
            return;

        if (!isSelected)
            return;

        if (controller.POI.type != POI.type)
            return;

        isSelected = false;
        itemImage.color = Color.white;
    }
    #endregion
}
