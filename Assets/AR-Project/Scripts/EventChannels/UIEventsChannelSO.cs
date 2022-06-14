using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/UI Events Channel")]
public class UIEventsChannelSO : ScriptableObject
{
	public Action OnSessionDataLoadedEventRaised;
	public Action<PointOfInterest> OnClueFoundNotificationEventRaised;
	

	public void RaiseSessionDataLoadedEvent()
	{
		if (OnSessionDataLoadedEventRaised != null)
		{
			OnSessionDataLoadedEventRaised.Invoke();
		}
	}

	public void RaiseClueFoundNotificationEvent(PointOfInterest poi)
	{
		if (OnClueFoundNotificationEventRaised != null)
		{
			OnClueFoundNotificationEventRaised.Invoke(poi);
		}
	}
}
