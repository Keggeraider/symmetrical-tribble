using UnityEngine;
using System.Linq;

public class PlayerAttacking : MonoBehaviour {
    Animator anim;
    HomingMissile missileScript;
    float radius;
    bool punching;
    Vector3 center;
    Animation leftPunch;
    Animation rightPunch;
    string leftPunchTrigger = "lpunch";
    string rightPunchTrigger = "rpunch";

    void Awake() {
        anim = GetComponent<Animator>();
        punching = false;
    }

    void Update() {
        if(Input.GetMouseButtonDown(0)) {
            anim.SetTrigger(leftPunchTrigger);
        } else if(Input.GetMouseButtonDown(1)) {
            anim.SetTrigger(rightPunchTrigger);
        } else if(Input.GetKey(KeyCode.F)) {
            anim.SetTrigger("throwing");
        }

    }

    private void hookConnect() {
        punching = true;
    }

    private void clearHook() {
        punching = false;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(center, radius);
    }

    void spawnMissiles(GameObject prefab) {
        GameObject missile;
        missile = Instantiate(prefab, transform.position + new Vector3(0f, 0.5f, 0.8f), transform.rotation, transform);
        missileScript = missile.GetComponent<HomingMissile>();
        missileScript.target = findTarget(transform.TransformDirection(Vector3.forward));
    }

    void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Enemy") && punching) {
            other.gameObject.SendMessageUpwards("TakeDamage", 10, SendMessageOptions.DontRequireReceiver);
        }
    }

    Transform findTarget(Vector3 direction) {
        // Transform target = null;
        LayerMask enemiesMask = LayerMask.GetMask("enemies");
        radius = missileScript.range;
        center = transform.position + (direction * radius);

        // Find closest visible target in front of player
        Collider[] objectColliders = Physics.OverlapSphere(center, radius, enemiesMask);
        Debug.Log(objectColliders.Length + " enemies nearby");
        var nearest = objectColliders
          .OrderBy(t => (t.gameObject.transform.position - transform.position).sqrMagnitude)
          .FirstOrDefault();

        return nearest.gameObject.transform;
    }
}
