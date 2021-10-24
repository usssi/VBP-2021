using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    private int lifes = 3;
    public int Lifes { get => lifes; set => lifes = value; }

    [SerializeField] private int roundsAmount;
    [SerializeField] private int[] animalsPerRound;
    private int animalsAmountToSpawn;


    [Header("CantAnimalesMatadosParaPancho")]
    [SerializeField] private int[] flagsAnimalsToKill;
    public int currentAnimalsToKill;

    private bool gameWon = false;
    public bool GameWon { get => gameWon; }

    public static bool gameOver = false;
    public static bool GameOver { get => gameOver; }


    [Header("AnimalSpawnerVars")]
    [SerializeField] private GameObject[] animalsToSpawn;
    [SerializeField] private GameObject fishPrefab;
    [SerializeField] private Transform xSpawnPos;
    private AnimalSpawner animalSpawner;


    [Header("AnimalGetterVars")]
    [SerializeField] private Transform rightCorralLimit;
    private AnimalGetter animalGetter;


    [Header("SceneCanvas")]
    [SerializeField] private GameObject gameWonCanvas, gameOverCanvas;
    [SerializeField] private GameObject gameButtonsCanvas, pauseButtonCanvas;
    [SerializeField] private GameObject panchitosOKCanvas, animalScapedCanvas;
    [SerializeField] private Image congratulationsImage, backgroundPanchoCanvas;
    [SerializeField] private float timeInScreen, timeAmount, alphaAmount;
   

    [Header("PauseCanvas")]
    [SerializeField] private GameObject pauseButton;

    //DifficultyValues
    private int roundIndex;

    [Header("ChadDifficulties")]
    [SerializeField] private float[] chadMovementSpeeds;
    [SerializeField] private float[] chadJumpSpeeds;
    [SerializeField] private float[] chadTimeMult;
    [SerializeField] private float[] chadKickCooldown;

    [Header("BallDifficulties")]
    [SerializeField] private float[] ballGravity;
    [SerializeField] private float[] ballWallReboundSpeed;
    [SerializeField] private float[] ballReceivedSpeed;
    [SerializeField] private float[] ballArmadaSpeed;


    [Header("AnimalsSpawnerDifficulties")]
    [SerializeField] private float[] timesToSpawnAnimals;


    //Scripts to change difficulty
    private ChadMovement movementScript;
    private Ball ballScript;

    [Header("CorralVars")]
    float yCorralMin;
    float yCorralMax;

    [Header("PanchosUI")]
    [SerializeField] private GameObject bigPancho;
    [SerializeField] private GameObject[] panchitosObject;
    private int panchitosCount = 0;
    [Header("Sounds")]
    //[SerializeField] public AudioClip[] anouncerArray;
    [Space]
    //[SerializeField] private AudioClip[] panchoLlegaMarcador;
    //[SerializeField] private AudioClip panchoDesliza;
    [Space]
    //[SerializeField] public AudioSource anouncer321;
    [SerializeField] private float delay321 = 0.7f;

    [Header("Lifes")]
    [SerializeField] private GameObject[] lifesObjects;
    private int lifesCount = 0;

    [Header("BlackImage")]
    [SerializeField] private Image blackImage;

    private void Awake()
    {
        StartCoroutine(FadeOut());
        gameOver = false;
        Time.timeScale = 1;
    }


    float timer = 4f;

    private void Start()
    {
        movementScript = GameObject.FindGameObjectWithTag("Player").GetComponent<ChadMovement>();
        yCorralMin = GameObject.Find("/Limits/CorralLimits/CorralBottomLimit").transform.position.y;
        yCorralMax = GameObject.Find("/Limits/CorralLimits/CorralTopLimit").transform.position.y;
        animalSpawner = new AnimalSpawner(animalsToSpawn, fishPrefab, yCorralMin, yCorralMax, xSpawnPos, this);
        animalGetter = new AnimalGetter(rightCorralLimit);
        SetDifficulty();
        Debug.Log("GameOver: " + gameOver);
        StartCoroutine(SpawnFirstAnimal(3.5f));

        Invoke("Anouncer321", delay321);

    }

    private void Update()
    {
        CheckWin();
        CheckGameOver();

        if(timer >= 0)
            timer -= Time.deltaTime;
        else
        {
            if (GetAnimalList().Count <= 0)
            {
                StartCoroutine(CheckIfSceneClear());
            }
        }
        
    }

    void CheckWin()
    {
        if (roundIndex > roundsAmount && gameWon == false)
        {
            gameWon = true;
            Debug.Log("Ganaste aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
            StartCoroutine(LoadNewScene(2));
        }
       
    }

    void CheckGameOver()
    {
        if (lifes <= 0 && gameOver == false)
        {
            gameOver = true;
            StartCoroutine(LoadNewScene(3));
            //EndGame();
        }
    }


    //void CheckAnimalsKilledFlag()
    //{

    //}

    void PopOutPanchitosOkCanvas()
    {

        gameButtonsCanvas.SetActive(false);
        pauseButtonCanvas.SetActive(false);
        panchitosOKCanvas.SetActive(true);
        Time.timeScale = 0;
        StartCoroutine(ClosePanchitosCanvas());

        //SetDifficulty();
    }

    public void LoseLife()
    {
        lifes--;
        lifesObjects[lifesCount].gameObject.SetActive(false);
        lifesObjects[++lifesCount].gameObject.SetActive(true);
    }

    void SetDifficulty()
    {

        if (roundIndex < chadMovementSpeeds.Length)
            movementScript.SetDifficulty(chadMovementSpeeds[roundIndex], chadJumpSpeeds[roundIndex], chadTimeMult[roundIndex], chadKickCooldown[roundIndex]);

        ballScript = GameObject.FindGameObjectWithTag("Ball").GetComponent<BallStateMachine>().CurrentState;
        if (ballScript != null && roundIndex < ballGravity.Length)
            ballScript.SetDifficulty(ballGravity[roundIndex], ballWallReboundSpeed[roundIndex], ballReceivedSpeed[roundIndex], ballArmadaSpeed[roundIndex]);

        if (roundIndex < animalsPerRound.Length)
            animalsAmountToSpawn = animalsPerRound[roundIndex];

        if (roundIndex <= flagsAnimalsToKill.Length)
            currentAnimalsToKill = flagsAnimalsToKill[roundIndex];

        ++roundIndex;
    }

    IEnumerator ClosePanchitosCanvas()
    {
        //if (panchitosCount < anouncerArray.Length)
        //{
        //    GameObject.FindGameObjectWithTag("AudioSource").GetComponent<AudioSource>().PlayOneShot(anouncerArray[panchitosCount]);

        //    //FindObjectOfType<AudioManager>().PlayA("awesome");

        //    Debug.Log("MIRA ESTO: " + anouncerArray[panchitosCount]);

        //}

        FindObjectOfType<AudioManager>().PlayA("anouncer" + panchitosCount);

        Color tempColor;

        yield return new WaitForSecondsRealtime(timeInScreen);
        while (congratulationsImage.color.a > 0)
        {
            tempColor = congratulationsImage.color;
            tempColor.a -= alphaAmount;
            congratulationsImage.color = tempColor;

            if (congratulationsImage.color.a < 0.3f)
            {

                Color tempColor1 = backgroundPanchoCanvas.color;
                tempColor1.a -= alphaAmount;
                backgroundPanchoCanvas.color = tempColor1;
            }

            yield return new WaitForSecondsRealtime(timeAmount);
        }

        panchitosOKCanvas.SetActive(false);
        Instantiate(bigPancho, Vector2.zero, bigPancho.transform.rotation);

        //GameObject.FindGameObjectWithTag("AudioSource").GetComponent<AudioSource>().PlayOneShot(panchoDesliza);
        FindObjectOfType<AudioManager>().Play("panchoDesliza");


        tempColor = congratulationsImage.color;
        tempColor.a = 1;
        congratulationsImage.color = tempColor;

        tempColor = backgroundPanchoCanvas.color;
        tempColor.a = 0.23f;
        backgroundPanchoCanvas.color = tempColor;

        gameButtonsCanvas.SetActive(true);
        pauseButton.SetActive(true);

        Time.timeScale = 1;

        yield return new WaitForSecondsRealtime(0.7f);

        //if (panchitoscount < panchollegamarcador.length)
        //    gameobject.findgameobjectwithtag("audiosource").getcomponent<audiosource>().playoneshot(panchollegamarcador[panchitoscount]);
        FindObjectOfType<AudioManager>().PlayB("marcador" + panchitosCount);


        panchitosObject[panchitosCount].gameObject.SetActive(false);
        panchitosObject[++panchitosCount].gameObject.SetActive(true);
    }

   

    void SpawnAnimalRound()
    {
        if(roundIndex <= roundsAmount)
            animalSpawner.SpawnAnimalRound(animalsAmountToSpawn);

        if (roundIndex == 4)
            animalSpawner.SpawnFish();

        if(roundIndex == 5)
        {
            animalSpawner.SpawnFish();
            animalSpawner.SpawnFish();
        }
        //GetAnimalsInScene();
    }

    private bool checkingScene = false;
    IEnumerator CheckIfSceneClear()
    {
        if (checkingScene == false)
        {
            //Debug.Log("Coroutine started");
            checkingScene = true;

            yield return 0;
            if (GetAnimalList().Count <= 0)
            {
                SetNewAnimalRound();
            }
            checkingScene = false;
        }
    }

    private void SetNewAnimalRound()
    {
        if(gameWon == false && gameOver == false)
        {
            SetDifficulty();
            PopOutPanchitosOkCanvas();
            SpawnAnimalRound();
        }

        
    }

    IEnumerator LoadNewScene(int sceneIndex)
    {
        yield return new WaitForSeconds(1f);

        while(blackImage.color.a < 0.95f)
        {
            Color tempColor = blackImage.color;
            tempColor.a += 0.005f;
            blackImage.color = tempColor;

            yield return new WaitForSeconds(0.01f);
        }

        SceneManager.LoadScene(sceneIndex);
        //Y acá cargar otra escena locura maestra magnífica asombrosa.
    }

    IEnumerator FadeOut()
    {
        Color c = blackImage.color;
        c.a = 1;
        blackImage.color = c;
        while (blackImage.color.a > 0.05f)
        {
            Color tempColor = blackImage.color;
            tempColor.a -= 0.005f;
            blackImage.color = tempColor;

            yield return new WaitForSeconds(0.008f);
        }
    }

    private IEnumerator SpawnFirstAnimal(float _time)
    {
        yield return new WaitForSeconds(_time);
        SpawnSoloAnimal();
    }

    public GameObject GetFirsAnimal() => animalGetter.GetFirstAnimal();
    public List<GameObject> GetAnimalList() => animalGetter.AnimalsList;
    //public void ReduceList() => animalGetter.ReduceAnimalList();
    public void RemoveAnimalFromList(GameObject animalParent) => animalGetter.RemoveAnimalFromList(animalParent);

    //public void GetAnimalsInScene() => animalGetter.GetAnimalsInScene();
    public void AddAnimalToList(GameObject animal) => animalGetter.AddAnimalToList(animal);

    public void SpawnSoloAnimal() => animalSpawner.SpawnSoloAnimal();


    private void Anouncer321()
    {
        //GameObject.FindGameObjectWithTag("AudioSource").GetComponent<AudioSource>().PlayOneShot(anouncer321);
        FindObjectOfType<AudioManager>().Play("321go");

        //anouncer321.Play();
        Debug.Log("3,2,1,go");
    }

}
