using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject particlePrefab;
    public float particleLifeTime;
    
    public float speed;
    public float rotateSpeed;
    public float collisionRadius;
    public float lifeTime;

    public LayerMask collisionCheckLayerMask;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }
    
    private void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
        transform.Rotate(new Vector3(0, 0, Vector3.right.x * rotateSpeed * Time.deltaTime));

        if (Physics2D.OverlapCircle(transform.position, collisionRadius, collisionCheckLayerMask))
        {
            GameObject particle = Instantiate(particlePrefab, transform.position, Quaternion.identity);
            Destroy(particle, particleLifeTime);
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, collisionRadius);
    }
}
