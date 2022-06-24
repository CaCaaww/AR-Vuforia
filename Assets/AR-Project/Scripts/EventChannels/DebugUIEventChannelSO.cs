using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/DebugUI Event Channel")]
public class DebugUIEventChannelSO : ScriptableObject
{
	public Action<string> OnDebugEventRaised;

	public void RaiseDebugEvent(string text)
	{
		if (OnDebugEventRaised != null)
        {
			OnDebugEventRaised.Invoke(text);
			Debug.Log("[DEBUG]Event Raised");
		}		
	}

	public Action<string> OnDebugEventRaisedGamesState;

	public void RaiseDebugEventGameState(string text)
	{
		if (OnDebugEventRaisedGamesState != null)
        {
			OnDebugEventRaisedGamesState.Invoke(text);
		}		
	}
}
