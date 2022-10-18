using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/UI Events Channel")]
public class UIEventsChannelSO : ScriptableObject
{
	#region Login Actions
 	public Action<bool> OnSessionDataLoadedEventRaised;
    public Action<string, string> OnLoginCredentialsSentEventRaised;
    #endregion

    #region In-game Actions
    public Action<PointOfInterest> OnPOIFoundEventRaised;
    public Action<PointOfInterest> OnPOIViewEventRaised;
    public Action OnHintRequestedEventRaised;
    public Action<int, int, int> OnPOIDeletedByHintEventRaised;
    public Action<PointOfInterest> OnPOIRemovedEventRaised;
    public Action<SolutionItemController> OnSolutionItemSelectedEventRaised;
	public Action<SolutionItemController> OnSolutionItemDeselectedEventRaised;
	public Action OnSolutionGivenEventRaised;
    public Action<bool, string, TimeSpan> OnEndgameReachedEventRaised;
    public Action OnClosingUIEventRaised;
	public Action OnOpeningUIEventRaised;
    public Action OnStartGameEventRaised;
	#endregion

	#region Login raiser methods
	public void RaiseLoginCredentialsSentEvent(string nicknameText, string passwordText)
	{
		if (OnLoginCredentialsSentEventRaised != null)
		{
			OnLoginCredentialsSentEventRaised.Invoke(nicknameText, passwordText);
		}
	}
	#endregion

	#region In-game raiser methods
    public void RaiseSessionDataLoadedEvent(bool success)
	{
		if (OnSessionDataLoadedEventRaised != null)
		{
			OnSessionDataLoadedEventRaised.Invoke(success);
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

	public void RaisePOIDeletedByHintEvent(int wherePoiId, int whenPoiId, int howPoiId) 
	{
		if (OnPOIDeletedByHintEventRaised != null)
		{
            OnPOIDeletedByHintEventRaised.Invoke(whenPoiId, whenPoiId, howPoiId);
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
	public void RaiseSolutionItemDeselectedEvent(SolutionItemController controller)
	{
		if (OnSolutionItemDeselectedEventRaised != null)
		{
			OnSolutionItemDeselectedEventRaised.Invoke(controller);
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
	#endregion
}
