using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class VuforiaManager : MonoBehaviour
{
    #region Inspector
    [Header("LISTEN Channels")]
    /// <summary>
    /// The SO channel for the UI events
    /// </summary>
    [Tooltip("The SO channel for the UI events")]
    [SerializeField] private UIEventsChannelSO uiEventsChannelSO;

    /// <summary>
    /// The SO channel for the AR events
    /// </summary>
    [Tooltip("The SO channel for the AR events")]
    [SerializeField] private AREventChannelSO arEventChannelSO;

    [Header("SO References")]
    [SerializeField] private GameStateSO gameStateSO;
    [SerializeField] private SessionDataSO sessionDataSO;
    [SerializeField] private PointsOfInterestSO pointsOfInterestSO;

    [Header("Debug SO")]
    [SerializeField] private DebugUIEventChannelSO debugUIEventChannelSO;

    //#if UNITY_EDITOR
    //[Header("Debug")]
    //[SerializeField] private Texture2D texture;
    //[SerializeField] private RawImage debug;
    //#endif

    #endregion

    private PointOfInterest tempPOI = new();

    private void OnEnable()
    {
        arEventChannelSO.OnPOIDetected += HandlePOIDetected;
    }

    private void OnDisable()
    {
        arEventChannelSO.OnPOIDetected -= HandlePOIDetected;
    }

    // Start is called before the first frame update
    void Start()
    {
        VuforiaApplication.Instance.OnVuforiaInitialized += StartSpwanImageTargets;
    }

    #region Coroutines
    /// <summary>
    /// Coroutine to build the Reference Image Library
    /// </summary>
    private IEnumerator SpawnImageTargets()
    {
        if(sessionDataSO.ResumeSession)
            Debug.Log("[VUFORIA] Resuming previous session");

        // For every p.o.i. in the session
        for (int i = 0; i < sessionDataSO.PointsOfInterest.Points.Count; i++)
        {
            // If we are resuming a previous session ignore the POIs that were already found
            if (sessionDataSO.PointsOfInterest.Points[i].detected)
            {
                Debug.Log("[VUFORIA] POI " + sessionDataSO.PointsOfInterest.Points[i].title + " already found, skipped");
                continue;
            }
                
            // Set to 1 for incrementing in the foreach loop the image target gameobject name    
            int k = 1;

            // Loop through every image inside the p.o.i.
            foreach (KeyValuePair<string, Texture2D> entry in sessionDataSO.PointsOfInterest.Points[i].imageNameAndTexture)
            {
                // If the image is readable
                if (entry.Value.isReadable)
                {
                    // Schedule a job to add the image to the library
                    var mImageTarget = VuforiaBehaviour.Instance.ObserverFactory.CreateImageTarget(entry.Value, 1, sessionDataSO.PointsOfInterest.Points[i].title + k.ToString());

                    // Yield until the the image is added to the library
                    yield return new WaitUntil(() => mImageTarget != null);

                    // Add the Observer to the ImageTarget
                    var observer = mImageTarget.gameObject.AddComponent<CustomObserverEventHandler>();

                    Debug.Log("[VUFORIA] Target created and active" + mImageTarget.gameObject.name);

                    // Set the reference for the AR Event Channel SO
                    observer.AREventChannelSO = arEventChannelSO;
                    // Set the reference for the UI Event Channel SO
                    observer.UIEventChannelSO = uiEventsChannelSO;

                    // Set image name
                    observer.ImageName = entry.Key;

                    // Keep track of the relation between a POI image name and a Vuforia Image Target Object
                    sessionDataSO.PointsOfInterest.ImageNameAndImageTargetObject.Add(entry.Key, observer.gameObject);

                    Debug.Log("[VUFORIA] " + entry.Key + " imageName: " + mImageTarget.TargetName);

                    k++;
                }
                // if The image is not readable
                else
                {
                    Debug.Log($"[VUFORIA] Image {entry.Key} must be readable to be added to the image library.");
                    
                    yield return null;
                }
            }
        }
         
        // If we are still in the loading state it means that the scene it's just starting to run
        // so we can notify that the image targets creation is finished 
        if (gameStateSO.CurrentGameState == GameState.Loading)
        {
            // Raise an event to notify that the reference library was created for the first time
            arEventChannelSO.RaiseImageTargetsCreationFinishedEvent();
        }
    }
    #endregion

    #region Callbacks
    /// <summary>
    /// Callbak to spawn all the Vuforia Image Target objects
    /// </summary>
    /// <param name="error">The Vuforia error value</param>
    private void StartSpwanImageTargets(VuforiaInitError error)
    {
        StartCoroutine(SpawnImageTargets());
    }

    /// <summary>
    /// Callback to handle a POI detection
    /// </summary>
    /// <param name="imageName"></param>
    private void HandlePOIDetected(string imageName)
    {
        Debug.Log("DETECTED image name: " + imageName);

        // Get the poi related to the image name
        if (sessionDataSO.PointsOfInterest.ImageNameAndPOI.ContainsKey(imageName))
        {
            tempPOI = sessionDataSO.PointsOfInterest.ImageNameAndPOI[imageName];
        }

        Debug.Log("of POI: " + tempPOI.title);
                        
        // Delete the respectives Vuforia Image Target objects
        // and then remove them from the dictionary
        foreach (var entry in tempPOI.imageNameAndTexture)
        {
            // Destroy the Vuforia Image Target Object
            Destroy(sessionDataSO.PointsOfInterest.ImageNameAndImageTargetObject[entry.Key]);

            // Remove the Vuforia Image Target from the dictionary
            sessionDataSO.PointsOfInterest.ImageNameAndImageTargetObject.Remove(entry.Key);
        }
    }
    #endregion
}
