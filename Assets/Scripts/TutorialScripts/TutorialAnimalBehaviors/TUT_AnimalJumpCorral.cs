﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TUT_AnimalJumpCorral : TUT_AnimalState
{
    private float xDesplazamiento = 0.8f;
    private float movementMult = 5;
    private float jumpHeight = 0.13f;

    public TUT_AnimalJumpCorral(sbyte _lifes, float _baseMovSpeed, float _shadowPosOffset, float _physicalCorralLimit, float _xToppedOffMove, float[] _yToppedOffMoveMinMax, float[] _corralLimits, bool _dead,
        GameObject _animal, GameObject _animalShadow, Animator _anim, AudioSource _audioSource, params AudioClip[] _audioClips) : base(_lifes, _baseMovSpeed, _shadowPosOffset, _physicalCorralLimit, _xToppedOffMove, _yToppedOffMoveMinMax, _corralLimits, _dead, _animal, _animalShadow, _anim, _audioSource, _audioClips)
    {
        endPos = new Vector2(startPos.x - xDesplazamiento, startPos.y);
    }


    protected sealed override void Enter()
    {
        //Debug.Log("JUMPCORRAL ENTER");
        anim.SetBool("Jump", true);
        CalculateAngle();
        base.Enter();
    }

    protected sealed override void Update()
    {
        base.Update();
        //animal.transform.Translate(movementDir  * movementMult * Time.deltaTime);
        ApplyParabolicMovement(true);

        if (animal.transform.position.x <= endPos.x)
        {
            animal.transform.position = new Vector2(animal.transform.position.x, startPos.y);
            nextState = new TUT_WalkToSpecificPosition(lifes, baseMovementSpeed, shadowPosOffset, physicCorralLimit, xToppedOffMove, yToppedOffMoveMinMax, corralLimits, dead,
                animal, animalShadow, anim, audioSource, new Vector2(-1, -0.72f), audioClips);
            stage = EVENT.EXIT;
        }

        SetInAirAnimalShadow();
    }


    protected override void Exit()
    {
        //Debug.Log("JUMPCORRAL EXIT");
        anim.SetBool("Jump", false);
        base.Exit();
    }
}
