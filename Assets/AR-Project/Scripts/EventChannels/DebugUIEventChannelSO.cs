using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/DebugUI Event Channel")]
public class DebugUIEventChannelSO : ScriptableObject
{
	public Action OnDebugEventRaised;

	public void RaiseDebugEvent()
	{
		if (OnDebugEventRaised != null)
        {
			OnDebugEventRaised.Invoke();
			Debug.Log("Event Raised");
		}
			
	}
}
