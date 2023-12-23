using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using YG;

public class LevelUpMenu : MonoBehaviour
{
    private TextMeshProUGUI pointsGT;

    private void IncreasSmth(Action action, int cost=1)
    {
        if (YandexGame.savesData.levelUpPoints - cost >= 0)
        {
            action();
            YandexGame.savesData.levelUpPoints -= cost;
            YandexGame.SaveProgress();
            pointsGT.text = $"Points: {YandexGame.savesData.levelUpPoints}";
        }
    }

    void Start()
    {
        pointsGT = GameObject.Find("Points").GetComponent<TextMeshProUGUI>();
    }

    public void IncreasExpirience()
    {
        IncreasSmth(() => { YandexGame.savesData.expirienceScale += 0.2f; });
    }

    public void IncreasSpeed()
    {
        IncreasSmth(() => { YandexGame.savesData.speedScale += 0.2f; });
    }

    public void IncreasDamage()
    {
        IncreasSmth(() => { YandexGame.savesData.damageScale += 0.2f; });
    }

    public void IncreasShieldsCount()
    {
        IncreasSmth(() => { YandexGame.savesData.numEnegryShield += 1; }, 4);
    }

    public void IncreasElementsCapacity()
    {
        IncreasSmth(() => { YandexGame.savesData.elementsCopacity += 1; }, 2);
    }
}
