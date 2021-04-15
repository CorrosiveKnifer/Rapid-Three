using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public bool IsFlagUp { get; private set; } = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponentInChildren<Animator>().SetBool("IsRaised", IsFlagUp);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Boulder"))
        {
            if(!IsFlagUp)
            {
                IsFlagUp = true;
                CheckpointSystem.instance.SetActiveCheckpoint(this);
            }
        }
    }
}
