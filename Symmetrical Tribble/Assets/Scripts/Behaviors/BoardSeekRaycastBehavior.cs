using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardSeekRaycastBehavior : MonoBehaviour
{
    public float boundary; // use negative value to disable the boundary/proximity activation
    public bool boundaryReached;
    public float speed;
    public GameObject target;

    private const bool DEBUGMODE = true;
    private const int FRAMERATE = 1; //3;
    private Rigidbody rb;

    private int _currentFrameRate;

    void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _currentFrameRate = 0;
        if(target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player");
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(++_currentFrameRate % FRAMERATE != 0)
        {
            return;
        }

        // Only activated once boundaryReached flag has been turned on
        if (!boundaryReached)
        {
            float distance = Vector3.Distance(this.transform.position,
                                              target.transform.position);
            if (boundary < 0 || distance < boundary)
            {
                boundaryReached = true;
            }
        }

        if (!boundaryReached)
        {
            return;
        }

        float step = speed * Time.deltaTime;

        // rotation
        Vector3 targetDir = target.transform.position - this.transform.position;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, 2.0f, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDir);
        Vector3 rightDir = Quaternion.AngleAxis(30f, transform.up) * transform.forward;
        Vector3 leftDir = Quaternion.AngleAxis(-30f, transform.up) * transform.forward;
        if(DEBUGMODE)
        {
            Debug.DrawRay(this.transform.position, newDir, Color.yellow, step);
            Debug.DrawRay(this.transform.position, rightDir, Color.blue, step);
            Debug.DrawRay(this.transform.position, leftDir, Color.cyan, step);
        }
        bool contactLeft = false,
             contactFront = false,
             contactRight = false;
        RaycastHit hitLeft,
                   hitFront,
                   hitRight;
        Ray rayLeft = new Ray(this.transform.position, leftDir),
            rayFront = new Ray(this.transform.position, newDir),
            rayRight = new Ray(this.transform.position, rightDir);
        if (Physics.Raycast(rayLeft, out hitLeft, speed))
        {
            if (!hitLeft.collider.CompareTag("Pick Up")
                && !hitLeft.collider.CompareTag("Player"))
            {
                contactLeft = true;
            }
        }
        if (Physics.Raycast(rayFront, out hitFront, speed))
        {
            if (!hitFront.collider.CompareTag("Pick Up")
                && !hitFront.collider.CompareTag("Player"))
            {
                contactFront = true;
            }
        }
        if (Physics.Raycast(rayRight, out hitRight, speed))
        {
            if (!hitRight.collider.CompareTag("Pick Up")
                && !hitRight.collider.CompareTag("Player"))
            {
                contactRight = true;
            }
        }

        Vector3 targetPos = target.transform.position;
        // This seems like there has to be a better way
        // than hardcoding the behavior... but it works
        // also based on feelers 30degrees from center (60deg from each other)
        // used 170deg to turn around vs 180, to hopefully get "unstuck"
        if(contactLeft)
        {
            if(contactFront)
            {
                if(contactRight)
                {
                    // -- \|/
                    targetPos = Quaternion.AngleAxis(170f, transform.up) * targetPos;
                }
                else
                {
                    // -- \|
                    targetPos = Quaternion.AngleAxis(60f, transform.up) * targetPos;
                }
            }
            else if(contactRight)
            {
                // -- \ /
                targetPos = Quaternion.AngleAxis(170f, transform.up) * targetPos;
            }
            else
            {
                // -- \
                targetPos = Quaternion.AngleAxis(30f, transform.up) * targetPos;
            }
        }
        else if(contactFront)
        {
            if(contactRight)
            {
                // -- |/
                targetPos = Quaternion.AngleAxis(-60f, transform.up) * targetPos;
            }
            else
            {
                // -- |
                targetPos = Quaternion.AngleAxis(90f, transform.up) * targetPos;
            }
        }
        else if(contactRight)
        {
            // -- /
            targetPos = Quaternion.AngleAxis(-30f, transform.up) * targetPos;

        }


        // position
        Vector3 nextStep = Vector3.MoveTowards(transform.position,
                                               targetPos,
                                               step);
        if (rb == null) {
            transform.position = nextStep;
        } else {
            rb.MovePosition(nextStep);
        }
    }
}
