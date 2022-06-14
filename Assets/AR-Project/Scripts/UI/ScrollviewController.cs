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
    private SessionDataSO sessionDataSO;

    [SerializeField]
    private GameObject buttonPrefab;

    [SerializeField]
    private GameObject buttonParent;

    [SerializeField]
    private EClueType clueType;
    #endregion

    #region Variables
    #endregion

    #region Properties
    #endregion

    #region Unity methods
    private void Awake()
    {

    }

    private void OnEnable()
    {
        uIEventsChannelSO.OnClueFoundNotificationEventRaised += AddPOI;

        
    }

    private void OnDisable()
    {   
        uIEventsChannelSO.OnClueFoundNotificationEventRaised -= AddPOI;
    }

    void Start()
    {
        foreach (var poi in sessionDataSO.PointsOfInterest.Points)
        {
            if (poi.clueType == clueType)
            {
                GameObject inventoryItem = Instantiate(buttonPrefab, buttonParent.transform);

                InventoryItemController inventoryItemController = inventoryItem.GetComponent<InventoryItemController>();

                inventoryItemController.POI = poi;
            }      
        }

    }
    
    void Update()
    {
        
    }
    #endregion
    
    #region Helper methods
    private void AddPOI(PointOfInterest poi)
    {
        if (poi.clueType == clueType)
        {
            GameObject inventoryItem = Instantiate(buttonPrefab, buttonParent.transform);

            InventoryItemController inventoryItemController = inventoryItem.GetComponent<InventoryItemController>();

            inventoryItemController.POI = poi;
        }      
    }

    private void RemovePOI()
    {

    }
    #endregion
}
