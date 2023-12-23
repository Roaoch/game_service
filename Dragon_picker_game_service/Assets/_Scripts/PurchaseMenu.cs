using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class PurchaseMenu : MonoBehaviour
{
    private void OnEnable() => YandexGame.PurchaseSuccessEvent += SuccessPurchased;
    private void OnDisable() => YandexGame.PurchaseSuccessEvent -= SuccessPurchased;

    public void SuccessPurchased(string id)
    {
        switch (id)
        {
            case "Dimonds":
                YandexGame.savesData.levelUpPoints += 1;
                break;
        }
        YandexGame.SaveProgress();
    }
}
