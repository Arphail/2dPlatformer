using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]

public class PhysicsMovement : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpVelocity;
    [SerializeField] private float _minGroundNormalY = .65f;
    [SerializeField] private float _gravityModifier = 1f;
    [SerializeField] private LayerMask _layerMask;
    
    public Vector2 Velocity;

    private AudioSource _audioSource;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;

    protected Rigidbody2D rb2d;
    protected List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(16);
    protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
    protected ContactFilter2D contactFilter;
    protected Vector2 targetVelocity;
    protected Vector2 groundNormal;
    protected bool grounded;

    protected const float minMoveDistance = 0.001f;
    protected const float shellRadius = 0.01f;

    void OnEnable()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(_layerMask);
        contactFilter.useLayerMask = true;
    }

    void Update()
    {
        targetVelocity = new Vector2(Input.GetAxis("Horizontal") * _speed, 0);
        
        if (Input.GetAxis("Horizontal") != 0)
        {
            _animator.SetBool("IsMoving", true);

            if (Input.GetAxis("Horizontal") < 0)
                _spriteRenderer.flipX = true;
            else if (Input.GetAxis("Horizontal") > 0)
                _spriteRenderer.flipX = false;
        }
        else
        {
            _animator.SetBool("IsMoving", false);
        }

        if (Velocity.y == 0)
            _animator.SetBool("Grounded", true);
        else
            _animator.SetBool("Grounded", false);

        if (Input.GetKey(KeyCode.Space) && grounded)
        {
            _audioSource.Play();
            Velocity.y = _jumpVelocity;
        }

        _animator.SetFloat("YVelocity", Velocity.y);
    }

    void FixedUpdate()
    {
        Velocity += _gravityModifier * Physics2D.gravity * Time.deltaTime;
        Velocity.x = targetVelocity.x;

        grounded = false;

        Vector2 deltaPosition = Velocity * Time.deltaTime;
        Vector2 moveAlongGround = new Vector2(groundNormal.y, -groundNormal.x);
        Vector2 move = moveAlongGround * deltaPosition.x;

        Movement(move, false);

        move = Vector2.up * deltaPosition.y;

        Movement(move, true);
    }

    void Movement(Vector2 move, bool yMovement)
    {
        float distance = move.magnitude;

        if (distance > minMoveDistance)
        {
            int count = rb2d.Cast(move, contactFilter, hitBuffer, distance + shellRadius);

            hitBufferList.Clear();

            for (int i = 0; i < count; i++)
                hitBufferList.Add(hitBuffer[i]);

            for (int i = 0; i < hitBufferList.Count; i++)
            {
                Vector2 currentNormal = hitBufferList[i].normal;
                if (currentNormal.y > _minGroundNormalY)
                {
                    grounded = true;
                    if (yMovement)
                    {
                        groundNormal = currentNormal;
                        currentNormal.x = 0;
                    }
                }

                float projection = Vector2.Dot(Velocity, currentNormal);

                if (projection < 0)
                    Velocity -= projection * currentNormal;

                float modifiedDistance = hitBufferList[i].distance - shellRadius;
                distance = modifiedDistance < distance ? modifiedDistance : distance;
            }
        }

        rb2d.position += move.normalized * distance;
    }
}