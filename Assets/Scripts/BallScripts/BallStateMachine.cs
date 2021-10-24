using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallStateMachine : MonoBehaviour
{
    private GameObject ball;
    private Transform player;
    private Ball currentState;
    private Animator ballAnim;
    private GameObject sagashiGameObject;
    public Ball CurrentState { get => currentState; }

    public GameObject redCross;
    [SerializeField] private GameObject GreenCircle;
    [SerializeField] private GameObject ballShadow;

    private ChadMovement chadScript;

    [Header("BallSpeeds")]
    [SerializeField] private float ballRematadaSpeed, ballReboteParedSpeed, ballRecibidaSpeed, ballArmadaSpeed;


    [Header("SceneLimits")]
    private float leftLimit;
    private float rightLimit;
    private float bottomLimit;
    private float topLimit;

    private Vector2 startPos;
    private Vector2 endPos;
    public Vector2 EndPos { get => endPos; }
    public Vector2 StartPos { get => startPos; }


    [Header("BallHud")]
    private int comboCount;
    [SerializeField]private GameObject superBallMask;

    bool ballActivated = false;

    private void Start()
    {
        ball = this.gameObject;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        chadScript = GameObject.FindGameObjectWithTag("Player").GetComponent<ChadMovement>();
        ballAnim = GameObject.FindGameObjectWithTag("Ball").GetComponent<Animator>();
        sagashiGameObject = GameObject.FindGameObjectWithTag("Chino");
        leftLimit = GameObject.Find("/Limits/MapLimits/BallLeftLimit").transform.position.x;
        rightLimit = GameObject.Find("/Limits/MapLimits/BallRightLimit").transform.position.x;
        bottomLimit = GameObject.Find("/Limits/MapLimits/BallBottomLimit").transform.position.y;
        topLimit = GameObject.Find("/Limits/MapLimits/BallTopLimit").transform.position.y;

        StartCoroutine(StartBall(3f));

        //Debug.Log($"LEFT: {leftLimit} RIGHT: {rightLimit} BOTTOM: {bottomLimit} TOP: {topLimit}");
    }

    private void Update()
    {
        if (ballActivated == false)
            return;

        startPos = currentState.StartPos;
        endPos = currentState.EndPos;
        currentState = currentState.Process();

        comboCount = currentState.ComboCount;
        //Debug.Log("Combo Count: " + comboCount);

        if(superBallMask != null)
        {
            if (3 - comboCount != 0)
            {
                float num = (3f - comboCount) / 3f;
                superBallMask.transform.localScale = new Vector3(num, 1, 0);

            }
            else
                superBallMask.transform.localScale = new Vector3(0, 1, 0);
        }
        
    }   

    private IEnumerator StartBall(float _time)
    {
        yield return new WaitForSeconds(_time);
        currentState = new BallCaida(ball, ballAnim, player, redCross, GreenCircle, sagashiGameObject, chadScript, leftLimit, rightLimit, bottomLimit, topLimit, ballRematadaSpeed, ballReboteParedSpeed, ballRecibidaSpeed, ballArmadaSpeed, ballShadow, 0, 4, false);
        ballActivated = true;
    }

    
}
