using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollviewController : MonoBehaviour
{
    #region Inspector
    [Header("LISTEN Channels")]
    [SerializeField]
    private UIEventsChannelSO uIEventsChannelSO;

    [Header("SO References")]
    [SerializeField]
    private PointsOfInterestSO pointsOfInterestSO;

    [SerializeField]
    private GameObject itemPrefab;

    [SerializeField]
    private GameObject itemParent;

    [SerializeField]
    private EPOIType clueType;
    #endregion

    #region Variables
    Dictionary<PointOfInterest, GameObject> scrollviewItems = new Dictionary<PointOfInterest, GameObject>();
    #endregion

    #region Properties
    #endregion

    #region Unity methods
    private void Awake()
    {

    }

    private void OnEnable()
    {
        uIEventsChannelSO.OnPOIFoundEventRaised += AddPOI;       
    }

    private void OnDisable()
    {   
        uIEventsChannelSO.OnPOIFoundEventRaised -= AddPOI;
    }

    void Start()
    {
        foreach (var poi in pointsOfInterestSO.Points)
        {
            if (poi.clueType == clueType)
            {
                GameObject inventoryItem = Instantiate(itemPrefab, itemParent.transform);

                InventoryItemController inventoryItemController = inventoryItem.GetComponent<InventoryItemController>();

                inventoryItemController.POI = poi;

                scrollviewItems.Add(poi, inventoryItem);
            }      
        }
    }
    #endregion
    
    #region Helper methods
    private void AddPOI(PointOfInterest poi)
    {
        if (poi.clueType == clueType)
        {
            GameObject inventoryItem = Instantiate(itemPrefab, itemParent.transform);

            InventoryItemController inventoryItemController = inventoryItem.GetComponent<InventoryItemController>();

            inventoryItemController.POI = poi;        
        }      
    }

    private void RemovePOI()
    {
        //TO DO
    }
    #endregion
}
