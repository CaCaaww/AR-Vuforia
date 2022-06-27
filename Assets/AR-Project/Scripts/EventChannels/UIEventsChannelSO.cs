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
    public Action OnHintRequestedEventRaised;
    public Action<PointOfInterest> OnPOIRemovedEventRaised;
    public Action<SolutionItemController> OnSolutionItemSelectedEventRaised;
    public Action OnSolutionGivenEventRaised;
    public Action<bool, string, TimeSpan> OnEndgameReachedEventRaised;
    public Action OnClosingUIEventRaised;
	public Action OnOpeningUIEventRaised;
    public Action OnStartGameEventRaised;


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

    public void RaiseHintRequestedEvent() 
	{
		if (OnHintRequestedEventRaised != null)
		{
            OnHintRequestedEventRaised.Invoke();
        }
	}

	public void RaisePOIRemovedEvent(PointOfInterest poi) 
	{
		if (OnPOIRemovedEventRaised != null)
		{
            OnPOIRemovedEventRaised.Invoke(poi);
        }
	}

	public void RaiseSolutionItemSelectedEvent(SolutionItemController controller)
	{
		if (OnSolutionItemSelectedEventRaised != null)
		{
            OnSolutionItemSelectedEventRaised.Invoke(controller);
        }
	}

	public void RaiseSolutionGivenEvent()
	{
		if (OnSolutionGivenEventRaised != null)
		{
            OnSolutionGivenEventRaised.Invoke();
        }
	}

	public void RaiseEndgameReachedEvent(bool isVictory, string endgameText, TimeSpan timeSpan)
	{
		if (OnEndgameReachedEventRaised != null)
		{
            OnEndgameReachedEventRaised.Invoke(isVictory, endgameText, timeSpan);

			//Stop the timer
			//EndgameTimerController.instance.EndTimer();
        }
	}

	public void RaiseClosingUIEvent()
	{
		if (OnClosingUIEventRaised != null)
		{
            OnClosingUIEventRaised.Invoke();
        }
	}

	public void RaiseOpeningUIEvent()
	{
		if (OnOpeningUIEventRaised != null)
		{
			OnOpeningUIEventRaised.Invoke();
        }
	}

    public void RaiseStartGameEvent()
	{
		if (OnStartGameEventRaised != null)
		{
			OnStartGameEventRaised.Invoke();
        }
	}
}
