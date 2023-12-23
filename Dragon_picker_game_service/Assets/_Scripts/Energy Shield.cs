using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnergyShield : MonoBehaviour
{
    private AudioSource audioSource;
    private EndModal endModal;
    private void Start()
    {
        endModal = GameObject.FindObjectOfType<EndModal>(true);
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!endModal.isActiveAndEnabled)
        {
            Vector3 mousePos2D = Input.mousePosition;
            mousePos2D.z = -Camera.main.transform.position.z;
            Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);
            Vector3 pos = transform.position;
            pos.x = mousePos3D.x;
            transform.position = pos;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Dragon Egg")
        {
            DragonPicker apScript = Camera.main.GetComponent<DragonPicker>();
            apScript.EggHitShield();

            audioSource.Play();

            Destroy(collision.gameObject);
        }
    }
}
