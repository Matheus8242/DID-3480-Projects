﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float speed = 3;

    void Update()
    {
        transform.position = new Vector2(transform.position.x, Mathf.PingPong(Time.time, 3));
    }
}
