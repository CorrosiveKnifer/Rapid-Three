using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public void PlayRandomAudio()
    {
        string[] names = GetComponent<AudioAgent>().AudioLibrary.Keys.ToArray();
        int randNum = Random.Range(0, names.Length);

        StartCoroutine(GetComponent<AudioAgent>().PlaySoundEffectSolo(names[randNum]));
    }

    public IEnumerator CameraDelay(GameObject toTeleport)
    {
        if (!flag)
            yield return null;

        flag = true;
        
        //CameraController.instance.referenceObject = CheckpointSystem.instance.active.gameObject;
        levelLoaderBlink.SetTrigger("Start");
        levelLoaderBlink.SetBool("Blink", false);
        //randomly pick an audio line
        PlayRandomAudio();

        bool cond = true;
        do
        {
            cond = levelLoaderBlink.GetCurrentAnimatorStateInfo(0).IsName("Sleep_End") && levelLoaderBlink.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.0;
            yield return new WaitForEndOfFrame();
        } while (!cond);

        levelLoaderBlink.SetBool("Blink", true);
        CameraController.instance.referenceObject = toTeleport;

        CheckpointSystem.instance.TeleportGamebjectToActive(toTeleport);
        flag = false;
        yield return null;
    }
}
