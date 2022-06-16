using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    #region Inspector
    [Header("LISTEN/SEND Channels")]
    [SerializeField]
    private UIEventsChannelSO uiEventsChannelSO;

    [Header("LISTEN Channels")]
    [SerializeField]
    private AREventChannelSO arEventChannelSO;
    
    [Header("SO References")]
    [SerializeField]
    private GameStateSO gameStateSO;
    [SerializeField]
    private PointsOfInterestSO pointsOfInterestSO;
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
    }

    void OnEnable()
    {
        arEventChannelSO.OnPOIDetected += HandlePOIDetected;
        uiEventsChannelSO.OnHintRequestedEventRaised += HandleHintRequest;

    }

    void OnDisable()
    {
        arEventChannelSO.OnPOIDetected -= HandlePOIDetected;
        uiEventsChannelSO.OnHintRequestedEventRaised -= HandleHintRequest;
    }
    #endregion

    void Start()
    {
        #if UNITY_EDITOR
        pointsOfInterestSO.WherePois.Clear();
        pointsOfInterestSO.WhenPois.Clear();
        pointsOfInterestSO.HowPois.Clear();

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
        #endif
    }
        
    #region Helper methods
    #endregion

    #region Callbacks
    private void HandlePOIDetected(string imageName)
    {
        for (int i = 0; i < numberOfPOIs; i ++)
        {
            if (pointsOfInterestSO.Points[i].imageName == imageName)
            {
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

                uiEventsChannelSO.OnPOIFoundEventRaised(pointsOfInterestSO.Points[i]);
                
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

        int randomIndex;
        int i;

        // Remove from the _where_ list
        // If we have any POIs in the _where_ list
        if (totalWherePois > 0) 
        {
            // Clear the temp list (just to be sure)
            tempPOIsList.Clear();

            for (i = 0; i < totalWherePois; i++) 
            {
                if (!pointsOfInterestSO.WherePois[i].isUseful) 
                {
                    tempPOIsList.Add(pointsOfInterestSO.WherePois[i]);
                }
            }

            if (tempPOIsList.Count > 0)
            {
                randomIndex = UnityEngine.Random.Range(0, tempPOIsList.Count);

                Debug.Log("Where index: " + randomIndex);

                Debug.Log("Removed _where_ POI: " + tempPOIsList[randomIndex].title);

                // Send the poi through the event channel for the ui
                uiEventsChannelSO.OnPOIRemovedEventRaised(tempPOIsList[randomIndex]);

                // Remove the where POI
                pointsOfInterestSO.WherePois.Remove(tempPOIsList[randomIndex]);
            }
            else
            {
                Debug.Log("No When POI found");
            }
            
        }

        // Remove from the _when_ list
        // If we have any POIs in the _when_ list
        if (totalWhenPois > 0)
        {
            // Clear the temp list (just to be sure)
            tempPOIsList.Clear();

            for (i = 0; i < totalWhenPois; i++) 
            {
                if (!pointsOfInterestSO.WhenPois[i].isUseful) 
                {
                    tempPOIsList.Add(pointsOfInterestSO.WhenPois[i]);
                }
            }

            randomIndex = UnityEngine.Random.Range(0, tempPOIsList.Count);

            Debug.Log("When index: " + randomIndex);

            Debug.Log("Removed _when_ POI: " + tempPOIsList[randomIndex].title);

            // Send the poi through the event channel for the ui
            uiEventsChannelSO.OnPOIRemovedEventRaised(tempPOIsList[randomIndex]);

            // Remove the when POI
            pointsOfInterestSO.WhenPois.Remove(tempPOIsList[randomIndex]);
        }

        // Remove from the _how_ list
        // If we have any POIs in the _how_ list
        if (totalHowPois > 0)
        {
            // Clear the temp list (just to be sure)
            tempPOIsList.Clear();

            for (i = 0; i < totalHowPois; i++) 
            {
                if (!pointsOfInterestSO.HowPois[i].isUseful) 
                {
                    tempPOIsList.Add(pointsOfInterestSO.HowPois[i]);
                }
            }

            randomIndex = UnityEngine.Random.Range(0, tempPOIsList.Count);

            Debug.Log("How index: " + randomIndex);

            Debug.Log("Removed _how_ POI: " + tempPOIsList[randomIndex].title);

            // Send the poi through the event channel for the ui
            uiEventsChannelSO.OnPOIRemovedEventRaised(tempPOIsList[randomIndex]);
            
            // Remove the where POI
            pointsOfInterestSO.HowPois.Remove(tempPOIsList[randomIndex]);
        }
    }
    #endregion
}
