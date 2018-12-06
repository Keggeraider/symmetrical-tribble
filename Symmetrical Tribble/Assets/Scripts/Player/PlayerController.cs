using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {
    private PlayerHealth health;


    void Awake() {
        health = GetComponent<PlayerHealth>();
        // Run the SetCountText function to update the UI (see below)
    }

    void OnTriggerEnter(Collider other) {
        switch(other.gameObject.tag) {
            case "Pick Up":
                HandlePickup(other);
                break;
            case "BombTrigger":
                GameManager.instance.triggerBomb();
                break;
            default:
                break;
        }
	  }

    void HandlePickup(Collider other)
    {
        // Make the other game object (the pick up) inactive, to make it disappear
        other.gameObject.SetActive(false);

        // Add one to the score variable 'count'
        ScoreManager.score += 1;

        GameManager.instance.checkWinCondition();

    }

    void OnCollisionEnter(Collision c) {
            if (c.gameObject.CompareTag("Bomb"))
                health.TakeDamage(health.currentHealth);
    }
}
