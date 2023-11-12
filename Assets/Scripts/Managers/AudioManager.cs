using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : UnitySingleton<AudioManager>
{
    // public AudioSource 
    public List<AudioChild> sounds = new List<AudioChild>();

    // Start is called before the first frame update
    void Start()
    {
        PlaySound("music");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlayAudioChild(string id, List<AudioChild> children) {
        foreach (AudioChild child in children)
        {
            if (child.id.ToLower() == id.ToLower())
            {
                child.Play();
                return;
            }
        }
    }

    public AudioSource GetSound(string id) {
        foreach (AudioChild child in sounds)
        {
            if (child.id.ToLower() == id.ToLower())
            {
                return child.sound;
            }
        }
        return null;
    }

    public void PlaySound(string id)
    {
        foreach (AudioChild child in sounds)
        {
            if (child.id.ToLower() == id.ToLower())
            {
                child.Play();
                return;
            }
        }
        Debug.Log($"{id}: Sound not found");
    }

    public void StopSound(string id)
    {
        foreach (AudioChild child in sounds)
        {
            if (child.id == id)
            {
                child.Stop();
                return;
            }
        }
    }
}

[System.Serializable]
public class AudioChild {
    public string id;
    public AudioSource sound;

    public AudioChild(string newId, AudioSource newSound, float newVolume) {
        id = newId;
        sound = newSound;
        sound.volume = newVolume;
    }

    public void Play()
    {
        this.sound.Play();
    }

    public void Stop()
    {
        this.sound.Stop();
    }
}