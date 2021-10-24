using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TUT_BallStateMachine : MonoBehaviour
{
    private GameObject ball;
    private Transform player;
    private TUT_Ball currentState;
    private Animator ballAnim;
    private GameObject sagashiGameObject;
    public TUT_Ball CurrentState { get => currentState; }

    public GameObject redCross;
    [SerializeField] private GameObject GreenCircle;
    [SerializeField] private GameObject ballShadow;

    private TUT_ChadMovement chadScript;

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
    [SerializeField] private GameObject superBallMask;


    private void Start()
    {
        ball = this.gameObject;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        chadScript = GameObject.FindGameObjectWithTag("Player").GetComponent<TUT_ChadMovement>();
        ballAnim = GameObject.FindGameObjectWithTag("Ball").GetComponent<Animator>();
        sagashiGameObject = GameObject.FindGameObjectWithTag("Chino");
        leftLimit = GameObject.Find("/Limits/MapLimits/BallLeftLimit").transform.position.x;
        rightLimit = GameObject.Find("/Limits/MapLimits/BallRightLimit").transform.position.x;
        bottomLimit = GameObject.Find("/Limits/MapLimits/BallBottomLimit").transform.position.y;
        topLimit = GameObject.Find("/Limits/MapLimits/BallTopLimit").transform.position.y;

        //currentState = new TUT_BallCaida(ball, ballAnim, player, redCross, GreenCircle, sagashiGameObject, chadScript,   leftLimit, rightLimit, bottomLimit, topLimit, ballRematadaSpeed, ballReboteParedSpeed, ballRecibidaSpeed, ballArmadaSpeed, ballShadow, 0, 4);
        currentState = new TUT_BallCaida(ball, ballAnim, player, redCross, GreenCircle, sagashiGameObject, chadScript, leftLimit, rightLimit, bottomLimit, topLimit, ballRematadaSpeed, ballReboteParedSpeed, ballRecibidaSpeed, ballArmadaSpeed, ballShadow, 0, 4, false);

        //Debug.Log($"LEFT: {leftLimit} RIGHT: {rightLimit} BOTTOM: {bottomLimit} TOP: {topLimit}");
    }

    private void Update()
    {
        startPos = currentState.StartPos;
        endPos = currentState.EndPos;
        currentState = currentState.Process();

        comboCount = currentState.ComboCount;
        //Debug.Log("Combo Count: " + comboCount)

    }

   public void SetCurrentStateToRebotePared()
    {
        currentState = new TUT_BallRebotePared(ball, ballAnim, player, redCross, GreenCircle, sagashiGameObject, chadScript,
            leftLimit, rightLimit, bottomLimit, topLimit, ballRematadaSpeed, ballReboteParedSpeed, ballRecibidaSpeed, ballArmadaSpeed, ballShadow, 0, 4);
    }

    public void SetCurrentStateToBallArmada()
    {
        currentState = new TUT_BallArmada(ball, ballAnim, player, redCross, GreenCircle, sagashiGameObject, chadScript,
            leftLimit, rightLimit, bottomLimit, topLimit, ballRematadaSpeed, ballReboteParedSpeed, ballRecibidaSpeed, ballArmadaSpeed, ballShadow, 0, 4, new Vector2(-1.5f, -0.3f));
    }

}
