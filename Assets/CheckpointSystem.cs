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

    private int index = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            player.transform.up = checkpoints[index].transform.up;
            boulder.transform.up = checkpoints[index].transform.up;

            player.transform.position = checkpoints[index].transform.position + (checkpoints[index].transform.up * 1.5f);
            boulder.transform.position = checkpoints[index].transform.position + (checkpoints[index].transform.up * 1.5f);

            player.GetComponent<Rigidbody2D>().velocity = new Vector2();
            boulder.GetComponent<Rigidbody2D>().velocity = new Vector2();
        }
    }

    public void SetActiveCheckpoint(Checkpoint checkpoint)
    {
        for (int i = 0; i < checkpoints.Length; i++)
        {
            if(checkpoint == checkpoints[i])
            {
                index = i;
                break;
            }
        }
    }
}
