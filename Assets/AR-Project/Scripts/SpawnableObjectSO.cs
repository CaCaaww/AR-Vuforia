using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SpawnManagerScriptableObject", order = 1)]
public class SpawnableObjectSO : ScriptableObject
{
    [SerializeField]
    private List<GameObject> spawnableObjects;

    public List<GameObject> SpawnableObjects { get { return spawnableObjects; } }

}
