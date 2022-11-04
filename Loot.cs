using System.Collections;
using UnityEngine;

public class Loot : MonoBehaviour
{
    private WaitForSeconds _waitBeforeDestroy = new WaitForSeconds(0.15f);
    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.TryGetComponent<Player>(out Player player))
            StartCoroutine(WaitBeforeDestroy());
    }

    private IEnumerator WaitBeforeDestroy()
    {
        bool readyToDestroy = false;

        while(readyToDestroy == false)
        {
            _audioSource.Play();
            yield return _waitBeforeDestroy;
            Destroy(gameObject);
            readyToDestroy = true;
        }
    }
}
