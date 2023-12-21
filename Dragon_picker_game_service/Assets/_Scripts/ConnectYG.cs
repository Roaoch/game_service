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

    public TextMeshProUGUI bestScoreGT;
    public TextMeshProUGUI achivmentsListGT;

    private void Start()
    {
        if (YandexGame.SDKEnabled)
        {
            EnableYG();
        }

        bestScoreGT = GameObject.Find("Best Score").GetComponent<TextMeshProUGUI>();
        achivmentsListGT = GameObject.Find("AchivmentsList").GetComponent<TextMeshProUGUI>();
    }

    public void EnableYG()
    {
        if (YandexGame.auth)
        {
            Debug.Log("Authentication ok");
            bestScoreGT.text = $"Best Score: {YandexGame.savesData.bestScore}";
            achivmentsListGT.text = string.Join("\n", YandexGame.savesData.achivments);
        }
        else
        {
            Debug.Log("Not authenticated");
            YandexGame.AuthDialog();
        }
    }
}
