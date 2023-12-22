using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum RootToEnd
{
    InGame,
    DragonIsDead,
    PlayerIsDead
}

public class EndModal : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

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
        gameObject.SetActive(!gameObject.activeSelf);
        Time.timeScale = gameObject.activeSelf ? 0 : 1;
    }
}
