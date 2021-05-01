using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class fps : MonoBehaviour
{
    public int frames;
    float deltatime;

    void Update()
    {
        deltatime = 1 / Time.smoothDeltaTime;
        frames = (int)deltatime;

        this.GetComponent<TextMeshProUGUI>().text = frames.ToString();
    }
}
