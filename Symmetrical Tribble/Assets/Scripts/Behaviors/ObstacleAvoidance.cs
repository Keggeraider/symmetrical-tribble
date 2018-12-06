using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleAvoidance : MonoBehaviour {

    private const bool DEBUGMODE = true;
    private Transform target;
    private float step = 100f;

    HomingMissile hm;

    // Start is called before the first frame update
    void Start() {
        hm = GetComponent<HomingMissile>();
        target = hm.target;
    }

    // Update is called once per frame
    void Update() {
        Vector3 destination = target.position - transform.position;

        destination.Normalize();

        sendRay(transform.position, ref destination);

        Vector3 leftRay = transform.position + 2 * Vector3.left;
        Vector3 rightRay = transform.position + 2 * Vector3.right;
        // Vector3 upRay = transform.position + 2 * Vector3.up;

        sendRay(leftRay, ref destination);
        sendRay(rightRay, ref destination);
        // sendRay(upRay, ref destination);

        Quaternion rotation = Quaternion.LookRotation(destination);

        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * step);
    }

    void sendRay(Vector3 start, ref Vector3 end) {
        RaycastHit hit;
        if(Physics.Raycast(start, transform.forward, out hit, 1f)) {
              if(DEBUGMODE)
                  Debug.DrawLine(transform.position, hit.point, Color.red);

              end += hit.normal * step;
        }
    }
}
