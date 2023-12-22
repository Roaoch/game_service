using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using YG;

public class ConnectYG : MonoBehaviour
{
    private void OnEnable() => YandexGame.GetDataEvent += EnableYG;
    private void OnDisable() => YandexGame.GetDataEvent -= EnableYG;

    private TextMeshProUGUI levelGT;

    private void Start()
    {
        levelGT = GameObject.Find("Level").GetComponent<TextMeshProUGUI>();
        if (YandexGame.SDKEnabled)
        {
            EnableYG();
        }
    }

    public void EnableYG()
    {
        if (YandexGame.auth)
        {
            Debug.Log("Authentication ok");
            levelGT.text = $"Level: {YandexGame.savesData.level}";
        }
        else
        {
            Debug.Log("Not authenticated");
            YandexGame.AuthDialog();
        }
    }
}
