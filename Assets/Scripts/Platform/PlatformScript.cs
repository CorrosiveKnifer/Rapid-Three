using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlatformScript : MonoBehaviour
{
    protected List<Rigidbody2D> bodiesWithin;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        bodiesWithin = new List<Rigidbody2D>();
    }

    // Update is called once per frame
    protected virtual void FixedUpdate()
    {
        foreach (var body in bodiesWithin)
        {
            if (body != null)
            {
                ApplyEffectToRigidBody2D(body);
            }
        }
    }

    protected abstract void ApplyEffectToRigidBody2D(Rigidbody2D body);

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        bodiesWithin.Add(other.gameObject.GetComponentInChildren<Rigidbody2D>());
    }

    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        bodiesWithin.Remove(other.gameObject.GetComponentInChildren<Rigidbody2D>());
    }
}
