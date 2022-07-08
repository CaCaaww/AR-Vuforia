using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SolutionItemController : MonoBehaviour
{
    #region Inspector
    [Header("LISTEN/SEND Channels")]
    [SerializeField] UIEventsChannelSO uIEventsChannelSO;

    [Header("SO References")]
    [SerializeField] private POIIconCollectionSO poiIconCollectionSO;

    [Header("UI References")]
    [SerializeField] private Button itemButton;
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemText;
    [SerializeField] private Image itemIcon;

    [Header("UI Style References")]
    [SerializeField] private Sprite itemSelected;
    [SerializeField] private Sprite itemUnselected;
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

        itemIcon.sprite = poiIconCollectionSO.GetIconByType(poi.iconType);

        itemImage.sprite = itemUnselected;
    }
    #endregion

    #region Helper methods
    private void SelectPOI()
    {
        if(!isSelected)
        {
            isSelected = true;

            itemImage.sprite = itemSelected;

            uIEventsChannelSO.RaiseSolutionItemSelectedEvent(this);
        }
        else
        {
            isSelected = false;
            itemImage.sprite = itemUnselected;
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
        itemImage.sprite = itemImage.sprite = itemUnselected;
    }
    #endregion
}
