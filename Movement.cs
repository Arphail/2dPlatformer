using System;
using UnityEngine;

[RequireComponent (typeof(Rigidbody2D))]
[RequireComponent (typeof(GroundChecker))]
[RequireComponent (typeof(SpriteRenderer))]
[RequireComponent (typeof(Animator))]

public class Movement : MonoBehaviour
{
    [SerializeField] private InputController _input;
    [SerializeField, Range(0f, 100f)] private float _maxSpeed = 4f;
    [SerializeField, Range(0f, 100f)] private float _maxAcceleration = 35f;
    [SerializeField, Range(0f, 100f)] private float _maxAirAcceleration = 20f;

    private const string AnimatorMoving = "IsMoving";

    private Animator _animator;
    private Rigidbody2D _rigidBody;
    private GroundChecker _groundChecker;
    private SpriteRenderer _spriteRenderer;
    private Vector2 _direction;
    private Vector2 _targetVelocity;
    private Vector2 _velocity;
    private float _maxSpeedChange;
    private float _acceleration;
    private bool _isGrounded;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rigidBody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _groundChecker = GetComponent<GroundChecker>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            _animator.SetBool(AnimatorMoving, true);
        else
            _animator.SetBool(AnimatorMoving, false);

        if (_direction.x < 0)
            _spriteRenderer.flipX = true;

        else if (_direction.x > 0)
            _spriteRenderer.flipX = false;

        _direction.x = _input.RetrieveMoveInput();
        _targetVelocity = new Vector2(_direction.x, 0f) * Mathf.Max(_maxSpeed - _groundChecker.Friction, 0f);
    }

    private void FixedUpdate()
    {
        _isGrounded = _groundChecker.IsGrounded;
        _velocity = _rigidBody.velocity;

        _acceleration = _isGrounded ? _maxAcceleration : _maxAirAcceleration;
        _maxSpeedChange = _acceleration * Time.deltaTime;
        _velocity.x = Mathf.MoveTowards(_velocity.x, _targetVelocity.x, _maxSpeedChange);

        _rigidBody.velocity = _velocity;
    }
}
