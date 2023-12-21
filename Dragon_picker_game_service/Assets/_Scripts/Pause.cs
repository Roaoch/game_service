using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    private bool paused = false;
    public GameObject pauseModal;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            paused = !paused;
            pauseModal.SetActive(paused);
            Time.timeScale = paused ? 0: 1;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
    }
}
