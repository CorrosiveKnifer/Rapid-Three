using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Michael Jordan
/// </summary>
public class GameManager : MonoBehaviour
{
    #region Singleton

    public static GameManager instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Cursor.lockState = CursorLockMode.Confined;
        }
        else
        {
            Debug.LogError("Second Instance of GameManager was created, this instance was destroyed.");
            Destroy(this);
        }
    }

    private void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }

    #endregion

    public Slider powerBar;

    //Volume Settings
    public static float MasterVolume { get; set; } = 1.0f;
    public static float SoundEffectVolume { get; set; } = 1.0f;
    public static float BackGroundVolume { get; set; } = 1.0f;

    public double GameTime = 0.0;

    private void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GameTime += Time.deltaTime;
    }

    //public void SetLife(float _life, float max = 100.0f)
    //{
    //    //lifeTimer.GetComponentInChildren<Text>().text = (Mathf.Floor(_life).ToString());
    //    //lifeTimer.GetComponent<Image>().fillAmount = _life / 100.0f;
    //    lifeBar.value = _life;
    //    lifeBar.maxValue = max;
    //}
    public void SetPower(float _power, float max = 100.0f)
    {
        powerBar.value = _power / max;
    }
}

