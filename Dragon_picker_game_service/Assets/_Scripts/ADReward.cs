using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class ADReward : MonoBehaviour
{
    private void OnEnable() => YandexGame.RewardVideoEvent += GetReward;
    private void OnDisable() => YandexGame.RewardVideoEvent -= GetReward;

    void GetReward(int id)
    {

    }
}
