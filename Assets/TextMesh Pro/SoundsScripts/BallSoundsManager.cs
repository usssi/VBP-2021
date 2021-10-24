using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSoundsManager : MonoBehaviour
{
    //private AudioSource audioSource;
    //[SerializeField] private AudioClip ballRematada, ballSuelo, ballReboteAnimal, ballRebotePared, ballArmada, ballSuperBall;

    private void Awake()
    {
        //audioSource = GameObject.FindGameObjectWithTag("AudioSource").GetComponent<AudioSource>();
    }


    public void PlaySound(string _str)
    {
        switch (_str)
        {
            case "rematada":
                //audioSource.PlayOneShot(ballRematada);
                FindObjectOfType<AudioManager>().Play("bolaRemate");
                Debug.Log("rematada");

                break;

            case "suelo":
                //audioSource.PlayOneShot(ballSuelo);
                FindObjectOfType<AudioManager>().Play("bolaSuelo");
                Debug.Log("suelo");


                break;

            case "reboteAnimal":
                //audioSource.PlayOneShot(ballReboteAnimal);
                FindObjectOfType<AudioManager>().Play("bolaReboteAnimal");
                Debug.Log("reboteAnimal");


                break;

            case "rebotePared":
                //audioSource.PlayOneShot(ballRebotePared);
                FindObjectOfType<AudioManager>().Play("bolaRebotePared");
                Debug.Log("rebotePared");


                break;

            case "superBall":
                //audioSource.PlayOneShot(ballSuperBall);
                FindObjectOfType<AudioManager>().Play("bolaSuper");
                Debug.Log("superBall");


                break;
        }
    }
}
