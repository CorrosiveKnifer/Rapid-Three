using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerImage : MonoBehaviour
{
    public Sprite[] m_digits;

    public Image[] m_DigitDisplay;

    private bool IsActive = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateText(int number)
    {
        string text = number.ToString();

        bool cond = number >= Mathf.Pow(10, m_DigitDisplay.Length - 1);
        SetActive(cond && text.Length == m_DigitDisplay.Length);
        
        if (!IsActive)
            return;

        for(int i = 0; i < text.Length; i++)
        {
            string digit = ""+text.ToCharArray()[i];
            int index = int.Parse(digit);

            m_DigitDisplay[i].sprite = m_digits[index];
        }
    }

    private void SetActive(bool val)
    {
        IsActive = val;

        foreach (var item in m_DigitDisplay)
        {
            item.enabled = val;
        }
    }
}
