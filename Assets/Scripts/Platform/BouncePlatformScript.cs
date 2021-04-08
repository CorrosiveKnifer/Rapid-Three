using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncePlatformScript : PlatformScript
{
    [Header("Platform Settings")]
    public float BounceMagnitude = 500.0f;

    protected override void ApplyEffectToRigidBody2D(Rigidbody2D body)
    {
            body.AddForce(transform.up * BounceMagnitude);
    }
}
