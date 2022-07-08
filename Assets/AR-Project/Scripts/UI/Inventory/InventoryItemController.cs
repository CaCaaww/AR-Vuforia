using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryItemController : MonoBehaviour
{
    #region Inspector
    [Header("SEND Channels")]
    [SerializeField] UIEventsChannelSO uIEventsChannelSO;

    [Header("SO References")]
    [SerializeField] private POIIconCollectionSO poiIconCollectionSO;

    [Header("UI References")]
    [SerializeField] private Button itemButton;
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemText;
    #endregion

    #region Variables
    private PointOfInterest poi;
    #endregion

    #region Properties
    public PointOfInterest POI { get => poi; set => poi = value; }
    #endregion

    #region Unity methods
    void Start()
    {
        itemText.text = poi.title;

        itemIcon.sprite = poiIconCollectionSO.GetIconByType(poi.iconType);

        itemButton.onClick.AddListener(ViewPOI);
    }
    #endregion

    #region Helper methods
    private void ViewPOI()
    {
        uIEventsChannelSO.RaiseOnPOIViewEvent(poi);
    }
    #endregion
}
