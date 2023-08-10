using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Color = System.Drawing.Color;

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
    [SerializeField] private Image frameSelected;
    [SerializeField] private Image frameNotSelected;
    
    [Header("UI Style References")]
    [SerializeField] private Sprite itemSelected;
    [SerializeField] private Sprite itemUnselected;
    #endregion

    #region Variables
    private PointOfInterest poi;
    [SerializeField]
    private bool isSelected;

    private UnityEngine.Color selectedColor;
    private UnityEngine.Color notSelectedColor;
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
        
        selectedColor = new UnityEngine.Color(.62f, .24f, .13f);
        //notSelectedColor = new UnityEngine.Color(1, .56f, .45f);
        notSelectedColor = UnityEngine.Color.white;

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
            
            //change the border
            frameSelected.enabled = true;
            frameNotSelected.enabled = false;
            itemImage.color = selectedColor;

        }
        else
        {
            isSelected = false;
            itemImage.sprite = itemUnselected;
            uIEventsChannelSO.RaiseSolutionItemDeselectedEvent(this);
            frameSelected.enabled = false;
            frameNotSelected.enabled = true;
            itemImage.color = notSelectedColor;


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
        frameSelected.enabled = false;
        frameNotSelected.enabled = true;
        itemImage.color = notSelectedColor;

    }
    #endregion
}
