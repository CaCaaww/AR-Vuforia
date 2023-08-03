using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryScrollviewController : MonoBehaviour
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
    private EPOIType poiType;
    #endregion

    #region Variables
    Dictionary<PointOfInterest, GameObject> scrollviewItems = new Dictionary<PointOfInterest, GameObject>();
    #endregion

    #region Properties

    #endregion

    #region Unity methods
    private void OnEnable()
    {
        uIEventsChannelSO.OnPOIFoundEventRaised += AddPOI;
        uIEventsChannelSO.OnPOIRemovedEventRaised += RemovePOI;
    }

    private void OnDisable()
    {   
        uIEventsChannelSO.OnPOIFoundEventRaised -= AddPOI;
        uIEventsChannelSO.OnPOIRemovedEventRaised -= RemovePOI;
    }

    void Start()
    {
        PopulateInventoryView(poiType);
    }
    #endregion
    
    #region Helper methods
    private void AddPOI(PointOfInterest poi)
    {
        if (poi.type == poiType)
        {
            GameObject inventoryItem = Instantiate(itemPrefab, itemParent.transform);

            InventoryItemController inventoryItemController = inventoryItem.GetComponent<InventoryItemController>();

            inventoryItemController.POI = poi;

            scrollviewItems.Add(poi, inventoryItem);        
        }      
    }

    private void RemovePOI(PointOfInterest poi)
    {
        if (poi.type == poiType)
        {
            Debug.Log("[UI] REMOVE POI From InventoryView - Title: " + poi.title + " Type: " + poi.type);

            Destroy(scrollviewItems[poi]);

            scrollviewItems.Remove(poi);
        }
    }

    private void PopulateInventoryView(EPOIType poiType)
    {
        List<PointOfInterest> list = new();

        switch (poiType)
        {
            case EPOIType.Where:
                {
                    if (pointsOfInterestSO.WherePOIsFound.Count == 0)
                        return;
                    
                    list = pointsOfInterestSO.WherePOIsFound;
                }
                break;
            case EPOIType.When:
                {
                    if (pointsOfInterestSO.WhenPOIsFound.Count == 0)
                        return;
                    
                    list = pointsOfInterestSO.WhenPOIsFound;
                }
                break;
            case EPOIType.How:
                {
                    if (pointsOfInterestSO.HowPOIsFound.Count == 0)
                        return;

                    list = pointsOfInterestSO.HowPOIsFound;
                }
                break;
            default:
                break;
        }

        if (list.Count == 0)
            return;

        foreach (var poi in list)
        {
            GameObject inventoryItem = Instantiate(itemPrefab, itemParent.transform);

            InventoryItemController inventoryItemController = inventoryItem.GetComponent<InventoryItemController>();

            inventoryItemController.POI = poi;

            scrollviewItems.Add(poi, inventoryItem);
        }
    }
    #endregion
}
