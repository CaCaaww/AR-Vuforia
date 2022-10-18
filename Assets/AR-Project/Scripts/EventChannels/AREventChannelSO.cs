using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/AR Event Channel")]
public class AREventChannelSO : ScriptableObject
{
    #region Public actions
    /// <summary>
    /// Public action for when an image is recognized
    /// </summary>
	public Action<string> OnPOIDetected;
    /// <summary>
    /// Public action for when an image is recognized
    /// </summary>
	public Action OnFinishedCreatingImageTargets;
    #endregion

    #region Raiser methods
    /// <summary>
    /// Raise an event when an image is recognized
    /// </summary>
	public void RaisePOIDetectionEvent(string imageName)
	{
		if (OnPOIDetected != null)
        {
			OnPOIDetected.Invoke(imageName);
		}
    }

    /// <summary>
    /// Raise an event when an image is recognized
    /// </summary>
	public void RaiseImageTargetsCreationFinishedEvent()
	{
		if (OnFinishedCreatingImageTargets != null)
        {
            OnFinishedCreatingImageTargets.Invoke();
		}
    }
    #endregion
}
