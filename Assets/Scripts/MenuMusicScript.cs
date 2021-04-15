using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuMusicScript : MonoBehaviour
{
    public Text winText;
    public GameObject menuText;

    private bool hasWon;
    // Start is called before the first frame update
    void Start()
    {
        hasWon = EndGoal.HasWon;

        if (EndGoal.HasWon)
        {
            EndGoal.HasWon = false;
            GetComponent<AudioAgent>().PlayBackground("Win");
        }
        else
        {
            GetComponent<AudioAgent>().PlayBackground("MainMenu", true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        winText.enabled = hasWon;
        menuText.SetActive(!hasWon);

        if (hasWon && GetComponent<AudioAgent>().IsAudioStopped("Win"))
        {
            GetComponent<AudioAgent>().PlayBackground("MainMenu", true);
            hasWon = false;
        }
    }
}
