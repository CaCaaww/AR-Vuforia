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

    [Header("Hint Removal")]
    /// <summary>
    /// Removal percentage (in decimal format) for every time an hint is used by the player
    /// The value should be between 0.01f and 0.99f with a step of 0.01f
    /// </summary>
    [Tooltip("Removal percentage(in decimal format) for every time an hint is used by the player. The value should be between 0.01f and 0.99f with a step of 0.01f")]
    [SerializeField] private float removalPercentage;
    #endregion

    #region Private Variables
    private TimerController timerController;
    //private int numberOfPOIs;
    private List<PointOfInterest> tempPOIsList = new ();
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

        //numberOfPOIs = pointsOfInterestSO.Points.Count;

        gameStateSO.ResetToState(GameState.Loading);
    }

    void OnEnable()
    {
        uiEventsChannelSO.OnStartGameEventRaised += HandleStartGameEvent;
        uiEventsChannelSO.OnHintRequestedEventRaised += HandleHintRequest;
        uiEventsChannelSO.OnSolutionItemSelectedEventRaised += HandleSolutionItemSelection;
        uiEventsChannelSO.OnSolutionItemDeselectedEventRaised += HandleSolutionItemDeselection;
        uiEventsChannelSO.OnSolutionGivenEventRaised += HandleSolutionGiven;

        arEventChannelSO.OnPOIDetected += HandlePOIDetected;      
    }

    void Start()
    {
        timerController = GetComponent<TimerController>();

        #if UNITY_EDITOR
        // Reset the variables in the POIs SO
        //pointsOfInterestSO.ResetVariables(sessionDataSO.ResumeSession);
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
        uiEventsChannelSO.OnSolutionItemDeselectedEventRaised -= HandleSolutionItemDeselection;
        uiEventsChannelSO.OnSolutionGivenEventRaised -= HandleSolutionGiven;
        
        arEventChannelSO.OnPOIDetected -= HandlePOIDetected;
    }
    #endregion

    #region Helper methods
    /// <summary>
    /// Remove a random unuseful POI from a list and return it's id
    /// </summary>
    /// <param name="pois">The list from where remove the POI</param>
    /// <param name="totalPois">The total number of POIs in the list</param>
    private int RemoveUnusefulPOI(List<PointOfInterest> pois, int totalPois)
    {
        // Clear the temp list (just to be sure)
        tempPOIsList.Clear();

        // Add to the temp list only the POIs that ARE NOT useful for the solution
        for (int i = 0; i < totalPois; i++) 
        {
            if (!pois[i].isUseful) 
            {
                tempPOIsList.Add(pois[i]);
                //Debug.Log
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

            // Save the POI id
            int deletedPOIId = tempPOIsList[randomIndex].id;

            // Remove the POI
            pois.Remove(tempPOIsList[randomIndex]);

            // Return the POI id;
            return deletedPOIId;
        }
        else
        {
            Debug.Log("No unuseful POI found");

            // return 0
            return 0;
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
        // Check the type and add the POI to the respective (POI found) list
        switch (pointsOfInterestSO.ImageNameAndPOI[imageName].type)
        {
            case EPOIType.Where:
                {
                    pointsOfInterestSO.WherePOIsFound.Add(pointsOfInterestSO.ImageNameAndPOI[imageName]);
                }
                break;
            case EPOIType.When:
                {
                    pointsOfInterestSO.WhenPOIsFound.Add(pointsOfInterestSO.ImageNameAndPOI[imageName]);
                }
                break;
            case EPOIType.How:
                {
                    pointsOfInterestSO.HowPOIsFound.Add(pointsOfInterestSO.ImageNameAndPOI[imageName]);
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
        // Debug for counting
        /*Debug.Log("Where: " + pointsOfInterestSO.WherePOIsFound.Count);
        Debug.Log("When: " + pointsOfInterestSO.WhenPOIsFound.Count);
        Debug.Log("How: " + pointsOfInterestSO.HowPOIsFound.Count);*/

        // Figures out how many POIs to remove from each type
        int totalPOIs = pointsOfInterestSO.WherePOIsFound.Count + pointsOfInterestSO.WhenPOIsFound.Count + pointsOfInterestSO.HowPOIsFound.Count;
        Debug.Log("POIs Found before hint: " + totalPOIs);
        int totalPoiRemoved = Mathf.FloorToInt(totalPOIs * removalPercentage);
        Debug.Log("totalPoiRemoved: " + totalPoiRemoved);
        int totalPoiRemovedPerType = Mathf.FloorToInt(totalPoiRemoved / 3);
        Debug.Log("totalPoiRemovedPerType: " + totalPoiRemovedPerType);
        // Base case when we have less than the number of removal
        if (totalPoiRemovedPerType <= 0)
		{
            totalPoiRemovedPerType = 1;
		}

        // Loops a remove. If none can be removed it continues.
        for (int i = 0; i < totalPoiRemovedPerType; i++) {
            // Remove a single random item (not part of the solution) for every type
            int wherePoiId = RemoveUnusefulPOI(pointsOfInterestSO.WherePOIsFound, pointsOfInterestSO.WherePOIsFound.Count);
            int whenPoiId = RemoveUnusefulPOI(pointsOfInterestSO.WhenPOIsFound, pointsOfInterestSO.WhenPOIsFound.Count);
            int howPoiId = RemoveUnusefulPOI(pointsOfInterestSO.HowPOIsFound, pointsOfInterestSO.HowPOIsFound.Count);

            // Raise an event informing which POIs where deleted by the hint
            //uiEventsChannelSO.RaisePOIDeletedByHintEvent(wherePoiId, whenPoiId, howPoiId);
        }

        totalPOIs = pointsOfInterestSO.WherePOIsFound.Count + pointsOfInterestSO.WhenPOIsFound.Count + pointsOfInterestSO.HowPOIsFound.Count;
        Debug.Log("POIs Found after hint: " + totalPOIs);
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
                    pointsOfInterestSO.WherePOIChosenAsSolutionId = controller.POI.id;
                }
                break;
            case EPOIType.When:
                {
                    pointsOfInterestSO.WhenPOIChosenAsSolutionId = controller.POI.id;
                }
                break;
            case EPOIType.How:
                {
                    pointsOfInterestSO.HowPOIChosenAsSolutionId = controller.POI.id;
                }
                break;
        }
    }

    /// <summary>
    /// Handle the deselection of an item from part of the solution
    /// </summary>
    /// <param name="controller">The controller for the solution item</param>
    private void HandleSolutionItemDeselection(SolutionItemController controller)
    {
        switch (controller.POI.type)
        {
            case EPOIType.Where:
                {
                    pointsOfInterestSO.WherePOIChosenAsSolutionId = 0;
                }
                break;
            case EPOIType.When:
                {
                    pointsOfInterestSO.WhenPOIChosenAsSolutionId = 0;
                }
                break;
            case EPOIType.How:
                {
                    pointsOfInterestSO.HowPOIChosenAsSolutionId = 0;
                }
                break;
        }
    }
    
    /// <summary>
    /// Handle what happens when the solution is given by the player
    /// </summary>
    private void HandleSolutionGiven() 
    {
        if ((pointsOfInterestSO.WherePOIChosenAsSolutionId == pointsOfInterestSO.WherePOISolutionId) &&
            (pointsOfInterestSO.WhenPOIChosenAsSolutionId == pointsOfInterestSO.WhenPOISolutionId) &&
            (pointsOfInterestSO.HowPOIChosenAsSolutionId == pointsOfInterestSO.HowPOISolutionId))
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
}
