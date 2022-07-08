using UnityEngine;

#region External classes
public enum EIconType
{
    Event,
    Dialog,
    Object
}
#endregion

[CreateAssetMenu(fileName = "New POIIconCollection", menuName = "Data/POI Icon Collection SO")]
public class POIIconCollectionSO : ScriptableObject
{
    #region Inspector
    [SerializeField] private GenericDictionary<EIconType, Sprite> poiIcons = new GenericDictionary<EIconType, Sprite>();
    #endregion

    #region Public methods
    public Sprite GetIconByType(EIconType iconType)
    {
        return poiIcons[iconType];
    }
    #endregion
}
