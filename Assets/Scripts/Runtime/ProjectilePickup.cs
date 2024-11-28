using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePickup : MonoBehaviour
{
    public int pickupCount = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PlayerController>())
        {
            other.gameObject.GetComponent<PlayerController>().ProjectileCount += pickupCount;
            Destroy(gameObject);
            Debug.Log("Picked Up Projectile");
        }
    }
}
