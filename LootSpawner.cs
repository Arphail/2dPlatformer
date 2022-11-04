using UnityEngine;

public class LootSpawner : MonoBehaviour
{
    private Spawner[] _spawners;

    void Start()
    {
        _spawners = GetComponentsInChildren<Spawner>();

        foreach(var spawner in _spawners)
            spawner.Spawn();
    }
}
