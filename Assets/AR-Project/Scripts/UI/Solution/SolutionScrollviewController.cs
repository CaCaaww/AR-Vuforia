using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolutionScrollviewController : MonoBehaviour
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
        #if UNITY_EDITOR
            PopulateSolutionView();
        #endif
    }
    #endregion
    
    #region Helper methods
    private void AddPOI(PointOfInterest poi)
    {
        if (poi.type == poiType)
        {
            GameObject inventoryItem = Instantiate(itemPrefab, itemParent.transform);

            SolutionItemController solutionItemController = inventoryItem.GetComponent<SolutionItemController>();

            solutionItemController.POI = poi;

            scrollviewItems.Add(poi, inventoryItem);        
        }      
    }

    private void RemovePOI(PointOfInterest poi)
    {
        if (poi.type == poiType)
        {
            Destroy(scrollviewItems[poi].gameObject);

            scrollviewItems.Remove(poi);
        }
    }
    #endregion

    #region Editor-only methods
    private void PopulateSolutionView()
    {
        foreach (var poi in pointsOfInterestSO.Points)
        {
            if (poi.type == poiType)
            {
                GameObject inventoryItem = Instantiate(itemPrefab, itemParent.transform);

                SolutionItemController solutionItemController = inventoryItem.GetComponent<SolutionItemController>();

                solutionItemController.POI = poi;

                scrollviewItems.Add(poi, inventoryItem);
            }      
        }
    }
    #endregion
}
