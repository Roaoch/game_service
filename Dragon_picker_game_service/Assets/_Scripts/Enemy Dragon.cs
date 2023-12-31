using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using YG;

public class NewBehaviourScript : MonoBehaviour
{
    public ElementsEnum element;

    [SerializeField] private GameObject dragonEggPrefab;
    [SerializeField] private float speed = 1f;
    [SerializeField] private float timeBeforeFirstEggDrop = 1f;
    [SerializeField] private float timeBetweenEggDrops = 1f;
    [SerializeField] private float leftRightDistance = 10f;
    [SerializeField] private float chanceDirection = 0.1f;
    [SerializeField] private float health;
    [SerializeField] private float expirience = 60;

    private float damageDamage;
    private float speedMultiplier = 1;
    private ElementsEnum damageType;
    private Slider dragonHealthGT;
    private EndModal endModal;
    private float healthMax;
    private void ReturnSpeedMultiplier()
    {
        speedMultiplier = 1;
    }

    void Start()
    {
        Invoke("DropEgg", timeBeforeFirstEggDrop);
        dragonHealthGT = GameObject.Find("Health").GetComponent<Slider>();
        endModal = GameObject.FindObjectOfType<EndModal>(true);
        healthMax = health;
    }

    void DropEgg()
    {
        Vector3 eggSpawnPoint = new Vector3(0f, 5f, 0f);
        GameObject egg = Instantiate<GameObject>(dragonEggPrefab, transform.position + eggSpawnPoint, transform.rotation);
        Invoke("DropEgg", timeBetweenEggDrops);
    }

    void Update()
    {
        Vector3 pos = transform.position;
        pos.x += speed * Time.deltaTime * speedMultiplier;
        transform.position = pos;

        if (pos.x < -leftRightDistance)
            speed = Mathf.Abs(speed);
        else if (pos.x > leftRightDistance)
            speed = -Mathf.Abs(speed);
    }

    private void FixedUpdate()
    {
        if (UnityEngine.Random.value < chanceDirection)
        {
            speed *= -1;
        }
    }

    public void GetDamage(float damage, ElementsEnum type, bool isDurational)
    {
        damageDamage = damage;
        damageType = type;
        if (isDurational)
        {
            DoDamage();
            Invoke("DoDamage", 1);
            Invoke("DoDamage", 2);
        }
        else
            DoDamage();
    }
    public void DoDamage()
    {
        if (element.Equals(damageType))
            health -= damageDamage;
        else
        {
            Debug.Log("Unequal types");
            if (element is ElementsEnum.����� && damageType is ElementsEnum.�����)
                health -= damageDamage * 0.8f;
            if (element is ElementsEnum.����� && damageType is ElementsEnum.�����)
                health -= damageDamage * 1.2f;
            if (element is ElementsEnum.����� && damageType is ElementsEnum.�����)
                health -= damageDamage * 0.8f;
            if (element is ElementsEnum.����� && damageType is ElementsEnum.�����)
                health -= damageDamage * 1.2f;
            if (element is ElementsEnum.����� && damageType is ElementsEnum.�����)
                health -= damageDamage * 0.8f;
            if (element is ElementsEnum.����� && damageType is ElementsEnum.�����)
                health -= damageDamage * 1.2f;
        }
        
        if (health <= 0)
        {
            YGSaveData();
            endModal.ToggleEndModal(RootToEnd.DragonIsDead);
            dragonHealthGT.value = 0;
            Destroy(gameObject);
        } 
        else
        {
            dragonHealthGT.value = 1 - (healthMax-health)/healthMax;
        }
    }

    public void TempIncressSpeedMultiplier()
    {
        speedMultiplier = 0;
        Invoke("ReturnSpeedMultiplier", 3);
    }

    public void YGSaveData()
    {
        YandexGame.savesData.expirience += expirience * YandexGame.savesData.expirienceScale;
        YandexGame.SaveProgress();
    }
}
