using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUIController : MonoBehaviour
{
    #region Inspector
    [Header("SO References")]
    [SerializeField] private GameStateSO gameStateSO;

    [Header("Base UI")]
    [SerializeField] private Button baseInventoryButton;
    [SerializeField] private Button baseSolutionButton;

    [Header("Inventory UI")]
    [SerializeField] private Canvas inventoryCanvas;

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

        baseInventoryButton.onClick.AddListener(() =>
        {
            gameStateSO.UpdateGameState(GameState.UI);

            if (!inventoryCanvas.enabled)
            {
                solutionCanvas.enabled = false;
                inventoryCanvas.enabled = true;               
            }   
        });  

        baseSolutionButton.onClick.AddListener(() =>
        {
            gameStateSO.UpdateGameState(GameState.UI);

            if (!solutionCanvas.enabled)
            {
                inventoryCanvas.enabled = false;
                solutionCanvas.enabled = true;
            }  
        });       
    }
    #endregion
}
