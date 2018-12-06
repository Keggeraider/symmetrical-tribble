using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class HomingMissile : MonoBehaviour {
    public Transform target;
    public float torque = 5f;
    public float speed = 10f;
    public float range;

    Rigidbody rb;

    void Start() {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate() {
        if (target != null) {
            Vector3 direction = target.position - rb.position;
            direction.Normalize();
            // Tech debt: Need to adjust rotateAmount vector with obstacle avoidance
            // so the missile stops getting stuck on the walls
            Vector3 rotateAmount = Vector3.Cross(direction, transform.up);
            rb.angularVelocity = -torque * rotateAmount;
            rb.velocity = transform.up * speed;

        } else {
            // Debug.Log("No target");
            Destroy(gameObject, 2f);
        }
    }

    // void OnCollisionEnter(Collision collision) {
    //     if (collision.collider.transform == target) {
    //         Debug.Log("Attacking enemy " + collision.gameObject.name + " for 10 damage");
    //         collision.gameObject.SendMessageUpwards("TakeDamage", 10, SendMessageOptions.DontRequireReceiver);
    //         Destroy(gameObject);
    //     }
    // }
}
