using UnityEngine;

public class EnemyHealth : MonoBehaviour {
    public int startingHealth = 100;
    public int currentHealth;
    // public AudioClip deathClip;


    Animator anim;
    // AudioSource enemyAudio;
    // ParticleSystem hitParticles;
    CapsuleCollider capsuleCollider;
    bool isDead;


    void Awake () {
        anim = GetComponent <Animator> ();
        // enemyAudio = GetComponent <AudioSource> ();
        capsuleCollider = GetComponent <CapsuleCollider> ();

        currentHealth = startingHealth;
    }


    void Update () {
    }


    public void TakeDamage (int amount/*, Vector3 hitPoint*/) {
        if(isDead)
            return;

        // enemyAudio.Play ();

        currentHealth -= amount;

        if(currentHealth <= 0)
            Death ();
    }


    void Death () {
        isDead = true;
        Destroy (gameObject, 2f);

        // anim.SetTrigger ("die");

        // enemyAudio.clip = deathClip;
        // enemyAudio.Play ();
    }
}
