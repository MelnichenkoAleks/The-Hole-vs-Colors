using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUnlockHandler : MonoBehaviour
{
    [SerializeField] Button[] buttons;
    int unlokerLevelsNumber;

    private void Start()
    {
        if (!PlayerPrefs.HasKey("levelsUnlocked"))
        {
            PlayerPrefs.SetInt("levelsUnlocked", 1);
        }

        unlokerLevelsNumber = PlayerPrefs.GetInt("levelsUnlocked");

        for (int i=0; i< buttons.Length; i++)
        {
            buttons[i].interactable = false;
        }
    }

    private void Update()
    {
        unlokerLevelsNumber = PlayerPrefs.GetInt("levelsUnlocked");
        for (int i =0; i < unlokerLevelsNumber; i++)
        {
            buttons[i].interactable = true;
        }
    }
}
