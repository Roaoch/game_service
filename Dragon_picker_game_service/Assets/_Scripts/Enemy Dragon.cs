using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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

    private float damageDamage;
    private float speedMultiplier = 1;
    private ElementsEnum damageType;
    private TextMeshProUGUI dragonHealthGT;

    private void ReturnSpeedMultiplier()
    {
        speedMultiplier = 1;
    }

    void Start()
    {
        Invoke("DropEgg", timeBeforeFirstEggDrop);
        dragonHealthGT = GameObject.Find("DragonHealth").GetComponent<TextMeshProUGUI>();
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
        if (health <= 0)
            Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        if (Random.value < chanceDirection)
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
            if (element is ElementsEnum.Fire && damageType is ElementsEnum.Wind)
                health -= damageDamage * 0.8f;
            if (element is ElementsEnum.Fire && damageType is ElementsEnum.Earth)
                health -= damageDamage * 1.2f;
            if (element is ElementsEnum.Earth && damageType is ElementsEnum.Fire)
                health -= damageDamage * 0.8f;
            if (element is ElementsEnum.Earth && damageType is ElementsEnum.Wind)
                health -= damageDamage * 1.2f;
            if (element is ElementsEnum.Wind && damageType is ElementsEnum.Earth)
                health -= damageDamage * 0.8f;
            if (element is ElementsEnum.Wind && damageType is ElementsEnum.Fire)
                health -= damageDamage * 1.2f;
        }
        dragonHealthGT.text = health.ToString();
    }

    public void TempIncressSpeedMultiplier()
    {
        speedMultiplier = 0;
        Invoke("ReturnSpeedMultiplier", 3);
    }
}
