using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public Sound[] anouncer;
    public Sound[] panchitos;
    public AnimalSounds[] animalSounds;


    /*
    FindObjectOfType<AudioManager>().Play("");
    */


    void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.outputAudioMixerGroup = s.output;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.mute = s.mute;
        }

        foreach (Sound sa in anouncer)
        {
            sa.source = gameObject.AddComponent<AudioSource>();
            sa.source.clip = sa.clip;
            sa.source.outputAudioMixerGroup = sa.output;

            sa.source.volume = sa.volume;
            sa.source.pitch = sa.pitch;
            sa.source.loop = sa.loop;
            sa.source.mute = sa.mute;
        }

        foreach (Sound sb in panchitos)
        {
            sb.source = gameObject.AddComponent<AudioSource>();
            sb.source.clip = sb.clip;
            sb.source.outputAudioMixerGroup = sb.output;

            sb.source.volume = sb.volume;
            sb.source.pitch = sb.pitch;
            sb.source.loop = sb.loop;
            sb.source.mute = sb.mute;
        }
    }

    public void Play (string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);


        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found");
            return;
        }

        s.source.Play();

    }

    public void PlayA (string name)
    {
        Sound sa = Array.Find(anouncer, sound => sound.name == name);
        Debug.Log("sound is playing");

        if (sa == null)
        {
            Debug.LogWarning("Sound: " + name + " not found");
            return;
        }

        sa.source.Play();

    }

    public void PlayB(string name)
    {
        Sound sb = Array.Find(panchitos, sound => sound.name == name);
        Debug.Log("sound is playing");

        if (sb == null)
        {
            Debug.LogWarning("Sound: " + name + " not found");
            return;
        }

        sb.source.Play();

    }

    public void PlayAnimalSound(string animalName)
    {
        for (int i = 0; i < animalSounds.Length; i++)
        {
            if (animalSounds[i].name == animalName )
            {
                FindObjectOfType<AudioManager>().Play("");

            }
        }
    }

}

[System.Serializable] 
public class AnimalSounds
{

    public string name;
    public AudioClip[] clip;
    public AudioMixerGroup output;

    [Range(0f, 1f)]
    public float volume;
    [Range(.1f, 3f)]
    public float pitch;

    public bool loop;
    public bool mute;



    [HideInInspector]
    public AudioSource source;
}