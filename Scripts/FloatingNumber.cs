using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingNumber : MonoBehaviour
{
    Animator Anim;
    // Start is called before the first frame update

        //This script just sets the position and lifetime of the floating text when it gets instantiated.
    void Start()
    {
        transform.position = transform.position + new Vector3(Random.Range(0.25f, 2f), 0, Random.Range(0.25f, 2f));
        Destroy(this.gameObject, 2);
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = new Quaternion(0, 0, 0, 1);
        
    }
}
