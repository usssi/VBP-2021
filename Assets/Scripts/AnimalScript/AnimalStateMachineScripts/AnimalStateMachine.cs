﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalStateMachine : MonoBehaviour
{
    [Header("AnimalVars")]
    [SerializeField] sbyte lifes;
    [SerializeField] float baseMovementSpeed;
    [SerializeField] float shadowPosOffset;
    private float physicalCorralLimit;
    [SerializeField] float xToppedOffMove;
    [SerializeField] float[] yToppedOffMoveMinMax = new float[2];
    [SerializeField] private float[] corralLimits = new float[4];        //0: -x      1: +x        2: -y         3: +y
    [SerializeField] private Vector2 lifesPosOffset;


    [Header("AnimalComponents")]
    private GameObject animalObj;
    private GameObject animalShadow;
    private GameObject currentLife;
    [SerializeField] private GameObject[] animalLifesObj;
    private Animator anim;
    private AudioSource audioSource;
    [SerializeField] private AudioClip[] audioClips;


    [Header("AnimalStates")]
    private AnimalState currentState;
    public AnimalState CurrentState { get => currentState; }


    private void Start()
    {
        animalObj = gameObject;
        anim = GetComponent<Animator>();
        audioSource = GameObject.FindGameObjectWithTag("AudioSource").GetComponent<AudioSource>();
        physicalCorralLimit = GameObject.Find("MapLimits/PhysicalLimits/LeftCorralPhysicalLimit").transform.position.x;
        corralLimits[0] = GameObject.Find("/Limits/CorralLimits/CorralLeftLimit").transform.position.x;
        corralLimits[1] = GameObject.Find("/Limits/CorralLimits/CorralRightLimit").transform.position.x;
        corralLimits[2] = GameObject.Find("/Limits/CorralLimits/CorralBottomLimit").transform.position.y;
        corralLimits[3] = GameObject.Find("/Limits/CorralLimits/CorralTopLimit").transform.position.y;
        animalShadow = transform.GetChild(0).gameObject;

        currentState = new WalkInsideCorral(lifes, baseMovementSpeed, shadowPosOffset, physicalCorralLimit, xToppedOffMove, yToppedOffMoveMinMax, corralLimits, false,
           animalObj, animalShadow, anim, audioSource, audioClips);
        currentLife = animalLifesObj[lifes];
        currentLife.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (lifes != currentState.Lifes)
            SetCurrentLife();

        currentState = currentState.Process();
        currentLife.transform.position = (Vector2)this.transform.position + lifesPosOffset;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;

        Gizmos.DrawRay((Vector2)transform.position, lifesPosOffset);
    }

    void SetCurrentLife()
    {
        currentLife.SetActive(false);
        lifes = currentState.Lifes;
        currentLife = animalLifesObj[lifes];
        currentLife.SetActive(true);
    }
    
}
