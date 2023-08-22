using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OffEffects : MonoBehaviour
{
    public Sprite onEffects;
    public Sprite offEffects;

    public Image EffectButton;
    public bool isOn;
    public AudioSource ad;

    void Start()
    {
        isOn = true;
    }

    void Update()
    {
        if (PlayerPrefs.GetInt("effect") == 0)
        {
            EffectButton.GetComponent<Image>().sprite = onEffects;
            ad.enabled = true;
            isOn = true;
        }
        else if (PlayerPrefs.GetInt("effect") == 1)
        {
            EffectButton.GetComponent<Image>().sprite = offEffects;
            ad.enabled = false;
            isOn = false;
        }
    }

    public void offSound()
    {
        if (!isOn)
        {
            PlayerPrefs.SetInt("effect", 0);
        }
        else if (isOn)
        {
            PlayerPrefs.SetInt("effect", 1);
        }
    }
}
