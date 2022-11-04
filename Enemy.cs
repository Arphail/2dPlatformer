using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private AudioSource _audioSource;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Player player))
            _animator.SetTrigger("Hit");
    }

    private void OnEnable()
    {
        _audioSource.Play();
    }

    private void OnDeathAnimationFinished()
    {
        Destroy(gameObject);
    }
}
