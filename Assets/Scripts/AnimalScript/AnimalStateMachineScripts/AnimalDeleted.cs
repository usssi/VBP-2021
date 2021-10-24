using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalDeleted : AnimalState
{
    public AnimalDeleted(sbyte _lifes, float _baseMovSpeed, float _shadowPosOffset, float _physicalCorralLimit, float _xToppedOffMove, float[] _yToppedOffMoveMinMax, float[] _corralLimits, bool _dead,
        GameObject _animal, GameObject _animalShadow, Animator _anim, AudioSource _audioSource, params AudioClip[] _audioClips)
        : base(_lifes, _baseMovSpeed, _shadowPosOffset, _physicalCorralLimit, _xToppedOffMove, _yToppedOffMoveMinMax, _corralLimits, _dead, _animal, _animalShadow, _anim, _audioSource, _audioClips)
    { 
        state = STATE.DEAD;
        dead = true;
        SetEndPos();
    }

    protected override void Enter()
    {
        
        base.Enter();
    }


    protected override void Update()
    {
        animal.transform.Translate(Vector2.right * Time.deltaTime * 15);
        if (CheckIfReachEndPos())
        {
            nextState = new AnimalDead(lifes, baseMovementSpeed, shadowPosOffset, physicCorralLimit, xToppedOffMove, yToppedOffMoveMinMax, corralLimits, dead,
                animal, animalShadow, anim, audioSource, audioClips);
            stage = EVENT.EXIT;
        }
        
        SetInAirAnimalShadow();
    }

    protected override void Exit()
    {

        base.Exit();
    }

    private void SetEndPos()
    {
        endPos = new Vector2(startPos.x + 5, startPos.y);
    }
}
