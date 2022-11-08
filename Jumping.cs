using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(GroundChecker))]
[RequireComponent (typeof(Animator))]

public class Jumping : MonoBehaviour
{
    [SerializeField] private InputController _input;
    [SerializeField, Range(0f, 10f)] private float _jumpHeight = 3f; 
    [SerializeField, Range(0, 5)] private int _maxAirJumps = 0;
    [SerializeField, Range(0f, 5f)] private float _upwardMovementMultiplier = 1.7f;
    [SerializeField, Range(0f, 5f)] private float _downwardMovementMultiplier = 3f;

    private const string AnimatorYVelocity = "YVelocity";
    private const string AnimatorGrounded = "Grounded";

    private Animator _animator;
    private Rigidbody2D _rigidBody;
    private GroundChecker _groundChecker;
    private Vector2 _velocity;
    private int _jumpPhase;
    private float _defaultGravityScale;
    private bool _isGrounded;
    private bool _desireToJump;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rigidBody = GetComponent<Rigidbody2D>();
        _groundChecker = GetComponent<GroundChecker>();
        _defaultGravityScale = 1f;
    }

    private void Update()
    {
        _desireToJump |= _input.RetrieveJumpInput();

        _animator.SetBool(AnimatorGrounded, _isGrounded);
        _animator.SetFloat(AnimatorYVelocity, _velocity.y);
    }

    private void FixedUpdate()
    {
        _isGrounded = _groundChecker.IsGrounded;
        _velocity = _rigidBody.velocity;

        if (_isGrounded)
            _jumpPhase = 0;

        if (_desireToJump)
        {
            _desireToJump = false;
            JumpAction();
        }

        if (_rigidBody.velocity.y > 0)
            _rigidBody.gravityScale = _upwardMovementMultiplier;
        
        else if (_rigidBody.velocity.y < 0)
            _rigidBody.gravityScale = _downwardMovementMultiplier;
        
        else if (_rigidBody.velocity.y == 0)
            _rigidBody.gravityScale = _defaultGravityScale;

        _rigidBody.velocity = _velocity;
    }

    private void JumpAction()
    {
        if( _isGrounded || _jumpPhase < _maxAirJumps)
        {
            _jumpPhase += 1;
            float jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * _jumpHeight);

            if (_velocity.y > 0f)
                jumpSpeed = Mathf.Max(jumpSpeed - _velocity.y, 0f);

            _velocity.y += jumpSpeed;
        }
    }
}
