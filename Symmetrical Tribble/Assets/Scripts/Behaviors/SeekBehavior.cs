using UnityEngine;
using System.Collections;

public class SeekBehavior : MonoBehaviour
{
    public GameObject target;   // target is set in the IDE
    public float speed;         // so is speed value
    public float initialHeight;

    private float _acceleration;

    // Use this for initialization
    void Start()
    {
        _acceleration = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null || !gameObject.activeSelf) 
        {
            return;
        }

        float step = speed * Time.deltaTime;
        _acceleration += step;
        Vector2 xz = Vector2.MoveTowards(new Vector2(this.transform.position.x, this.transform.position.z),
                                         new Vector2(target.transform.position.x, target.transform.position.z),
                                         step);
        this.transform.position = new Vector3(xz.x, 
                                              initialHeight - _acceleration,
                                              xz.y);

        //Vector3 direction = Vector3.Normalize(target.transform.position - transform.position);
        //transform.position += direction * speed * Time.deltaTime;

        // if bomb has reached the ground, remove it from view
        if (transform.position.y < 0f)
        {
            gameObject.SetActive(false);
        }
    }
}
