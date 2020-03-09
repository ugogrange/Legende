﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerKeyboard : MonoBehaviour
{
public float walkSpeed;
public float turnSpeed;
public string inputFront;
public string inputBack;
public string inputLeft;
public string inputRight;

  
    void Start()
    {
        
    }

  
    void Update()
    {
        if (Input.GetKey(inputFront)){
            transform.Translate(0,0,walkSpeed * Time.deltaTime);
        }
        if (Input.GetKey(inputBack)){
            transform.Translate(0,0,-walkSpeed * Time.deltaTime);
        }
        if (Input.GetKey(inputLeft)){
            transform.Rotate(0,-turnSpeed* Time.deltaTime,0);
        }
        if (Input.GetKey(inputRight)){
            transform.Rotate(0,turnSpeed* Time.deltaTime,0);
        }
    }
}
