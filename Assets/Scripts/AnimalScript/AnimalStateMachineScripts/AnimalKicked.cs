using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalKicked : AnimalState
{
    private float animalKickedSpeed = 2.5f;

    public AnimalKicked(sbyte _lifes, float _baseMovSpeed, float _shadowPosOffset, float _physicalCorralLimit, float _xToppedOffMove, float[] _yToppedOffMoveMinMax, float[] _corralLimits, bool _dead,
        GameObject _animal, GameObject _animalShadow, Animator _anim, AudioSource _audioSource, params AudioClip[] _audioClips)
        : base(_lifes, _baseMovSpeed, _shadowPosOffset, _physicalCorralLimit, _xToppedOffMove, _yToppedOffMoveMinMax, _corralLimits, _dead, _animal, _animalShadow, _anim, _audioSource, _audioClips)
    {
        state = STATE.KICKED;
        parabolicSpeed = animalKickedSpeed;
        kicked = true;
        SetEndPos();
    }

    protected sealed override void Enter()
    {
        anim.SetBool("toppedOff", true);
        CalculateAngle();
        base.Enter();
    }


    protected sealed override void Update()
    {
        base.Update();
        ApplyParabolicMovement(false);
        ApplyRotation();
        if (CheckIfReachEndPos())
        {
            nextState = new WalkInsideCorral(lifes, baseMovementSpeed, shadowPosOffset, physicCorralLimit, xToppedOffMove, yToppedOffMoveMinMax, corralLimits, dead,
                animal, animalShadow, anim, audioSource, audioClips);
            stage = EVENT.EXIT;
        }

        SetInAirAnimalShadow();

        
    }


    protected sealed override void Exit()
    {
        anim.SetBool("toppedOff", false);
        animal.transform.rotation = Quaternion.identity;
        base.Exit();
    }


    private void SetEndPos()
    {
        endPos = new Vector2(Random.Range(corralLimits[0], corralLimits[1]), Random.Range(corralLimits[2], corralLimits[3]));
    }
}
