using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject _objectTemplate;

    private GameObject _spawnedObject;

    public bool IsBusy { get; private set; }

    public void Update()
    {
        if(_spawnedObject == null)
            IsBusy = false;
    }

    public void Spawn()
    {
        if(IsBusy == false)
        {
            _spawnedObject = Instantiate(_objectTemplate, transform.position, Quaternion.identity);
            IsBusy = true;
        }
    }
}
