using UnityEngine;
using UnityEngine.UI;

public class GameplayUIController : MonoBehaviour
{
    #region Inspector
    [Header("SEND Event Channels")]
    [SerializeField] private UIEventsChannelSO uiEventsChannelSO;

    [Header("SO References")]
    [SerializeField] private GameStateSO gameStateSO;

    [Header("Base UI")]
    [SerializeField] private Button baseInventoryButton;
    [SerializeField] private Button baseSolutionButton;

    [Header("Inventory UI")]
    [SerializeField] private Canvas inventoryCanvas;

    [Header("Solution UI")]
    [SerializeField] private Canvas solutionCanvas;

    [Header("Endgame UI")]
    [SerializeField] private Canvas endgameCanvas;
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
        endgameCanvas.enabled = false;

        baseInventoryButton.onClick.AddListener(() =>
        {
            if (!inventoryCanvas.enabled)
            {
                gameStateSO.UpdateGameState(GameState.UI);

                solutionCanvas.enabled = false;
                inventoryCanvas.enabled = true;

                uiEventsChannelSO.RaiseOpeningUIEvent();                       
            }   
        });  

        baseSolutionButton.onClick.AddListener(() =>
        {
            if (!solutionCanvas.enabled)
            {
                gameStateSO.UpdateGameState(GameState.UI);

                inventoryCanvas.enabled = false;
                solutionCanvas.enabled = true;

                uiEventsChannelSO.RaiseOpeningUIEvent();  
            }  
        });       
    }
    #endregion
}
