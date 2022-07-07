using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region External classes

[Serializable]
public class NPCAvatar
{
    public int avatarID;
    public string avatarName;
    public Sprite sprite;
}

#endregion


[CreateAssetMenu(fileName = "New NPCAvatarCollection", menuName = "Data/NPC Avatar Collection SO")]
public class NPCAvatarCollectionSO : ScriptableObject
{
    #region Inspector
    [SerializeField] private List<NPCAvatar> npcAvatars = new List<NPCAvatar>();
    #endregion

    #region Properties
    public List<NPCAvatar> NPCAvatars { get => npcAvatars; }
    #endregion
}
