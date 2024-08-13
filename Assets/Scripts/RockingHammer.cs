using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockingHammer : MonoBehaviour
{
    public float speed = 2.0f;
    public float maxAngle = 30.0f;

    private void Update()
    {
        float angle = maxAngle * Mathf.Sin(Time.time * speed);
        transform.localRotation = Quaternion.Euler(0, 0, angle);
    }
}
