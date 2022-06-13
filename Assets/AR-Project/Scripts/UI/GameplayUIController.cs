using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUIController : MonoBehaviour
{
    #region Inspector
    [Header("Base UI")]
    [SerializeField] private Button baseInventoryButton;
    [SerializeField] private Button baseSolutionButton;

    [Header("Inventory UI")]
    [SerializeField] private Canvas inventoryCanvas;
    //[SerializeField] private InventoryUIController inventoryUIController;

    [Header("Solution UI")]
    [SerializeField] private Canvas solutionCanvas;
    #endregion

    #region Variables
    #endregion

    #region Properties
    #endregion

    #region Unity methods
    void Awake()
    {
        SetupUI();
    }
    #endregion
    
    #region Helper methods
    private void SetupUI()
    {
        // Disable the canvases (just to be sure)
        inventoryCanvas.enabled = false;
        solutionCanvas.enabled = false;

        baseSolutionButton.onClick.AddListener(() =>
        {
            inventoryCanvas.enabled = false;
            solutionCanvas.enabled = true;
        });
    
        baseInventoryButton.onClick.AddListener(() =>
        {
            solutionCanvas.enabled = false;
            inventoryCanvas.enabled = true;
        });  
    }
    #endregion
}
