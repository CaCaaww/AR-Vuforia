using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUIController : MonoBehaviour
{
    #region Inspector
    [Header("SO References")]
    [SerializeField] private SessionDataSO sessionDataSO;

    [Header("Inventory References")]
    [SerializeField] private Canvas inventoryCanvas;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button hintButton;

    [Header("Where References")]
    [SerializeField] private Canvas whereCanvas;
    [SerializeField] private Button whereButton;
    [SerializeField] private Image whereButtonBackground;

    [Header("When References")]
    [SerializeField] private Canvas whenCanvas;
    [SerializeField] private Button whenButton;
    [SerializeField] private Image whenButtonBackground;

    [Header("How References")]
    [SerializeField] private Canvas howCanvas;
    [SerializeField] private Button howButton;
    [SerializeField] private Image howButtonBackground;
    #endregion

    #region Variables
    #endregion

    #region Properties
    private Color32 buttonSelectedColor = new Color32(200, 171, 116, 255);
    #endregion

    #region Unity methods
    void Awake()
    {
        SetupUI();
    }
    #endregion
    
    #region Helper methods
    private void WhereButtonBehaviour()
    {
        whereCanvas.enabled = true;
        whereButtonBackground.color = buttonSelectedColor;

        whenCanvas.enabled = false;
        whenButtonBackground.color = Color.white;

        howCanvas.enabled = false;
        howButtonBackground.color = Color.white;
    }

    private void WhenButtonBehaviour()
    {
        whenCanvas.enabled = true;
        whenButtonBackground.color = buttonSelectedColor;
        
        whereCanvas.enabled = false;
        whereButtonBackground.color = Color.white;

        howCanvas.enabled = false; 
        howButtonBackground.color = Color.white;
    }

    private void HowButtonBehaviour()
    {
        howCanvas.enabled = true;
        howButtonBackground.color = buttonSelectedColor;

        whenCanvas.enabled = false;
        whenButtonBackground.color = Color.white;

        whereCanvas.enabled = false;
        whereButtonBackground.color = Color.white;
    }
    
    private void SetupUI()
    {
        // Select the where canvas as the default one when opening the inventory (disable the others)
        whereCanvas.enabled = true;
        whenCanvas.enabled = false;
        howCanvas.enabled = false;

        whereButton.onClick.AddListener(WhereButtonBehaviour);
        whenButton.onClick.AddListener(WhenButtonBehaviour);
        howButton.onClick.AddListener(HowButtonBehaviour);

        closeButton.onClick.AddListener(() =>
        {
            inventoryCanvas.enabled = false;
        });

        // TODO: hint button
    }
    #endregion
}
