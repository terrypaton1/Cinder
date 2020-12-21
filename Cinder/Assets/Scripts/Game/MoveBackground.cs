﻿using UnityEngine;

public class MoveBackground : MonoBehaviour
{
    private Vector3 position;
    private float rotation;

    private float range = .2f;

    protected void Start()
    {
        position = transform.position;
    }

    protected void Update()
    {
        float radian = Mathf.Deg2Rad * rotation;
        var newPosition = position;
        newPosition.x += Mathf.Cos(radian) * range;
        newPosition.y += Mathf.Sin(radian) * range;
        transform.position = newPosition;
        rotation += Time.deltaTime * 10;
    }
}