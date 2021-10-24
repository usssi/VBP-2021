using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalDead : AnimalState
{
    private GameManager gameManager;

    public AnimalDead(sbyte _lifes, float _baseMovSpeed, float _shadowPosOffset, float _physicalCorralLimit, float _xToppedOffMove, float[] _yToppedOffMoveMinMax, float[] _corralLimits, bool _dead,
        GameObject _animal, GameObject _animalShadow, Animator _anim, AudioSource _audioSource, params AudioClip[] _audioClips)
        : base(_lifes, _baseMovSpeed, _shadowPosOffset, _physicalCorralLimit, _xToppedOffMove, _yToppedOffMoveMinMax, _corralLimits, _dead, _animal, _animalShadow, _anim, _audioSource, _audioClips)
    {
        state = STATE.DEAD;
    }

    protected sealed override void Enter()
    {
        dead = true;
        animalShadow.gameObject.SetActive(false);
        //gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        //gameManager.AnimalsKilled++;
        //gameManager.ReduceList();
        anim.SetTrigger("dead");
        GameObject.Destroy(animal.transform.parent.gameObject, 1);
        base.Enter();
        SetGroundedAnimalShadow();
    }


    protected sealed override void Update()
    {

        
    }

    protected sealed override void Exit()
    {

        base.Exit();
    }

}
