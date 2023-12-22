using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    private bool paused = false;
    private DragonPicker dragonPicker;

    public GameObject pauseModal;

    private void Start()
    {
        dragonPicker = Camera.main.GetComponent<DragonPicker>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            paused = !paused;
            pauseModal.SetActive(paused);
            Time.timeScale = paused ? 0: 1;
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            dragonPicker.AddElementToHand(ElementsEnum.Earth);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            dragonPicker.AddElementToHand(ElementsEnum.Fire);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            dragonPicker.AddElementToHand(ElementsEnum.Wind);
        }
        else if (Input.GetMouseButtonDown(1))
        {
            dragonPicker.ExcSpell();
        }
    }
}
