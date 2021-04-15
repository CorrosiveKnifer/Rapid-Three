using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathField : MonoBehaviour
{
    public Animator levelLoaderBlink;

    private bool flag = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            other.GetComponent<PlayerController>()?.ReleaseBoulder();
            StartCoroutine(CameraDelay(other.gameObject));
        }
        else
        {
            
            CheckpointSystem.instance.TeleportGamebjectToActive(other.gameObject);
        }
    }

    private IEnumerator CameraDelay(GameObject toTeleport)
    {
        if (!flag)
            yield return null;

        flag = true;
        
        //CameraController.instance.referenceObject = CheckpointSystem.instance.active.gameObject;

        levelLoaderBlink.SetTrigger("Start");

        //randomly pick an audio line
        int randNum = Random.Range(0, 10);
        string[] names = { "AnyLower", "ChopChop", "ForgotThis", "GoodEntertainment", "Haha1", "Haha2", "Haha3", "Haha4", "PitfulAttempt", "TimeUnkind"};
        Debug.Log(names[randNum]);
        GetComponent<AudioAgent>().PlaySoundEffect(names[randNum]);

        bool cond = true;
        do
        {
            cond = levelLoaderBlink.GetCurrentAnimatorStateInfo(0).IsName("Sleep_End") && levelLoaderBlink.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.0;
            yield return new WaitForEndOfFrame();
        } while (!cond);

        levelLoaderBlink.SetTrigger("Blink");
        CameraController.instance.referenceObject = toTeleport;

        CheckpointSystem.instance.TeleportGamebjectToActive(toTeleport);
        flag = false;
        yield return null;
    }
}
