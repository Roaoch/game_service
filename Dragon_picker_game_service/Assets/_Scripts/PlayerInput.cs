using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    private DragonPicker dragonPicker;
    private EndModal endModal;

    private void Start()
    {
        dragonPicker = Camera.main.GetComponent<DragonPicker>();
        endModal = GameObject.FindObjectOfType<EndModal>(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            endModal.ToggleEndModal(RootToEnd.InGame);
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
