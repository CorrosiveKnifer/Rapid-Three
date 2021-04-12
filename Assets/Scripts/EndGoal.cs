using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Rachael work
/// </summary>
public class EndGoal : MonoBehaviour
{
    public LevelLoader level;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Boulder"))
        {
            level.LoadNextLevel();
        }
    }
}
