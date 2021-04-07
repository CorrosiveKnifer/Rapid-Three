
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyPlatformScript : PlatformScript
{
    [Header("Platform Settings")]
    public float StickMagnitude = 50.0f;

    protected override void ApplyEffectToRigidBody2D(Rigidbody2D body)
    {
        body.AddForce(Vector3.down * StickMagnitude);
        return;
    }
}
