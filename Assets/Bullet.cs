using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Transform target = null;
    public float chasingOffset = 0.3f;
    public float speed = 50;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        FlyTo();
    }

    void FlyTo()
    {
        if (target == null) { Destroy(gameObject); return; }

        if (Vector3.Distance(transform.position, target.position) <= chasingOffset) { Destroy(this.gameObject); return; }    
        Vector3 dir = target.position - transform.position; 
        dir = new Vector3(dir.x, dir.y, dir.z);
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);
    }
}
