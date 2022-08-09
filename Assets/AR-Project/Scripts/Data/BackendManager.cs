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
    private int numberOfPOIs;
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
        UnityEngine.Rendering.DebugManager.instance.enableRuntimeUI = false;

        timerController = GetComponent<TimerController>();

        numberOfPOIs = pointsOfInterestSO.Points.Count;
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

    void OnDisable()
    {
        uiEventsChannelSO.OnStartGameEventRaised -= HandleStartGameEvent;
        uiEventsChannelSO.OnHintRequestedEventRaised -= HandleHintRequest;
        uiEventsChannelSO.OnSolutionItemSelectedEventRaised -= HandleSolutionItemSelection;
        uiEventsChannelSO.OnSolutionGivenEventRaised -= HandleSolutionGiven;
        
        arEventChannelSO.OnPOIDetected -= HandlePOIDetected;
    }
    #endregion

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
        
    #region Helper methods
    private void RemoveItemFromList(List<PointOfInterest> pois, int totalPois)
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
    private void HandleStartGameEvent()
    {
        // Start the timer
        timerController.StartTimer();
    }
    
    private void HandlePOIDetected(string imageName)
    {
        // For all the POIs of the session
        for (int i = 0; i < numberOfPOIs; i ++)
        {
            // If the POI was already detected, skip it
            if (pointsOfInterestSO.Points[i].alreadyDetected)
                return;

            // If the image name in inside the key/value pair imageName/url
            if (pointsOfInterestSO.Points[i].imageNameAndUrl.ContainsKey(imageName))
            {
                // Set this POI as detected
                pointsOfInterestSO.Points[i].alreadyDetected = true;

                // Check the type of the POI and add the POI to the respective list
                switch (pointsOfInterestSO.Points[i].type)
                {
                    case EPOIType.Where:
                        {
                            pointsOfInterestSO.WherePois.Add(pointsOfInterestSO.Points[i]);
                        }
                        break;
                    case EPOIType.When:
                        {
                            pointsOfInterestSO.WhenPois.Add(pointsOfInterestSO.Points[i]);
                        }
                        break;
                    case EPOIType.How:
                        {
                            pointsOfInterestSO.HowPois.Add(pointsOfInterestSO.Points[i]);
                        }
                        break;
                }

                // Raise an event informing that a POI was found
                uiEventsChannelSO.RaiseOnPOIFoundEvent(pointsOfInterestSO.Points[i]);
                
                // Change the game state to POIPopUP
                gameStateSO.UpdateGameState(GameState.POIPopUp);

                return;
            }
        }
    }

    private void HandleHintRequest()
    {
        int totalWherePois = pointsOfInterestSO.WherePois.Count;
        int totalWhenPois = pointsOfInterestSO.WhenPois.Count;
        int totalHowPois = pointsOfInterestSO.HowPois.Count;

        RemoveItemFromList(pointsOfInterestSO.WherePois, totalWherePois);
        RemoveItemFromList(pointsOfInterestSO.WhenPois, totalWhenPois);
        RemoveItemFromList(pointsOfInterestSO.HowPois, totalHowPois);
    }
   
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
    private void PopulateInventory()
    {
        pointsOfInterestSO.WherePois.Clear();
        pointsOfInterestSO.WhenPois.Clear();
        pointsOfInterestSO.HowPois.Clear();

        pointsOfInterestSO.ImageNameAndPOI_Dict.Clear();

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
