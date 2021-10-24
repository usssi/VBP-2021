using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalState
{
    [Header("AnimalVars")]
    protected sbyte lifes;
    public sbyte Lifes { get => lifes; }

    [Header("AnimalComponents")]
    protected GameObject animal;
    protected Animator anim;
    protected AudioSource audioSource;
    protected AudioClip[] audioClips;

    [Header("MovementVars")]
    protected float baseMovementSpeed;
    protected Vector2 movementDir;

    [Header("ToppedOffVars")]
    protected bool toppedOff;
    protected float xToppedOffMove;
    protected float[] yToppedOffMoveMinMax = new float[2];


    [Header("KickedVars")]
    protected bool kicked;


    [Header("DeadVars")]
    protected bool dead;
    public bool Dead { get => dead; }


    [Header("CorralVars")]
    protected float[] corralLimits;
    protected float physicCorralLimit;

    [Header("CalculateAngleVars")]
    protected float angle;
    protected float angleCos;
    protected float angleSin;
    protected float xComponent;
    protected float yComponent;
    protected float timeSinceAngleCalculated;
    protected float parabolicSpeed;
    protected float gravity = 4;
    protected Vector2 startPos;
    protected Vector2 endPos;

    [Header("Shadow")]
    protected GameObject animalShadow;
    protected float shadowPosOffset = 0.194f;


    private GameManager gameManager;

    //[Header("StateMachineVars")]
    protected enum STATE { WALKINSIDECORRAL, JUMPCORRAL, WALKOUTSIDECORRAL, TOPPEDOFF, KICKED, DEAD };
    protected enum EVENT { ENTER, UPDATE, EXIT };
    protected STATE state;
    protected EVENT stage;
    protected AnimalState nextState;

    protected virtual void Enter() { stage = EVENT.UPDATE; animalShadow.transform.rotation = Quaternion.identity;  }
    protected virtual void Update() { stage = EVENT.UPDATE; if (lifes <= 0) dead = true; }
    protected virtual void Exit() { stage = EVENT.EXIT; }

    public AnimalState Process()
    {
        if (stage == EVENT.ENTER) Enter();
        if (stage == EVENT.UPDATE) Update();
        if (stage == EVENT.EXIT)
        {
            Exit();
            return nextState;
        }

        return this;
    }


    //El constructor va a ser así: sbytes, ints, floats, booleans,Vectores, Componentes, Otros

    public AnimalState(sbyte _lifes, float _baseMovSpeed, float _shadowPosOffset, float _physicalCorralLimit, float _xToppedOffMove, float[] _yToppedOffMoveMinMax, float[] _corralLimits, bool _dead,
        GameObject _animal, GameObject _animalShadow, Animator _anim, AudioSource _audioSource, params AudioClip[] _audioClips)
    {
        lifes = _lifes;
        baseMovementSpeed = _baseMovSpeed;
        shadowPosOffset = _shadowPosOffset;
        physicCorralLimit = _physicalCorralLimit;
        xToppedOffMove = _xToppedOffMove;
        yToppedOffMoveMinMax = _yToppedOffMoveMinMax;
        corralLimits = _corralLimits;
        dead = _dead;


        animal = _animal;
        anim = _anim;
        audioSource = _audioSource;
        audioClips = _audioClips;
        animalShadow = _animalShadow;
        startPos = animal.transform.position;

        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        //En los constructores debo setear la startPos y la endPos;
    }

    protected void SetMovementDirection(bool leftDir)
    {
        if (leftDir)
            movementDir = Vector2.left;

    }

    protected void ApplyMovement()
    {
        animal.transform.Translate(movementDir * baseMovementSpeed * Time.deltaTime, Space.World);
    }


    protected void CalculateAngle()
    {
        Vector2 targetDir = (endPos - startPos);
        float y = targetDir.y;
        targetDir.y = 0;
        float x = targetDir.magnitude;
        float sSqr = Mathf.Pow(parabolicSpeed, 2);
        float underTheSquareRoot = Mathf.Pow(parabolicSpeed, 4) - gravity * (gravity * Mathf.Pow(x, 2) + 2 * y * sSqr);

        if (underTheSquareRoot >= 0)
        {
            float root = Mathf.Sqrt(underTheSquareRoot);
            float highAngle = sSqr + root;

            angle = Mathf.Atan2(highAngle, gravity * x);
            angleCos = Mathf.Cos(angle);
            angleSin = Mathf.Sin(angle);
        }
        else
        {
            parabolicSpeed++;
            CalculateAngle();
        }

    }

    protected void ApplyParabolicMovement(bool negativeXComponent)
    {
        timeSinceAngleCalculated += Time.deltaTime;

        if (negativeXComponent)
            xComponent = -(parabolicSpeed * angleCos * timeSinceAngleCalculated) + startPos.x;
        else
            xComponent = (parabolicSpeed * angleCos * timeSinceAngleCalculated) + startPos.x;

        yComponent = (parabolicSpeed * angleSin * timeSinceAngleCalculated - (gravity * Mathf.Pow(timeSinceAngleCalculated, 2) / 2)) + startPos.y;

        animal.transform.position = new Vector2(xComponent, yComponent);
    }

    protected void ApplyRotation()
    {
        animal.transform.Rotate(0, 0, 2000 * Time.deltaTime);
    }

    public void AnimalToppedOff()
    {
        PlayRandomSound();
        if (kicked == false)
        {
            lifes--;

            if (lifes == 0)
            {
                dead = true;
                gameManager.RemoveAnimalFromList(animal);

            }

            nextState = new AnimalToppedOff(lifes, baseMovementSpeed, shadowPosOffset, physicCorralLimit, xToppedOffMove, yToppedOffMoveMinMax, corralLimits, dead,
                animal, animalShadow, anim, audioSource, audioClips);
            stage = EVENT.EXIT;

        }
        else
        {
            dead = true;
            gameManager.RemoveAnimalFromList(animal);

            nextState = new AnimalDeleted(lifes, baseMovementSpeed, shadowPosOffset, physicCorralLimit, xToppedOffMove, yToppedOffMoveMinMax, corralLimits, dead,
                animal, animalShadow, anim, audioSource, audioClips);
            stage = EVENT.EXIT;
        }


    }

    public void AnimalKicked()
    {
        PlayRandomSound();

        nextState = new AnimalKicked(lifes, baseMovementSpeed, shadowPosOffset, physicCorralLimit, xToppedOffMove, yToppedOffMoveMinMax, corralLimits, dead,
                animal, animalShadow, anim, audioSource, audioClips);
        stage = EVENT.EXIT;
    }

    protected void SetGroundedAnimalShadow()
    {
        //Voy a tener que pasar en los cambios de estado el offset del animalito este.
        animalShadow.transform.position = new Vector2(animal.transform.position.x, animal.transform.position.y - shadowPosOffset);
    }

    protected void SetInAirAnimalShadow()
    {
        //Bueno brunito, momento Matemáticas. :D
        float shadowXComponent = animal.transform.position.x;
        float numerador = (endPos.y - startPos.y);
        float denominador = endPos.x - startPos.x;
        float pendienteDeLaRecta = numerador / denominador;
        float shadowYComponent = (pendienteDeLaRecta * ((shadowXComponent - startPos.x)) + startPos.y) - shadowPosOffset;

        animalShadow.transform.position = new Vector2(shadowXComponent, shadowYComponent);
        animalShadow.transform.rotation = Quaternion.identity;
    }



    protected bool CheckIfReachEndPos()
    {
        if (animal.transform.position.x > endPos.x)
            return true;
        else
            return false;

    }


    protected bool CheckIfInsideCorral()
    {
        if (animal.transform.position.x >= physicCorralLimit)
            return true;
        else
            return false;
    }


    void PlayRandomSound()
    {
        audioSource.PlayOneShot(audioClips[Random.Range(0, audioClips.Length)]);
        Debug.Log("sonido de animal");
    }

}
