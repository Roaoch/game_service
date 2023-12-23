using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ElementToLevel : MonoBehaviour
{
    [SerializeField] private int Level;
    public void OnHitByPlayer()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + Level);
    }
}
