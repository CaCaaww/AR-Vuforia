using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryScreenController : MonoBehaviour
{
    #region Inspector
    [Header("Where References")]
    [SerializeField] private GameObject wherePanel;
    [SerializeField] private Button whereButton;
    

    [Header("When References")]
    [SerializeField] private GameObject whenPanel;
    [SerializeField] private Button whenButton;
    

    [Header("How References")]
    [SerializeField] private GameObject howPanel;
    [SerializeField] private Button howButton;
    #endregion

    #region Variables
    #endregion

    #region Properties
    #endregion

    #region Unity methods
    void Awake()
    {
        whereButton.onClick.AddListener(WhereButtonBehaviour);
        whenButton.onClick.AddListener(WhenButtonBehaviour);
        howButton.onClick.AddListener(HowButtonBehaviour);
    }

    void Start()
    {
        
    }
    
    void Update()
    {
        
    }
    
    #endregion
    
    #region Helper methods
    private void WhereButtonBehaviour()
    {
        whenPanel.SetActive(false);
        howPanel.SetActive(false);

        wherePanel.SetActive(true);
    }

    private void WhenButtonBehaviour()
    {
        wherePanel.SetActive(false);
        howPanel.SetActive(false);

        whenPanel.SetActive(true);   
    }

    private void HowButtonBehaviour()
    {
        wherePanel.SetActive(false);
        whenPanel.SetActive(false);

        howPanel.SetActive(true);   
    }

    #endregion
}
