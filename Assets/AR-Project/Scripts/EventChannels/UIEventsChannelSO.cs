using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/UI Events Channel")]
public class UIEventsChannelSO : ScriptableObject
{
	public Action OnSessionDataLoadedEventRaised;
	public Action<PointOfInterest> OnPOIFoundEventRaised;
    public Action<PointOfInterest> OnPOIViewEventRaised;


    public void RaiseSessionDataLoadedEvent()
	{
		if (OnSessionDataLoadedEventRaised != null)
		{
			OnSessionDataLoadedEventRaised.Invoke();
		}
	}

	public void RaiseOnPOIFoundEvent(PointOfInterest poi)
	{
		if (OnPOIFoundEventRaised != null)
		{
			OnPOIFoundEventRaised.Invoke(poi);
		}
	}

	public void RaiseOnPOIViewEvent(PointOfInterest poi)
	{
		if (OnPOIViewEventRaised != null)
		{
			OnPOIViewEventRaised.Invoke(poi);
		}
	}
}
