using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private Transform _path;
    [SerializeField] private PlayerFinder _playerFinder;
    [SerializeField] private float _speed;
    [SerializeField] private float _speedBoost;

    private Player _player;
    private Transform _target;
    private Transform[] _points;
    private int _currentPoint;
    private float _boostedSpeed;

    private void Start()
    {
        _boostedSpeed = _speed + _speedBoost;
        _points = new Transform[_path.childCount];

        for(int i = 0; i < _path.childCount; i++)
            _points[i] = _path.GetChild(i);
    }

    private void Update()
    {
        if (_playerFinder.PlayerToChase == null)
            _target = _points[_currentPoint];
        
        if (_playerFinder.PlayerToChase != null)
        {
            IncreaseSpeed();
            _player = _playerFinder.PlayerToChase.GetComponent<Player>();
            _target = _player.transform;
        }
            
        transform.position = Vector3.MoveTowards(transform.position, _target.position, _speed * Time.deltaTime);

        if (transform.position == _target.position)
        {
            _currentPoint++;

            if (_currentPoint >= _points.Length)
                _currentPoint = 0;
        }
    }

    private void IncreaseSpeed()
    {
        _speed = _boostedSpeed;
    }
}
