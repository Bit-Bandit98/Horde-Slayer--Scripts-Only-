﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetToZero : MonoBehaviour
{
    
    

    // Update is called once per frame
    void FixedUpdate()
    {

        transform.localPosition = Vector3.zero;
    }
}
