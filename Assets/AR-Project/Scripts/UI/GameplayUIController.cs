using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUIController : MonoBehaviour
{
    #region Inspector
    [Header("Base Screen")]
    [SerializeField] private GameObject baseScreen;
    [SerializeField] private Button baseInventoryButton;
    [SerializeField] private Button baseSolutionButton;

    [Header("Inventory Screen")]
    [SerializeField] private GameObject inventoryScreen;
    [SerializeField] private InventoryScreenController inventoryScreenController;

    [Header("Solution Screen")]
    [SerializeField] private GameObject solutionScreen;
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

    void Start()
    {
        
    }
    
    void Update()
    {
        
    }
    
    #endregion
    
    #region Helper methods
    private void SetupUI()
    {
        baseScreen.SetActive(true);

        baseSolutionButton.onClick.AddListener(() =>
        {
            inventoryScreen.SetActive(false);
            solutionScreen.SetActive(true);            
        });
    
        baseInventoryButton.onClick.AddListener(() =>
        {
            solutionScreen.SetActive(false);
            inventoryScreen.SetActive(true);
        });

        inventoryScreen.SetActive(false);
    }

    #endregion
}
