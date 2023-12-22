using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCast : MonoBehaviour
{
    private Camera _camera;
    private Ray ray;
    private RaycastHit hit;

    void Start()
    {
        _camera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "Element Fire" ||
                    hit.collider.tag == "Element Earth" ||
                    hit.collider.tag == "Element Wind"
                ) 
                {
                    if (hit.collider.GetComponent<InGameElement>() is not null)
                    {
                        hit.collider.GetComponent<InGameElement>().OnHitByPlayer();
                        Camera.main.GetComponent<DragonPicker>().ElementHit(hit.collider.tag);
                    }
                    else
                        hit.collider.GetComponent<ElementToLevel>().OnHitByPlayer();
                }
            }
        }
    }
}
