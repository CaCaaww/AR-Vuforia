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
    private int numberOfPOIs;
    private List<PointOfInterest> tempPOIsList = new List<PointOfInterest>();
    #endregion

    #region Properties
    #endregion

    #region Unity methods
    void Awake()
    {
        numberOfPOIs = pointsOfInterestSO.Points.Count;
        gameStateSO.UpdateGameState(GameState.Loading);
    }

    void OnEnable()
    {
        arEventChannelSO.OnPOIDetected += HandlePOIDetected;
        uiEventsChannelSO.OnHintRequestedEventRaised += HandleHintRequest;
        uiEventsChannelSO.OnSolutionItemSelectedEventRaised += HandleSolutionItemSelection;
        uiEventsChannelSO.OnSolutionGivenEventRaised += HandleSolutionGiven;
    }

    void OnDisable()
    {
        arEventChannelSO.OnPOIDetected -= HandlePOIDetected;
        uiEventsChannelSO.OnHintRequestedEventRaised -= HandleHintRequest;
        uiEventsChannelSO.OnSolutionItemSelectedEventRaised -= HandleSolutionItemSelection;
        uiEventsChannelSO.OnSolutionGivenEventRaised -= HandleSolutionGiven;
    }
    #endregion

    void Start()
    {
        #if UNITY_EDITOR
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

            pointsOfInterestSO.AddToImageNameAndPOI_Dict(poi.imageName, poi);
        }
        #endif

        #if UNITY_ANDROID
        foreach (var poi in pointsOfInterestSO.Points)
        {
            pointsOfInterestSO.AddToImageNameAndPOI_Dict(poi.imageName, poi);
        }
        #endif
    }
        
    #region Helper methods
    private void RemoveItemFromList(List<PointOfInterest> pois, int totalPois)
    {
        // Clear the temp list (just to be sure)
        tempPOIsList.Clear();

        for (int i = 0; i < totalPois; i++) 
        {
            if (!pois[i].isUseful) 
            {
                tempPOIsList.Add(pois[i]);
            }
        }

        if (tempPOIsList.Count > 0)
        {
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
            Debug.Log("No POI found");
        }
    }
    #endregion

    #region Callbacks
    private void HandlePOIDetected(string imageName)
    {
        for (int i = 0; i < numberOfPOIs; i ++)
        {
            if (pointsOfInterestSO.Points[i].imageName == imageName)
            {
                pointsOfInterestSO.Points[i].alreadyDetected = true;

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

                uiEventsChannelSO.RaiseOnPOIFoundEvent(pointsOfInterestSO.Points[i]);
                
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
            uiEventsChannelSO.RaiseEndgameReachedEvent(true, sessionDataSO.VictoryText);
        }
        else
        {
            // Defeat
            Debug.Log("Nope, the answer is not correct, game over");
            uiEventsChannelSO.RaiseEndgameReachedEvent(false, sessionDataSO.DefeatText);
        }
    }
    #endregion
}
