using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonEgg : MonoBehaviour
{
    [SerializeField] private static float bottomY = -30;
    private AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        ParticleSystem ps = GetComponent<ParticleSystem>();
        var em = ps.emission;
        em.enabled = true;

        Renderer rend;
        rend = GetComponent<Renderer>();
        rend.enabled = false;
    }
    void Update()
    {
        if(transform.position.y < bottomY)
        {
            DragonPicker apScript = Camera.main.GetComponent<DragonPicker>();
            apScript.EggHitGround();
            audioSource.Play();

            Destroy(gameObject);           
        }
    }
}
