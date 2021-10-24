using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TUT_WalkToSpecificPosition : TUT_AnimalState
{
    Vector2 specificPosition;

   public TUT_WalkToSpecificPosition(sbyte _lifes, float _baseMovSpeed, float _shadowPosOffset, float _physicalCorralLimit, float _xToppedOffMove, float[] _yToppedOffMoveMinMax, float[] _corralLimits, bool _dead,
        GameObject _animal, GameObject _animalShadow, Animator _anim, AudioSource _audioSource, Vector2 _specificPosition, params AudioClip[] _audioClips) : base(_lifes, _baseMovSpeed, _shadowPosOffset, _physicalCorralLimit, _xToppedOffMove, _yToppedOffMoveMinMax, _corralLimits, _dead, _animal, _animalShadow, _anim, _audioSource, _audioClips)
    {
        specificPosition = _specificPosition;
    }

    protected override void Enter()
    {
        base.Enter();
    }

    protected override void Update()
    {
        base.Update();

        SetGroundedAnimalShadow();

        if(animal.transform.position.x <= physicCorralLimit)
        {
            if(TUT_AnimalState.isInsideCorral == true)
            {
                TUT_AnimalState.isInsideCorral = false;
                nextState = new TUT_AnimalJumpCorral(lifes, baseMovementSpeed, shadowPosOffset, physicCorralLimit, xToppedOffMove, yToppedOffMoveMinMax, corralLimits, dead,
               animal, animalShadow, anim, audioSource, audioClips);
                stage = EVENT.EXIT;
            }
        }

        if(Vector2.Distance(animal.transform.position, specificPosition) > 0.05f)
        {
            animal.transform.position = Vector2.MoveTowards(animal.transform.position, specificPosition, 0.5f * Time.deltaTime);
        } else
        {
            nextState =new TUT_AnimalSatDown(lifes, baseMovementSpeed, shadowPosOffset, physicCorralLimit, xToppedOffMove, yToppedOffMoveMinMax, corralLimits, dead,
                animal, animalShadow, anim, audioSource, audioClips);
            stage = EVENT.EXIT;
        }
    }

}
