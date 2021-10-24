using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkOutsideCorral : AnimalState
{
    private GameObject door;
    private bool hasReachedDoor = false;
    private GameManager gameManager;

    private GameObject[] pathsGameObject;           //Para agarrar los obj. No puedo hacer directamente el GetComponent porque no heredo de MonoB
    private AnimalPath[] animalsPaths;               //Para agarrar todos los componentes AnimalPath de los obj contenidos en el array pathsGameObj
    private List<AnimalPath> possibleAnimalPaths = new List<AnimalPath>();       //Almacenar los posibles paths que puede recorrer el animal, estos son aquellos que inician en un punto más bajo que su pos de spawn
    private AnimalPath currentPath;                 //Path que va a recorrer

    private int pathPosIndex;
   public WalkOutsideCorral(sbyte _lifes, float _baseMovSpeed, float _shadowPosOffset, float _physicalCorralLimit, float _xToppedOffMove, float[] _yToppedOffMoveMinMax, float[] _corralLimits, bool _dead,
        GameObject _animal, GameObject _animalShadow, Animator _anim, AudioSource _audioSource, params AudioClip[] _audioClips)
        : base(_lifes, _baseMovSpeed, _shadowPosOffset, _physicalCorralLimit, _xToppedOffMove, _yToppedOffMoveMinMax, _corralLimits, _dead, _animal, _animalShadow, _anim, _audioSource, _audioClips)
    {
        state = STATE.WALKOUTSIDECORRAL;
    }

    protected sealed override void Enter()
    {
        door = GameObject.FindGameObjectWithTag("Door");
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        GetPaths();
        SelectPath();
        SetMovementDirection(true);
        base.Enter();
    }

    

    protected sealed override void Update()
    {
        base.Update();
        if(followPath == false)
        {
            ApplyMovement();
            StartFollowPath();
        }
        else
        {
            FollowPath();
        }
        SetGroundedAnimalShadow();

        CheckIfHasReachedDoor();

        //DetectDoor();
    }

    protected sealed override void Exit()
    {

        base.Exit();
    }

    void GetPaths()
    {
        pathsGameObject = GameObject.FindGameObjectsWithTag("AnimalPath");
        animalsPaths = new AnimalPath[pathsGameObject.Length];
        for (int i = 0; i < pathsGameObject.Length; i++)
        {
            animalsPaths[i] = pathsGameObject[i].GetComponent<AnimalPath>();
        }
    }

    void SelectPath()
    {
        //Acá vamos a elegir un path aleatroio  que tenga su inicio en una posición menor en la componente Y que la del animal.
        for(int i = 0; i < animalsPaths.Length; i++)
        {
            if (animalsPaths[i].PathStartPos.y < animal.transform.position.y)
            {
                possibleAnimalPaths.Add( animalsPaths[i]);
                //Debug.LogWarning("PathGetted");
            }

            //Acá abría que poner un break o una forma de agarrar un path random.
        }

        currentPath = possibleAnimalPaths[Random.Range(0, possibleAnimalPaths.Count)];

        for(int j = 0; j < currentPath.PathPoints.Length; j++)
        {
            if(currentPath.PathPoints[j].position.y > animal.transform.position.y)
            {
                pathPosIndex = j;
                firstAbovePosition = currentPath.PathPoints[j];
                //Debug.LogWarning("firstAbovePos: " + firstAbovePosition.transform.position);
                break;
            }
        }
    }

    //Obj donde almacenaremos el primer punto por encima del animal.
    private Transform firstAbovePosition;
    private bool followPath = false;

    void StartFollowPath()
    {
        if(firstAbovePosition != null)
            if (animal.transform.position.x <= firstAbovePosition.position.x)
            {
                followPath = true;

                if(pathPosIndex + 1 < currentPath.PathPoints.Length)
                    movementDir = currentPath.PathPoints[++pathPosIndex].position - animal.transform.position;
            }
    }

    void FollowPath()
    {
        animal.transform.Translate(movementDir.normalized * baseMovementSpeed * Time.deltaTime);
        if (Vector2.Distance(animal.transform.position, currentPath.PathPoints[pathPosIndex].position) < 0.1f && pathPosIndex < currentPath.PathPoints.Length - 1)
        {
            if(currentPath.PathPoints[pathPosIndex + 1] != null)
                 movementDir = currentPath.PathPoints[++pathPosIndex].position - animal.transform.position;
        }
    }

    void CheckIfHasReachedDoor()
    {
        if (Vector2.Distance(door.transform.position, animal.transform.position) < 0.3f && hasReachedDoor == false)
        {
            GameObject.FindGameObjectWithTag("PuertaAbierta").GetComponent<Animator>().SetTrigger("open");
            GameObject.FindGameObjectWithTag("PuertaAbierta").GetComponent<PlayDoorOpenSound>().PlayOpenSound();
            hasReachedDoor = true;
            gameManager.LoseLife();
            dead = true;
            gameManager.RemoveAnimalFromList(animal);
            if (gameManager.GetAnimalList().Count == 0)
            {
                gameManager.SpawnSoloAnimal();
            }
            GameObject.Destroy(animal.transform.parent.gameObject);
        }
       
    }
}
