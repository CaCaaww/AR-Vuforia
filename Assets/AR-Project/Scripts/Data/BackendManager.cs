using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackendManager : MonoBehaviour
{
    #region Inspector
    [Header("LISTEN/SEND Channels")]
    [SerializeField] private UIEventsChannelSO uiEventsChannelSO;

    [Header("LISTEN Channels")]
    [SerializeField] private AREventChannelSO arEventChannelSO;
    
    [Header("SO References")]
    [SerializeField] private GameStateSO gameStateSO;
    [SerializeField] private SessionDataSO sessionDataSO;
    [SerializeField] private PointsOfInterestSO pointsOfInterestSO;
    #endregion

    #region Private Variables
    private TimerController timerController;
    //private int numberOfPOIs;
    private List<PointOfInterest> tempPOIsList = new List<PointOfInterest>();
    #endregion

    #region Properties
    #endregion

    #region Unity methods
    void Awake()
    {
        // Disable screen dimming
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Screen.orientation = ScreenOrientation.Portrait;

        // Bypass a bug at runtime
        UnityEngine.Rendering.DebugManager.instance.enableRuntimeUI = false;

        timerController = GetComponent<TimerController>();

        //numberOfPOIs = pointsOfInterestSO.Points.Count;

        gameStateSO.ResetToState(GameState.Loading);
    }

    void OnEnable()
    {
        uiEventsChannelSO.OnStartGameEventRaised += HandleStartGameEvent;
        uiEventsChannelSO.OnHintRequestedEventRaised += HandleHintRequest;
        uiEventsChannelSO.OnSolutionItemSelectedEventRaised += HandleSolutionItemSelection;
        uiEventsChannelSO.OnSolutionGivenEventRaised += HandleSolutionGiven;

        arEventChannelSO.OnPOIDetected += HandlePOIDetected;
        
    }

    void Start()
    {
        #if UNITY_EDITOR
            PopulateInventory();
        #endif

        // Uncomment only in builds with just the 02-AR-Project scene
        /*#if UNITY_ANDROID && !UNITY_EDITOR
        foreach (var poi in pointsOfInterestSO.Points)
        {
            pointsOfInterestSO.AddToImageNameAndPOI_Dict(poi.imageName, poi);
        }
        #endif
        */
    }

    void OnDisable()
    {
        uiEventsChannelSO.OnStartGameEventRaised -= HandleStartGameEvent;
        uiEventsChannelSO.OnHintRequestedEventRaised -= HandleHintRequest;
        uiEventsChannelSO.OnSolutionItemSelectedEventRaised -= HandleSolutionItemSelection;
        uiEventsChannelSO.OnSolutionGivenEventRaised -= HandleSolutionGiven;
        
        arEventChannelSO.OnPOIDetected -= HandlePOIDetected;
    }
    #endregion

    #region Helper methods
    /// <summary>
    /// Remove a random unuseful POI from a list
    /// </summary>
    /// <param name="pois">The list from where remove the POI</param>
    /// <param name="totalPois">The total number of POIs in the list</param>
    private void RemoveUnusefulPOI(List<PointOfInterest> pois, int totalPois)
    {
        // Clear the temp list (just to be sure)
        tempPOIsList.Clear();

        // Add to the temp list only the POIs that ARE NOT useful for the solution
        for (int i = 0; i < totalPois; i++) 
        {
            if (!pois[i].isUseful) 
            {
                tempPOIsList.Add(pois[i]);
            }
        }

        // If the number of POIs not useful is more than zero
        if (tempPOIsList.Count > 0)
        {
            // Get a radom index from the list
            int randomIndex = UnityEngine.Random.Range(0, tempPOIsList.Count);

            Debug.Log(pois[randomIndex].type + " index: " + randomIndex);

            Debug.Log("Removed " + pois[randomIndex].type + " POI: " + tempPOIsList[randomIndex].title);

            // Send the poi through the event channel for the ui
            uiEventsChannelSO.RaisePOIRemovedEvent(tempPOIsList[randomIndex]);

            // Remove the POI
            pois.Remove(tempPOIsList[randomIndex]);
        }
        else
        {
            Debug.Log("No unuseful POI found");
        }
    }
    #endregion

    #region Callbacks
    /// <summary>
    /// Handle the start of the game
    /// </summary>
    private void HandleStartGameEvent()
    {
        // Start the timer
        timerController.StartTimer();
    }
    
    /// <summary>
    /// Handle when a POI is detected
    /// </summary>
    /// <param name="imageName">The name of the image detected</param>
    private void HandlePOIDetected(string imageName)
    {
        // If the POI was already detected, skip it
        if (pointsOfInterestSO.ImageNameAndPOI[imageName].alreadyDetected)
            return;
        
        // Set this POI as detected
        pointsOfInterestSO.ImageNameAndPOI[imageName].alreadyDetected = true;

        // Check the type and add the POI to the respective list
        switch (pointsOfInterestSO.ImageNameAndPOI[imageName].type)
        {
            case EPOIType.Where:
                {
                    pointsOfInterestSO.WherePois.Add(pointsOfInterestSO.ImageNameAndPOI[imageName]);
                }
                break;
            case EPOIType.When:
                {
                    pointsOfInterestSO.WhenPois.Add(pointsOfInterestSO.ImageNameAndPOI[imageName]);
                }
                break;
            case EPOIType.How:
                {
                    pointsOfInterestSO.HowPois.Add(pointsOfInterestSO.ImageNameAndPOI[imageName]);
                }
                break;
        }

        // Raise an event informing that a POI was found
        uiEventsChannelSO.RaiseOnPOIFoundEvent(pointsOfInterestSO.ImageNameAndPOI[imageName]);
        
        // Change the game state to POIPopUP
        gameStateSO.UpdateGameState(GameState.POIPopUp);
    }

    /// <summary>
    /// Handle an hint request
    /// </summary>
    private void HandleHintRequest()
    {
        // Remove a single random item (not part of the solution) for every type
        RemoveUnusefulPOI(pointsOfInterestSO.WherePois, pointsOfInterestSO.WherePois.Count);
        RemoveUnusefulPOI(pointsOfInterestSO.WhenPois, pointsOfInterestSO.WhenPois.Count);
        RemoveUnusefulPOI(pointsOfInterestSO.HowPois, pointsOfInterestSO.HowPois.Count);
    }
   
    /// <summary>
    /// Handle the selection of an item as part of the solution
    /// </summary>
    /// <param name="controller">The controller for the solution item</param>
    private void HandleSolutionItemSelection(SolutionItemController controller)
    {
        switch (controller.POI.type)
        {
            case EPOIType.Where:
                {
                    pointsOfInterestSO.WherePOIChosenAsSolution = controller.POI;
                }
                break;
            case EPOIType.When:
                {
                    pointsOfInterestSO.WhenPOIChosenAsSolution = controller.POI;
                }
                break;
            case EPOIType.How:
                {
                    pointsOfInterestSO.HowPOIChosenAsSolution = controller.POI;
                }
                break;
        }
    }
    
    /// <summary>
    /// Handle what happens when the solution is given by the player
    /// </summary>
    private void HandleSolutionGiven() 
    {
        if (pointsOfInterestSO.WherePOIChosenAsSolution.isUseful &&
            pointsOfInterestSO.WhenPOIChosenAsSolution.isUseful &&
            pointsOfInterestSO.HowPOIChosenAsSolution.isUseful)
        {
            // Victory
            Debug.Log("Victory!");

            timerController.StopTimer();

            uiEventsChannelSO.RaiseEndgameReachedEvent(true, sessionDataSO.VictoryText, timerController.TimePlaying);
        }
        else
        {
            // Defeat
            Debug.Log("Nope, the answer is not correct, game over");
            uiEventsChannelSO.RaiseEndgameReachedEvent(false, sessionDataSO.DefeatText, timerController.TimePlaying);
        }
    }
    #endregion

    #region  Editor-only methods
    /// <summary>
    /// Populate the inventory when using the Unity Editor
    /// </summary>
    private void PopulateInventory()
    {
        // Clear the lists (just to be sure)
        pointsOfInterestSO.WherePois.Clear();
        pointsOfInterestSO.WhenPois.Clear();
        pointsOfInterestSO.HowPois.Clear();

        pointsOfInterestSO.ImageNameAndPOI.Clear();

        foreach (var poi in pointsOfInterestSO.Points)
        {
            switch (poi.type)
                {
                    case EPOIType.Where:
                        {
                            pointsOfInterestSO.WherePois.Add(poi);
                        }
                        break;
                    case EPOIType.When:
                        {
                            pointsOfInterestSO.WhenPois.Add(poi);
                        }
                        break;
                    case EPOIType.How:
                        {
                            pointsOfInterestSO.HowPois.Add(poi);
                        }
                        break;
                }
        }
    }
    #endregion
}
