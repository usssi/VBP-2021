using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TUT_GameManager : MonoBehaviour
{
    [SerializeField] private GameObject animalSchool, tutorialBall,jumpIndicator, blowIndicator, kickIndicator,chadBlowPos, redCross, greenCircle;
    [SerializeField] private Animator thumbUp;

    [SerializeField]
    private Transform rightCorralLimit;
    [SerializeField] private TUT_BallStateMachine tutBallStateMachine;
    [SerializeField] private TUT_AnimalStateMachine tutAnimalStateMachine;
    [SerializeField] private Image blackImage;
    private TUT_ChadMovement chadMovement;

    private delegate void CurrentState();
    CurrentState state;
    [SerializeField] private Transform senseiPos;

    //Lógica de las distintas partes que se deben ejecutar a lo largo del tutorial.

    private void Awake()
    {
        StartCoroutine(FadeOut());
    }

    private void Start()
    {
        chadMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<TUT_ChadMovement>();
        state = FirstState;
    }

    private void Update()
    {
        if(state != null)
            state();
    }


    public GameObject GetFirstAnimal() => animalSchool;

    private void FirstState()
    {
        if (jumpIndicator.activeSelf == false)
            jumpIndicator.SetActive(true);

        if(Vector2.Distance(jumpIndicator.transform.position, chadMovement.JumpStart) < 0.5f)
        {
            thumbUp.SetTrigger("thumbUp");
            FindObjectOfType<AudioManager>().Play("sagashiOk");

            state = null;
            state = SecondState;
        }
    }

    private void SecondState()
    {
        
            jumpIndicator.SetActive(false);

            tutorialBall.SetActive(true);
        

        if(Vector2.Distance(jumpIndicator.transform.position, chadMovement.JumpStart) < 0.5f)
        {
            if(Vector2.Distance(blowIndicator.transform.position, chadBlowPos.transform.position) < 0.5f)
            {
                if(chadMovement.hasBlow == true)
                {
                    thumbUp.SetTrigger("thumbUp");
                    FindObjectOfType<AudioManager>().Play("sagashiOk");

                    state = ThirdState;
                }
            }
        }
    }

    private int recibida = 0;
    private void ThirdState()
    {
        if(recibida != TUT_Ball.timesRecibida)
        {
            recibida = TUT_Ball.timesRecibida;
            thumbUp.SetTrigger("thumbUp");
            FindObjectOfType<AudioManager>().Play("sagashiOk");

        }
        if (TUT_Ball.timesRecibida >= 3)
        {
            if (tutorialBall.activeSelf == true)
            {
                StartCoroutine(WaitToDesapearBall());
                state = FourthState;
            }
        }
    }
    private int pateadas = 0;
    private void FourthState()
    {
        if(Vector2.Distance(animalSchool.transform.position, kickIndicator.transform.position) < 0.1f)
        {
            if(kickIndicator.activeSelf == false)
                kickIndicator.SetActive(true);  
        }
        else
        {
            if (kickIndicator.activeSelf == true)
                kickIndicator.SetActive(false);
        }
        
        if(pateadas != TUT_AnimalState.timesKicked)
        {
            pateadas = TUT_AnimalState.timesKicked;
            thumbUp.SetTrigger("thumbUp");
            FindObjectOfType<AudioManager>().Play("sagashiOk");

        }

        if (TUT_AnimalState.timesKicked >= 2)
        {
            PlayerPrefs.SetInt("hasPlayedTutorial", 0);
            StartCoroutine(LoadNewScene(1));
            state = null;
        }
    }

    IEnumerator WaitToDesapearBall()
    {
        while(Vector2.Distance(tutorialBall.transform.position, senseiPos.position) > 0.2f)
        {
            yield return 0;
        }

        redCross.SetActive(false);
        greenCircle.SetActive(false);
        tutorialBall.SetActive(false);
        tutAnimalStateMachine.SetNewWalkingPos();


        //yield return new WaitForSeconds(_time);

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

    IEnumerator LoadNewScene(int sceneIndex)
    {
        yield return new WaitForSeconds(1f);

        while (blackImage.color.a < 0.95f)
        {
            Color tempColor = blackImage.color;
            tempColor.a += 0.005f;
            blackImage.color = tempColor;

            yield return new WaitForSeconds(0.008f);
        }

        SceneManager.LoadScene(sceneIndex);
    }


}
