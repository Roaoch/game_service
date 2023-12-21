using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        enemy = GameObject.Find("Root");
        if (transform.position.y >= 10)
            Destroy(gameObject);
        if(autoAim)
        {
            transform.position = Vector3.Lerp(transform.position, enemy.transform.position, speed * Time.fixedDeltaTime);
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
