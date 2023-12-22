using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using YG;

public enum RootToEnd
{
    InGame,
    DragonIsDead,
    PlayerIsDead
}

public class EndModal : MonoBehaviour
{
    private TextMeshProUGUI endModalText;

    public void LoadMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void ReloadScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ToggleEndModal(RootToEnd rootToEnd)
    {
        if (endModalText == null)
        {
            endModalText = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        }

        gameObject.SetActive(!gameObject.activeSelf);
        Time.timeScale = gameObject.activeSelf ? 0 : 1;

        if (rootToEnd is RootToEnd.InGame)
        {
            endModalText.text = "Pause";
        }
        else if (rootToEnd is RootToEnd.PlayerIsDead)
        {
            endModalText.text = "You Die";
        }
        else if (rootToEnd is RootToEnd.DragonIsDead)
        {
            var newLevel = (int)(YandexGame.savesData.expirience / YandexGame.savesData.toNewLevel);
            if (newLevel >= YandexGame.savesData.level + 1)
            {
                YandexGame.savesData.levelUpPoints += newLevel - YandexGame.savesData.level;
                YandexGame.savesData.level = newLevel;
                YandexGame.SaveProgress();
                endModalText.text = $"You acquire {newLevel} level!";
            }
            else
            {
                endModalText.text = $"need {YandexGame.savesData.level * YandexGame.savesData.toNewLevel - YandexGame.savesData.expirience}exp to {++newLevel} level";
            }
        }
    }
}
