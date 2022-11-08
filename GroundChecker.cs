using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    private float _minNormal = 0.09f;

    public bool IsGrounded { get; private set; }
    public float Friction { get; private set; }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        EvaluateColission(collision);
        RetrieveFriction(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        EvaluateColission(collision);
        RetrieveFriction(collision);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        IsGrounded = false;
        Friction = 0;
    }

    private void EvaluateColission(Collision2D collision)
    {
        for (int i = 0; i < collision.contactCount; i++)
        {
            Vector2 normal = collision.GetContact(i).normal;
            IsGrounded |= normal.y >= _minNormal;
        }
    }

    private void RetrieveFriction(Collision2D collision)
    {
        PhysicsMaterial2D material = collision.rigidbody.sharedMaterial;

        Friction = 0;

        if (material != null)
            Friction = material.friction;
    }
}
