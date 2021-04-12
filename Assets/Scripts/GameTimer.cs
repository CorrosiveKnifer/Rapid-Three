using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Michael Jordan
/// </summary>
public class GameTimer : MonoBehaviour
{
    public int m_time;
    public float m_secondCount = 0.0f;

    public GameObject m_single;
    public GameObject m_double;
    public GameObject m_triple;

    public bool m_countDown = false;
    public float sizeOfOneSecond = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        m_time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        m_secondCount += Time.deltaTime;
        if (m_secondCount > sizeOfOneSecond)
        {
            m_secondCount -= sizeOfOneSecond; //Adjust for lag
            m_time = (m_countDown) ? m_time - 1 : m_time + 1;
            Mathf.Clamp(m_time, 0, 999);
        }


        m_single.GetComponent<TimerImage>().UpdateText(m_time);
        m_double.GetComponent<TimerImage>().UpdateText(m_time);
        m_triple.GetComponent<TimerImage>().UpdateText(m_time);
    }
}
