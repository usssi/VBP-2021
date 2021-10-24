using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChadSoundsManager : MonoBehaviour
{
    private AudioSource audioSource;
    //[SerializeField] private AudioClip chadRecibe;

    private void Start()
    {
        audioSource = GameObject.FindGameObjectWithTag("AudioSource").GetComponent<AudioSource>();
        //Debug.Log(audioSource);
    }

    public void PlaySound(AudioClip sound)
    {
        audioSource.PlayOneShot(sound);
        Debug.Log("golpe o salto");
    }

    public void ReceibeSound()
    {
        //audioSource.PlayOneShot(chadRecibe); 
        FindObjectOfType<AudioManager>().Play("armaChad");

        Debug.Log("chad recibe");
    }
}
