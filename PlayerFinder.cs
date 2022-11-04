using UnityEngine;

public class PlayerFinder : MonoBehaviour
{
    public Player PlayerToChase { get; private set; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Player>(out Player player))
            PlayerToChase = player;
    }
}
