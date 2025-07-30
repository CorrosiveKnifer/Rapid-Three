
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyPlatformScript : PlatformScript
{
    [Header("Platform Settings")]
    public float StickMagnitude = 50.0f;

    protected override void ApplyEffectToRigidBody2D(Rigidbody2D body)
    {
        body.linearVelocity = new Vector2(body.linearVelocity.x * 0.8f, body.linearVelocity.y);
        body.AddForce(Vector3.down * StickMagnitude);
        return;
    }
}
