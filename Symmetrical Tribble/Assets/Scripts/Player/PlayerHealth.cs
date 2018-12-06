using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class PlayerHealth : MonoBehaviour {

    public int startingHealth = 100;
    // [HideInInspector]
    public int currentHealth;
    public Text healthStatusText;
    public Image damageImage;
    public float flashSpeed = 5f;
    public Color flashColor = new Color(1f, 0f, 0f, 0.1f);
    // public AudioClip deathClip;

    Animator anim;
    AudioSource playerAudio;
    Rigidbody rb;
    Collider c;
    PlayerAttacking playerAttacking;
    bool isDead;
    bool damaged;

    [System.Serializable]
    public class HealthLevel {
        public string text;
        public Color color;

        public HealthLevel(string t, Color c) {
            text = t;
            color = c;
        }
    }

    HealthLevel[] healthLevel =  new HealthLevel[] {
        new HealthLevel("Dying", Color.red),
        new HealthLevel("Like a zombie", new Color((float)0.85 *Color.red.r, Color.red.g,Color.red.b)),
        new HealthLevel("Running on fumes", Color.yellow),
        new HealthLevel("Weak", Color.yellow),
        new HealthLevel("Injured", Color.cyan),
        new HealthLevel("Ok", Color.blue),
        new HealthLevel("Strong", new Color(Color.green.r,(float)0.9 * Color.green.g,Color.green.b)),
        new HealthLevel("Super strong", Color.green),
        
    };

    void Awake() {
        anim = GetComponent <Animator> ();
        playerAudio = GetComponent <AudioSource> ();
        rb = GetComponent<Rigidbody>();
        c = GetComponent<CapsuleCollider>();
        playerAttacking = GetComponentInChildren <PlayerAttacking> ();
        currentHealth = startingHealth;
        setHealthText();
    }

    void Update() {
        if(damaged) {
            damageImage.color = flashColor;
        } else {
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed);
        }
        damaged = false;
    }

    public void TakeDamage(int amount) {
        damaged = true;
        currentHealth -= amount;
        if(currentHealth <= 0 && !isDead) {
            Death();
        }
        if(!setHealthText() && !isDead) {
            Death();
        }
        // playerAudio.Play();
    }

    bool setHealthText() {
        //note. there are 8 health levels. so 100/8. means they change at 12.5 step sizes.
        //so if you take damage of 10 and you started with 100 you are still super strong.
        double healthStatusStepSize = (double)startingHealth / healthLevel.Length;
        int lostHealthStrength = startingHealth - currentHealth;
        int healthStepsLost = Convert.ToInt32(((double)lostHealthStrength / healthStatusStepSize));

        int newHeathLevel = (healthLevel.Length - healthStepsLost)-1;
        int statusIndex = Mathf.Clamp(newHeathLevel, 0, healthLevel.Length-1);
        healthStatusText.text = healthLevel[statusIndex].text;
        healthStatusText.color = healthLevel[statusIndex].color;
        return newHeathLevel > 0;
    }

    void Death() {
        isDead = true;
        anim.SetTrigger("die");
        c.isTrigger = true;
        rb.isKinematic = true;
        rb.detectCollisions = false;

        // playerAudio.clip = deathClip;
        // playerAudio.Play();

        GameManager.instance.gameOver();
        // anim.enabled = false;
        playerAttacking.enabled = false;
    }
}
