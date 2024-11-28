using UnityEngine;

public class SoundPlayer
{
    private float destroyDelayOffset = 0.1f;
    
    public void PlaySoundOnGameObject(GameObject gameObj, AudioClip clip, float volume)
    {
        AudioSource source = gameObj.AddComponent<AudioSource>();

        source.clip = clip;
        source.volume = volume;
        
        source.loop = false;
        source.playOnAwake = false;
        
        source.Play();
        
        Object.Destroy(source, clip.length + destroyDelayOffset);
    }
    
    public void PlaySoundAtPosition(Transform position, AudioClip clip, float volume)
    {
        GameObject gamObj = Object.Instantiate(new GameObject("Sound"), position);
        
        AudioSource source = gamObj.AddComponent<AudioSource>();

        source.clip = clip;
        source.volume = volume;
        
        source.loop = false;
        source.playOnAwake = false;
        
        source.Play();
        
        Object.Destroy(gamObj, clip.length + destroyDelayOffset);
    }
}
