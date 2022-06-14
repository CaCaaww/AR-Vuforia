using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryItemController : MonoBehaviour
{
    #region Inspector
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
    }
    #endregion
    
    #region Helper methods
    #endregion
}
