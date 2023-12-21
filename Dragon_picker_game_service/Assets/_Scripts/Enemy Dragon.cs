using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] private GameObject dragonEggPrefab;
    [SerializeField] private float speed = 1f;
    [SerializeField] private float timeBeforeFirstEggDrop = 1f;
    [SerializeField] private float timeBetweenEggDrops = 1f;
    [SerializeField] private float leftRightDistance = 10f;
    [SerializeField] private float chanceDirection = 0.1f;
    [SerializeField] private float health;
    [SerializeField] private ElementsEnum element;

    private float Damage;
    private ElementsEnum Type;

    void Start()
    {
        Invoke("DropEgg", timeBeforeFirstEggDrop);
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
        pos.x += speed * Time.deltaTime;
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
        Damage = damage;
        Type = type;
        if (isDurational)
        {
            DoDamage();
            Invoke("DoDamage",1);
            Invoke("DoDamage", 2);
        }
        else
            DoDamage();
    }
    public void DoDamage()
    {
        if (element.Equals(Type))
            health -= Damage;
        else
        {
            Debug.Log("Unequal types");
            if (element is ElementsEnum.Fire && Type is ElementsEnum.Wind)
                health -= Damage * 0.8f;
            if (element is ElementsEnum.Fire && Type is ElementsEnum.Earth)
                health -= Damage * 1.2f;
            if (element is ElementsEnum.Earth && Type is ElementsEnum.Fire)
                health -= Damage * 0.8f;
            if (element is ElementsEnum.Earth && Type is ElementsEnum.Wind)
                health -= Damage * 1.2f;
            if (element is ElementsEnum.Wind && Type is ElementsEnum.Earth)
                health -= Damage * 0.8f;
            if (element is ElementsEnum.Wind && Type is ElementsEnum.Fire)
                health -= Damage * 1.2f;
        }
    }
}
