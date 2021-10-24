using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TUT_AnimalSatDown : TUT_AnimalState
{
   public TUT_AnimalSatDown(sbyte _lifes, float _baseMovSpeed, float _shadowPosOffset, float _physicalCorralLimit, float _xToppedOffMove, float[] _yToppedOffMoveMinMax, float[] _corralLimits, bool _dead,
        GameObject _animal, GameObject _animalShadow, Animator _anim, AudioSource _audioSource, params AudioClip[] _audioClips) : base(_lifes, _baseMovSpeed, _shadowPosOffset, _physicalCorralLimit, _xToppedOffMove, _yToppedOffMoveMinMax, _corralLimits, _dead, _animal, _animalShadow, _anim, _audioSource, _audioClips)
    {

    }

    //No hace absolutamente nada

    protected override void Enter()
    {
        base.Enter();
        anim.SetBool("sit", true);
        Debug.Log("animal sentado");
    }

    protected override void Exit()
    {
        anim.SetBool("sit", false);
        base.Exit();
    }


}
