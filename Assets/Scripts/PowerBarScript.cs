using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerBarScript : MonoBehaviour
{
    public Image Bar;

    // Update is called once per frame
    void Update()
    {
        Bar.color = Color.Lerp(Color.green, Color.red, GetComponent<Slider>().value);
    }
}
