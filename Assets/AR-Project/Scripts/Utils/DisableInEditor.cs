using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class DisableInEditor : MonoBehaviour
{
    #region Unity methods

    void Awake()
    {
        #if UNITY_EDITOR
        gameObject.SetActive(false);
        #endif
    }
    #endregion
}
