using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayDoorOpenSound : MonoBehaviour
{
   // AudioSource audioSource;
   // [SerializeField] private AudioSource doorOpenSound;

    //private void Start() => audioSource = GameObject.FindGameObjectWithTag("AudioSource").GetComponent<AudioSource>();

    public void PlayOpenSound()
    {
        FindObjectOfType<AudioManager>().Play("puerta");

       /* if (doorOpenSound != null) 
        {
            //doorOpenSound.Play();            
        }*/
    }
}
