using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingCircle : MonoBehaviour
{
    #region Inspector
    #endregion
    
    #region Variables
    private RectTransform rectComponent;
    private float rotateSpeed = 200f;
    #endregion
    
    #region Properties
    #endregion
    
    #region Unity methods

    void Awake()
    {
        rectComponent = GetComponent<RectTransform>();
    }
   
    void Update()
    {
        rectComponent.Rotate(0f, 0f, rotateSpeed * Time.deltaTime);
    }
    
    #endregion
    
    #region Helper methods
    #endregion
}
