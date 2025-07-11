using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class AchiveManager : MonoBehaviour
{
    public GameObject[] lockCharacter;
    public GameObject[] unlockCharacter;

    enum Achive { UnlockPotato, UnlockBean }
    Achive[] achives;
    void Awake()
    {
        achives = (Achive[])Enum.GetValues(typeof(Achive));
        if (!PlayerPrefs.HasKey("MyData")) { Init(); }
    }

    void Init()
    {
        PlayerPrefs.SetInt("MyData", 1);

        foreach (Achive achive in achives)
        {
            PlayerPrefs.SetInt(achive.ToString(), 0);
        }
    }

    void Start()
    {
        UnlockCharacter();
    }

    void UnlockCharacter()
    {
        for (int i = 0; i < lockCharacter.Length; i++)
        {
            string achiveName = achives[i].ToString();
            bool isUnlock = PlayerPrefs.GetInt(achiveName) == 1;
            lockCharacter[i].SetActive(!isUnlock);
            unlockCharacter[i].SetActive(isUnlock);
        }
    }
    // Update is called once per frame
    void LateUpdate()
    {
        foreach (Achive achive in achives)
        {
            CheckAchive(achive);
        }
    }

    void CheckAchive(Achive achive)
    {
        bool isAchive = false;

        switch (achive)
        {
            case Achive.UnlockPotato:
                isAchive = GameManager.instance.kill >= 10;
                break;
            case Achive.UnlockBean:
                isAchive = GameManager.instance.gameTime == GameManager.instance.maxGameTime;
                break;
        }
        if (isAchive && PlayerPrefs.GetInt(achive.ToString()) == 0)
        {
            PlayerPrefs.SetInt(achive.ToString(), 1);
        }
    }
}
