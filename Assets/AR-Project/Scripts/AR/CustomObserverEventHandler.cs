using UnityEngine;
using Vuforia;

/// <summary>
/// A custom handler that implements the ITrackableEventHandler interface.
///
/// Changes made to this file could be overwritten when upgrading the Vuforia version.
/// When implementing custom event handler behavior, consider inheriting from this class instead.
/// </summary>
public class CustomObserverEventHandler : MonoBehaviour
{
    public enum TrackingStatusFilter
    {
        Tracked,
        Tracked_ExtendedTracked,
        Tracked_ExtendedTracked_Limited
    }

    /// <summary>
    /// A filter that can be set to either:
    /// - Only consider a target if it's in view (TRACKED)
    /// - Also consider the target if's outside of the view, but the environment is tracked (EXTENDED_TRACKED)
    /// - Even consider the target if tracking is in LIMITED mode, e.g. the environment is just 3dof tracked.
    /// </summary>
    public TrackingStatusFilter StatusFilter = TrackingStatusFilter.Tracked;
   
    protected ObserverBehaviour mObserverBehaviour;
    protected TargetStatus mPreviousTargetStatus = TargetStatus.NotObserved;
    protected bool mCallbackReceivedOnce;

    protected string imageName;

    protected AREventChannelSO arEventChannelSO;
    protected UIEventsChannelSO uiEventsChannelSO;

    public string ImageName { get => imageName; set => imageName = value; }
    public AREventChannelSO AREventChannelSO { get => arEventChannelSO; set => arEventChannelSO = value; }
    public UIEventsChannelSO UIEventChannelSO { get => uiEventsChannelSO; set => uiEventsChannelSO = value; }

    protected void OnDisable()
    {
        uiEventsChannelSO.OnOpeningUIEventRaised -= DisableObserver;
        uiEventsChannelSO.OnClosingUIEventRaised -= EnableObserver;
        arEventChannelSO.OnPOIDetected -= DisableObserver;
        uiEventsChannelSO.OnStartGameEventRaised -= EnableObserver;
    }

    protected virtual void Start()
    {
        uiEventsChannelSO.OnOpeningUIEventRaised += DisableObserver;
        uiEventsChannelSO.OnClosingUIEventRaised += EnableObserver;
        arEventChannelSO.OnPOIDetected += DisableObserver;
        uiEventsChannelSO.OnStartGameEventRaised += EnableObserver;

        mObserverBehaviour = GetComponent<ObserverBehaviour>();
        
        if (mObserverBehaviour)
        {
            mObserverBehaviour.OnTargetStatusChanged += OnObserverStatusChanged;
            mObserverBehaviour.OnBehaviourDestroyed += OnObserverDestroyed;

            OnObserverStatusChanged(mObserverBehaviour, mObserverBehaviour.TargetStatus);
        }

        mObserverBehaviour.enabled = false;
    }

    private void DisableObserver()
    {
        mObserverBehaviour.enabled = false;
    }

    private void EnableObserver()
    {
        mObserverBehaviour.enabled = true;
    }

    private void DisableObserver(string s)
    {
        mObserverBehaviour.enabled = false;
    }

    protected virtual void OnDestroy()
    {
        if (mObserverBehaviour)
            OnObserverDestroyed(mObserverBehaviour);
    }

    void OnObserverDestroyed(ObserverBehaviour observer)
    {
        mObserverBehaviour.OnTargetStatusChanged -= OnObserverStatusChanged;
        mObserverBehaviour.OnBehaviourDestroyed -= OnObserverDestroyed;

        mObserverBehaviour = null;
    }

    void OnObserverStatusChanged(ObserverBehaviour behaviour, TargetStatus targetStatus)
    {
        var name = mObserverBehaviour.TargetName;

        Debug.Log($"Target status: {name} {targetStatus.Status} -- {targetStatus.StatusInfo}");

        HandleTargetStatusChanged(mPreviousTargetStatus.Status, targetStatus.Status);
        HandleTargetStatusInfoChanged(targetStatus.StatusInfo);
        
        mPreviousTargetStatus = targetStatus;
    }

    protected virtual void HandleTargetStatusChanged(Status previousStatus, Status newStatus)
    {
        var shouldBeRendererBefore = ShouldBeRendered(previousStatus);
        var shouldBeRendererNow = ShouldBeRendered(newStatus);
        if (shouldBeRendererBefore != shouldBeRendererNow)
        {
            if (shouldBeRendererNow)
            {
                OnTrackingFound();
            }
            else
            {
                OnTrackingLost();
            }
        }
        else
        {
            if (!mCallbackReceivedOnce && !shouldBeRendererNow)
            {
                // This is the first time we are receiving this callback, and the target is not visible yet.
                // --> Hide the augmentation.
                OnTrackingLost();
            }
        }

        mCallbackReceivedOnce = true;
    }

    protected virtual void HandleTargetStatusInfoChanged(StatusInfo newStatusInfo)
    {
        if (newStatusInfo == StatusInfo.WRONG_SCALE)
        {
            Debug.LogErrorFormat("The target {0} appears to be scaled incorrectly. " +
                                 "This might result in tracking issues. " +
                                 "Please make sure that the target size corresponds to the size of the " +
                                 "physical object in meters and regenerate the target or set the correct " +
                                 "size in the target's inspector.", mObserverBehaviour.TargetName);
        }
    }

    protected bool ShouldBeRendered(Status status)
    {
        if (status == Status.TRACKED)
        {
            // always render the augmentation when status is TRACKED, regardless of filter
            return true;
        }

        if (StatusFilter == TrackingStatusFilter.Tracked_ExtendedTracked && status == Status.EXTENDED_TRACKED)
        {
            // also return true if the target is extended tracked
            return true;
        }

        if (StatusFilter == TrackingStatusFilter.Tracked_ExtendedTracked_Limited &&
            (status == Status.EXTENDED_TRACKED || status == Status.LIMITED))
        {
            // in this mode, render the augmentation even if the target's tracking status is LIMITED.
            // this is mainly recommended for Anchors.
            return true;
        }

        return false;
    }

    public virtual void OnTrackingFound()
    {
        arEventChannelSO.RaisePOIDetectionEvent(imageName);
    }

    public virtual void OnTrackingLost() {}  
}