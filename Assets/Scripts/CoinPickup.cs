using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] AudioClip coinPickupSFX;
    [SerializeField] int coinValue = 10;

    bool wasCollected = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !wasCollected)
        {
            wasCollected = true;
            AudioSource.PlayClipAtPoint(coinPickupSFX, Camera.main.transform.position);

            gameObject.SetActive(false);
            Destroy(gameObject);

            FindObjectOfType<GameSession>().addScore(coinValue);

        }
    }
}
