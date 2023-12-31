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
    private TextMeshProUGUI pointsGT;

    private void Start()
    {
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
            levelGT = GameObject.Find("Level").GetComponent<TextMeshProUGUI>();
            pointsGT = GameObject.Find("Points").GetComponent<TextMeshProUGUI>();

            levelGT.text = $"Level: {YandexGame.savesData.level}";
            pointsGT.text = $"Points: {YandexGame.savesData.levelUpPoints}";
        }
        else
        {
            Debug.Log("Not authenticated");
            YandexGame.AuthDialog();
        }
    }
}
