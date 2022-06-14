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

    void Start()
    {
        
    }
    
    void Update()
    {
        
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
                    case EClueType.Where:
                        {
                            pointsOfInterestSO.WherePois.Add(pointsOfInterestSO.Points[i]);
                        }
                        break;
                    case EClueType.When:
                        {
                            pointsOfInterestSO.WhenPois.Add(pointsOfInterestSO.Points[i]);
                        }
                        break;
                    case EClueType.How:
                        {
                            pointsOfInterestSO.HowPois.Add(pointsOfInterestSO.Points[i]);
                        }
                        break;
                }

                uIEventsChannelSO.OnClueFoundNotificationEventRaised(pointsOfInterestSO.Points[i]);
                return;
            }
        }
    }
    #endregion
}
