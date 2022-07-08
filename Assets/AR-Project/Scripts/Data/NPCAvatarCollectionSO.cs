using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region External classes

[Serializable]
public class NPCAvatar
{
    public string avatarName;
    public Sprite sprite;
}

#endregion


[CreateAssetMenu(fileName = "New NPCAvatarCollection", menuName = "Data/NPC Avatar Collection SO")]
public class NPCAvatarCollectionSO : ScriptableObject
{
    #region Inspector
    [SerializeField] private GenericDictionary<int, NPCAvatar> npcAvatars = new GenericDictionary<int, NPCAvatar>();
    #endregion

    #region Properties
    #endregion

    #region Public methods
    public Sprite GetAvatarSpriteByID(int id)
    {
        return npcAvatars[id].sprite;
    }

    public string GetAvatarNameByID(int id)
    {
        return npcAvatars[id].avatarName;
    }
    #endregion
}
