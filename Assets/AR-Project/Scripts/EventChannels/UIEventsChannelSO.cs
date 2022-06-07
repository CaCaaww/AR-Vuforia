using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/UI Events Channel")]
public class UIEventsChannelSO : ScriptableObject
{
	public Action OnSessionDataLoadedEventRaised;
	public Action<string> OnClueFoundNotificationEventRaised;
	

	public void RaiseSessionDataLoadedEvent()
	{
		if (OnSessionDataLoadedEventRaised != null)
		{
			OnSessionDataLoadedEventRaised.Invoke();
		}
	}

	public void RaiseClueFoundNotificationEvent(string imageName)
	{
		if (OnClueFoundNotificationEventRaised != null)
		{
			OnClueFoundNotificationEventRaised.Invoke(imageName);
		}
	}
}
