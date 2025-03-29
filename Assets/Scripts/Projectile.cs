using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public float speed = 1.0f;
    private Transform transform;

    // Start is called before the first frame update
    void Start()
    {
        transform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * Time.deltaTime * speed);
    }
}
