using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject enemy;
    public Rigidbody rb;
    public float speed;
    public float damage;
    public bool isDurational;
    public ElementsEnum type;
    public bool autoAim;

    [SerializeField] private Material earthMaterial;
    [SerializeField] private Material fireMaterial;
    [SerializeField] private Material windMaterial;

    void Start()
    {
        var curMaterial = gameObject.GetComponent<Renderer>();
        if (type == ElementsEnum.Земля)
            curMaterial.sharedMaterial = earthMaterial;
        else if (type == ElementsEnum.Огонь) 
            curMaterial.sharedMaterial = fireMaterial;
        else if (type == ElementsEnum.Ветер)
            curMaterial.sharedMaterial = windMaterial;
    }

    void Update()
    {
        enemy = GameObject.Find("Root");
        if (transform.position.y >= 10)
            Destroy(gameObject);
        if(autoAim)
        {
            transform.position = Vector3.Lerp(transform.position, enemy.transform.position, speed * Time.deltaTime);
        }
        else
            rb.velocity = new Vector3(0, speed, 0);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Hitted smth");
        if(collision.collider.CompareTag("Enemy"))
        {
            Debug.Log("Hit");
            var dragon = collision.collider.GetComponentInParent<NewBehaviourScript>();
            dragon.GetDamage(damage,type,isDurational);
            Invoke("Destroy", 2.1f);
            gameObject.SetActive(false);

        }
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
