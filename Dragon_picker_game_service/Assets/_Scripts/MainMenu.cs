using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject earthLevel;
    [SerializeField] private GameObject fireLevel;
    [SerializeField] private GameObject windLevel;
    [SerializeField] private GameObject playGameButton;
    [SerializeField] private GameObject achivmentButton;
    [SerializeField] private GameObject optionButton;
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void SelectLevel()
    {
        playGameButton.SetActive(false);
        achivmentButton.SetActive(false);
        optionButton.SetActive(false);
        earthLevel.SetActive(true);
        fireLevel.SetActive(true);
        windLevel.SetActive(true);     
    }
}
