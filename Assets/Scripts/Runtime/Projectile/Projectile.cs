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
    public float lifeTime;

    public LayerMask collisionCheckLayerMask;
    
    public AudioClip hitAudioClip;
    [Range(0f, 1f)] public float hitAudioClipVolume;

    private SoundPlayer soundPlayer = new SoundPlayer();

    private Vector2 previousPos;

    private bool hasHit = false;

    private void Start()
    {
        Destroy(gameObject, lifeTime);

        previousPos = transform.position;
    }
    
    private void Update()
    {
        if (!hasHit)
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
            transform.Rotate(new Vector3(0, 0, Vector3.right.x * rotateSpeed * Time.deltaTime));

            Vector3 fromPosition = previousPos;
            Vector3 toPosition = transform.position;
            Vector3 direction = toPosition - fromPosition;

            RaycastHit2D hit = Physics2D.Raycast(previousPos, direction, Vector2.Distance(previousPos, transform.position), collisionCheckLayerMask);

            if (hit)
            {
                Hit(hit.normal);
            }

            previousPos = transform.position;
        }
    }

    private void Hit(Vector3 normal)
    {
		soundPlayer.PlaySoundOnGameObject(gameObject, hitAudioClip, hitAudioClipVolume);

		GameObject particle = Instantiate(particlePrefab, transform.position, Quaternion.identity);
        particle.transform.up = normal;
		Destroy(particle, particleLifeTime);
		Destroy(gameObject);

        hasHit = true;
	}
}
