using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/UI Events Channel")]
public class UIEventsChannelSO : ScriptableObject
{
	public Action<string> OnClueFoundNotificationEventRaised;

	public void RaiseClueFoundNotificationEvent(string imageName)
	{
		if (OnClueFoundNotificationEventRaised != null)
		{
			OnClueFoundNotificationEventRaised.Invoke(imageName);
			//Debug.Log("Event Raised");
		}

	}
}
