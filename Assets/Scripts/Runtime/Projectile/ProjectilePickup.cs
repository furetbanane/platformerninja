using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePickup : MonoBehaviour
{
    public int pickupCount = 1;
    
    public AudioClip pickupAudioClip;
    [Range(0f, 1f)] public float pickupAudioClipVolume;

    private SoundPlayer soundPlayer = new SoundPlayer();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PlayerController>())
        {
            other.gameObject.GetComponent<PlayerController>().ProjectileCount += pickupCount;
            
            soundPlayer.PlaySoundOnGameObject(other.gameObject, pickupAudioClip, pickupAudioClipVolume);
            
            Destroy(gameObject);
            Debug.Log("Picked Up Projectile");
        }
    }
}
