using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkInsideCorral : AnimalState
{


   public WalkInsideCorral(sbyte _lifes, float _baseMovSpeed, float _shadowPosOffset, float _physicalCorralLimit, float _xToppedOffMove, float[] _yToppedOffMoveMinMax, float[] _corralLimits, bool _dead,
        GameObject _animal, GameObject _animalShadow, Animator _anim, AudioSource _audioSource, params AudioClip[] _audioClips)
        : base(_lifes, _baseMovSpeed, _shadowPosOffset, _physicalCorralLimit, _xToppedOffMove, _yToppedOffMoveMinMax, _corralLimits, _dead, _animal, _animalShadow, _anim, _audioSource, _audioClips)
    {
        state = STATE.WALKINSIDECORRAL;
   }

    protected sealed override void Enter()
    {
        anim.SetFloat("animOffset", Random.Range(0f, 1f));
        //Debug.Log("WALKINSIDECORRAL ENTER");
        SetMovementDirection(true);
        base.Enter();
    }

    protected sealed override void Update()
    {
        base.Update();
        ApplyMovement();
        DetectCorralLimit();
        SetGroundedAnimalShadow();
    }

    protected sealed override void Exit()
    {
        //Debug.Log("WALKINSIDECORRAL EXIT");
        base.Exit();
    }


    void DetectCorralLimit()
    {
        if(animal.transform.position.x <= physicCorralLimit)
        {
            nextState = new JumpCorral(lifes, baseMovementSpeed, shadowPosOffset, physicCorralLimit, xToppedOffMove, yToppedOffMoveMinMax, corralLimits, dead,
                animal, animalShadow, anim, audioSource, audioClips);
            stage = EVENT.EXIT;
        }
    }


    //Para cambiar de a otro estado estando en WalkInsideCorral tenemos 2 condiciones:
    /*Llegar al límite del corral, lo que lo hace pasar a JumpCorral.
      Ser rematado, lo que lo hace pasar a AnimalToppedOff.     
     */


}
