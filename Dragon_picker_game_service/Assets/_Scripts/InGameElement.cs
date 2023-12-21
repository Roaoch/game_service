using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameElement : MonoBehaviour
{
    public void OnHitByPlayer()
    {
        Destroy(gameObject);
    }
}
