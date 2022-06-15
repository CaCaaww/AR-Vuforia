using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    #region Inspector
    [Header("LISTEN Channels")]
    [SerializeField]
    private AREventChannelSO aREventChannelSO;

    [Header("SEND Channels")]
    [SerializeField]
    private UIEventsChannelSO uIEventsChannelSO;

    [Header("SO References")]
    [SerializeField]
    private GameStateSO gameStateSO;
    [SerializeField]
    private PointsOfInterestSO pointsOfInterestSO;
    #endregion

    #region Private Variables
    private int numberOfPOIs;
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
        aREventChannelSO.OnPOIDetected += HandlePOIDetected;
    }

    void OnDisable()
    {
        aREventChannelSO.OnPOIDetected -= HandlePOIDetected;
    }
    #endregion
    
    #region Helper methods
    #endregion

    #region Callbacks
    private void HandlePOIDetected(string imageName)
    {
        for (int i = 0; i < numberOfPOIs; i ++)
        {
            if (pointsOfInterestSO.Points[i].imageName == imageName)
            {
                switch (pointsOfInterestSO.Points[i].clueType)
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

                uIEventsChannelSO.OnPOIFoundEventRaised(pointsOfInterestSO.Points[i]);
                
                gameStateSO.UpdateGameState(GameState.POIPopUp);

                return;
            }
        }
    }
    #endregion
}
