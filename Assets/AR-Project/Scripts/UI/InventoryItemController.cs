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
    #endregion

    #region Properties
    public PointOfInterest POI { get => poi; set => poi = value; }
    #endregion

    #region Unity methods
    void Start()
    {
        itemText.text = poi.title;

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
