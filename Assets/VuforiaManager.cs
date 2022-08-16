using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class VuforiaManager : MonoBehaviour
{
    #region Inspector
    [Header("SO References")]
    [SerializeField] private GameStateSO gameStateSO;
    [SerializeField] private SessionDataSO sessionDataSO;
    [SerializeField] private PointsOfInterestSO pointsOfInterestSO;

    [Header("Debug")]
    [SerializeField] private Texture2D texture;
    [SerializeField] private RawImage debug;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        VuforiaApplication.Instance.OnVuforiaInitialized += SpawnImageTargets;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Coroutines
    /// <summary>
    /// Coroutine to build the Reference Image Library
    /// </summary>
    private void SpawnImageTargets(VuforiaInitError error)
    {
        Debug.Log(sessionDataSO.PointsOfInterest.Points[0].title);

        debug.texture = texture;

        var mTarget = VuforiaBehaviour.Instance.ObserverFactory.CreateImageTarget(
            texture,
            1f,
            "Asimov");
        // Add the DefaultObserverEventHandler to the newly created game object
        mTarget.gameObject.AddComponent<DefaultObserverEventHandler>();
        Debug.Log("Target created and active" + mTarget);

        /*// For every p.o.i. in the session
        for (int i = 0; i < sessionDataSO.PointsOfInterest.Points.Count; i++)
        {
            GameObject imageTarget = new GameObject("ImageTarget" + i);

            // Loop through every image inside the p.o.i.
            foreach (KeyValuePair<string, Texture2D> image in sessionDataSO.PointsOfInterest.Points[i].imageNameAndTexture)
            {
                // If the image is readable
                if (image.Value.isReadable)
                {
                    // Schedule a job to add the image to the library
                    var mImageTarget = VuforiaBehaviour.Instance.ObserverFactory.CreateImageTarget(image.Value, 1, image.Key);

                    imageTarget.gameObject.AddComponent<ImageTargetBehaviour>();

                // Yield until the the image is added to the library
                //yield return new WaitUntil(() => mImageTarget != null);

                    Debug.Log("[ARP] " + image.Key + " imageName: " + mImageTarget.TargetName);
                }
                // if The image is not readable
                else
                {
                    Debug.Log($"[ARP] Image {image.Key} must be readable to be added to the image library.");

                    //yield return null;
                }
            }
        }
         
        // If we are still in the loading state it means that the scene it's just starting to run
        // so we can notify that the reference library creation is completed 
        if (gameStateSO.CurrentGameState == GameState.Loading)
        {
            //EnableTrackedImageManager();
            //gameStateSO.UpdateGameState(GameState.Tracking);

            // Raise an event to notify that the reference library was created for the first time
            //arEventChannelSO.RaiseReferenceLibraryFirstTimeCreatedEvent();
        }*/
    }
 
    #endregion
}
