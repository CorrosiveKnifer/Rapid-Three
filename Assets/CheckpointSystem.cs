using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointSystem : MonoBehaviour
{
    #region Singleton

    public static CheckpointSystem instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Cursor.lockState = CursorLockMode.Confined;
        }
        else
        {
            Debug.LogError("Second Instance of CheckpointSystem was created, this instance was destroyed.");
            Destroy(this);
        }
    }

    private void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }

    #endregion

    public GameObject player;
    public GameObject boulder;

    public Checkpoint[] checkpoints;

    private Checkpoint active;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.L) && active != null)
        {
            player.transform.up = active.transform.up;
            boulder.transform.up = active.transform.up;

            player.transform.position = active.transform.position + (active.transform.up * 1.5f);
            boulder.transform.position = active.transform.position + (active.transform.up * 1.5f);

            player.GetComponent<Rigidbody2D>().velocity = new Vector2();
            boulder.GetComponent<Rigidbody2D>().velocity = new Vector2();
        }
    }

    public void SetActiveCheckpoint(Checkpoint checkpoint)
    {
        active = checkpoint;
    }
}
