using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformScript : PlatformScript
{
    [Header("Platform Settings")]
    public GameObject targetLocation;
    public float speed;

    private Vector3 initialPosition;
    private bool IsMovingForward = true;
    private Vector3 direction;

    protected override void Start()
    {
        base.Start();
        initialPosition = transform.position;
        direction = (targetLocation.transform.position - transform.position).normalized;
    }

    protected override void FixedUpdate()
    {
        float forward = (IsMovingForward) ? 1 : -1;
        transform.position += forward * direction * speed * Time.fixedDeltaTime;

        if(IsMovingForward && Vector3.Distance(transform.position, targetLocation.transform.position) <= 0.5f)
        {
            IsMovingForward = false;
        }

        if (!IsMovingForward && Vector3.Distance(transform.position, initialPosition) <= 0.5f)
        {
            IsMovingForward = true;
        }

        base.FixedUpdate();
    }

    protected override void ApplyEffectToRigidBody2D(Rigidbody2D body)
    {
        float forward = (IsMovingForward) ? 1 : -1;
        body.transform.position += forward * direction *speed * Time.fixedDeltaTime;
    }
}
