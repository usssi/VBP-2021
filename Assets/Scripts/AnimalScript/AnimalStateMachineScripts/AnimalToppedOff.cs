using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalToppedOff : AnimalState
{
    public AnimalToppedOff(sbyte _lifes, float _baseMovSpeed, float _shadowPosOffset, float _physicalCorralLimit, float _xToppedOffMove, float[] _yToppedOffMoveMinMax, float[] _corralLimits, bool _dead,
        GameObject _animal, GameObject _animalShadow, Animator _anim, AudioSource _audioSource, params AudioClip[] _audioClips)
        : base(_lifes, _baseMovSpeed, _shadowPosOffset, _physicalCorralLimit, _xToppedOffMove, _yToppedOffMoveMinMax, _corralLimits, _dead, _animal, _animalShadow, _anim, _audioSource, _audioClips)
    {
        state = STATE.TOPPEDOFF;
        SetEndPos();
        //Debug.Log("EndPos: " + endPos);
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
            if(lifes > 0)
            {
                if (animal.transform.position.x >= physicCorralLimit - 0.6f)
                {
                    nextState = new WalkInsideCorral(lifes, baseMovementSpeed, shadowPosOffset, physicCorralLimit, xToppedOffMove, yToppedOffMoveMinMax, corralLimits, dead,
                        animal, animalShadow, anim, audioSource, audioClips);
                    stage = EVENT.EXIT;
                }
                else
                {
                    nextState = new WalkOutsideCorral(lifes, baseMovementSpeed, shadowPosOffset, physicCorralLimit, xToppedOffMove, yToppedOffMoveMinMax, corralLimits, dead,
                         animal, animalShadow, anim, audioSource, audioClips);

                    stage = EVENT.EXIT;
                }
            }
            else
            {
                nextState = new AnimalDead(lifes, baseMovementSpeed, shadowPosOffset, physicCorralLimit, xToppedOffMove, yToppedOffMoveMinMax, corralLimits, dead,
                    animal, animalShadow, anim, audioSource, audioClips);
                stage = EVENT.EXIT;
            }

        }

        SetInAirAnimalShadow();
    }

    protected sealed override void Exit()
    {
        anim.SetBool("toppedOff", false);
        animal.transform.rotation = Quaternion.identity;
        base.Exit();
    }

    void SetEndPos()
    {
        float randomNum = Random.Range(yToppedOffMoveMinMax[0], yToppedOffMoveMinMax[1]);
        if ((startPos.y + randomNum) > corralLimits[3])
            endPos = new Vector2(startPos.x + xToppedOffMove, startPos.y + yToppedOffMoveMinMax[0]);
        else if ((startPos.y + randomNum) < corralLimits[2])
            endPos = new Vector2(startPos.x + xToppedOffMove, startPos.y + yToppedOffMoveMinMax[1]);
        else
            endPos = new Vector2(startPos.x + xToppedOffMove, startPos.y + randomNum);

    }
}
