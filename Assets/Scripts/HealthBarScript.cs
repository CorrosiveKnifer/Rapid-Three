using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        GetComponentInChildren<Text>().text = (Mathf.FloorToInt(GetComponent<Slider>().value)).ToString();
    }
}
