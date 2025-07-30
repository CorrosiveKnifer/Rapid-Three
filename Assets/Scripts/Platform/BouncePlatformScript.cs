using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncePlatformScript : PlatformScript
{
    [Header("Platform Settings")]
    public float BounceMagnitude = 500.0f;

    protected override void ApplyEffectToRigidBody2D(Rigidbody2D body)
    {
        if(body.linearVelocity.y <= 0)
            body.AddForce(transform.up * (-1 * body.mass * body.linearVelocity.y), ForceMode2D.Impulse);
    }
}
