using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sounplayerEndgame : MonoBehaviour
{
    [SerializeField] private string youWhat;
    void Start()
    {

        FindObjectOfType<AudioManager>().Play(youWhat);

    }

}
