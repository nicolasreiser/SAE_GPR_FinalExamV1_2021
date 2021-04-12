using System.Collections.Generic;
using UnityEngine;


public class DropSpawner : MonoBehaviour
{
    [SerializeField] DropPrefabPair[] dropsPrefabs;
    [SerializeField] Transform dropsParent;

    Dictionary<Drop, GameObject> dropPrefabTable;

    private void Awake()
    {
        dropPrefabTable = new Dictionary<Drop, GameObject>();

        foreach (var dp in dropsPrefabs)
        {
            if (dropPrefabTable.ContainsKey(dp.Drop))
            {
                Debug.LogWarning($"Double definition of drop prefab for drop: {dp.Drop.DropName}");
            }
            else
            {
                dropPrefabTable.Add(dp.Drop, dp.Prefab);
            }
        }
    }

    public GameObject SpawnDropAt(Drop drop, Vector3 position)
    {
        if (drop == null)
            return null;

        if (dropPrefabTable.ContainsKey(drop))
        {
            var go = Instantiate(dropPrefabTable[drop], position, Quaternion.identity, dropsParent);

            //Inject drop reference
            if (go.TryGetComponent(out IDropOwner dropOwner))
            {
                dropOwner.SetDrop(drop);
            }

            return go;
        }
        else
        {
            Debug.LogError($"No drop prefab defined for {drop.DropName} on DropSpawner {name}");
            return null;
        }
    }
}

[System.Serializable]
public class DropPrefabPair
{
    public Drop Drop;
    public GameObject Prefab;
}
