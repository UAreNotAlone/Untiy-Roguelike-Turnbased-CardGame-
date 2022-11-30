using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotation : MonoBehaviour
{
    public float speed = 10f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float s = Time.deltaTime * speed;
        transform.Rotate(Vector3.forward, s);
        transform.Rotate(Vector3.up, Random.Range(-s, s));
        
    }
}
