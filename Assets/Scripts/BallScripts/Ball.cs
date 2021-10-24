using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball
{
    public enum STATE {RECIBIDA, ARMADA, REMATADA, REBOTEANIMAL, REBOTEPARED,CAIDASUELO};

    protected enum EVENT
    {
        ENTER, UPDATE, EXIT
    };

    protected STATE actualState;
    protected EVENT stage;
    protected GameObject ball;
    protected Transform player;
    protected Animator ballAnim;
    protected GameObject sagashiGameObject;
    protected Ball nextState;

    protected GameObject redCross;
    protected GameObject greenCircle;
    protected GameObject ballShadow;

    protected float gravity = 4f;

    protected ChadMovement chadScript;

    protected BallSoundsManager soundsManager;

    [Header("BallSpeeds")]
    protected float ballRematadaSpeed, ballReboteParedSpeed, ballRecibidaSpeed, ballArmadaSpeed;

    [Header("BallComboCounter")]
    protected int comboCount;
    public int ComboCount { get => comboCount; }

    [Header("MovementAndCalculateAngleVars")]
    protected Vector2 startPos;
    protected Vector2 endPos;
    protected float speed;
    protected float angle;
    protected float angleCos;
    protected float angleSin;
    protected float xComponent;
    protected float yComponent;
    protected float timeSinceBallInAir;
    private sbyte xComponentDir = 1;
    protected bool addXStartPosComponent = false;
    protected float leftLimit;
    protected float rightLimit;
    protected float topLimit;
    protected float bottomLimit;

    protected float endPosOffset;

    public Vector2 EndPos { get => new Vector2(endPos.x, endPos.y - endPosOffset); }
    public Vector2 StartPos { get => startPos; }

    public STATE ActualState { get => actualState; }

    public Ball(GameObject _ball, Animator _ballAnim ,Transform _player, GameObject _redCross, GameObject _greenCircle,GameObject _sagashiGObj, ChadMovement _chadScript
        , float _leftL, float _rightL, float _bottomL, float _topL, float _ballRemSpd, float _ballRebParSpd, float _ballRecSpd, float _ballArmSpd, GameObject _ballShadow, int _comboCount, float _gravity)

    {
        ball = _ball;
        ballAnim = _ballAnim;
        player = _player;
        redCross = _redCross;
        greenCircle = _greenCircle;
        sagashiGameObject = _sagashiGObj;
        chadScript = _chadScript;
        leftLimit = _leftL;
        rightLimit = _rightL;
        bottomLimit = _bottomL;
        topLimit = _topL;
        ballRematadaSpeed = _ballRemSpd;
        ballReboteParedSpeed = _ballRebParSpd;
        ballRecibidaSpeed = _ballRecSpd;
        ballArmadaSpeed = _ballArmSpd;
        ballShadow = _ballShadow;
        comboCount = _comboCount;
        gravity = _gravity;

        startPos = ball.transform.position;
    }

    protected virtual void Enter() 
    {
        stage = EVENT.UPDATE;
        soundsManager = ball.GetComponent<BallSoundsManager>();
    }
    protected virtual void Update() { stage = EVENT.UPDATE; SetBallRotation(); }
    protected virtual void Exit() { stage = EVENT.EXIT; }

    public Ball Process()
    {
        if (stage == EVENT.ENTER) Enter();
        if (stage == EVENT.UPDATE) Update();
        if(stage == EVENT.EXIT)
        {
            Exit();
            return nextState;
        }

        return this;
    }

    protected void CalculateAngle(bool lowAngle)
    {
        Vector2 targetDir = endPos - startPos;
        float y = targetDir.y;
        targetDir.y = 0;
        float x = targetDir.magnitude;
        float sSqr = Mathf.Pow(speed, 2);
        float underTheSqueareRoot = Mathf.Pow(speed, 4) - gravity * (gravity * Mathf.Pow(x, 2) + 2 * y * sSqr);

        if(underTheSqueareRoot >= 0)
        {
            float root = Mathf.Sqrt(underTheSqueareRoot);
            if (lowAngle)
                angle = Mathf.Atan2(sSqr - root, gravity * x) * Mathf.Rad2Deg;
            if (!lowAngle)
                angle = Mathf.Atan2(sSqr + root, gravity * x) * Mathf.Rad2Deg;

            angleCos = Mathf.Cos(angle * Mathf.Deg2Rad);
            angleSin = Mathf.Sin(angle * Mathf.Deg2Rad);
            //Debug.Log("AngleCos: " + angleCos);
            //Debug.Log("AngleSin: " + angleSin);
        }
        else
        {
            speed++;
            CalculateAngle(lowAngle);
        }
    }

    protected Vector2 ApplyMovement()
    {
        timeSinceBallInAir += Time.deltaTime;

        if (addXStartPosComponent)
            xComponent = (speed * angleCos * timeSinceBallInAir) + startPos.x;
        else if (!addXStartPosComponent)
            xComponent = (speed * angleCos * timeSinceBallInAir) - startPos.x;

        yComponent = (float)(speed * angleSin * timeSinceBallInAir - (gravity * Mathf.Pow(timeSinceBallInAir, 2)) / 2) + startPos.y;
        return new Vector2(xComponent * xComponentDir, yComponent);
    }

    protected bool CompareXPositions()
    {
        if (startPos.x > endPos.x)
            return true;
        else
            return false;
    }
    
    protected void CheckIfAddxStartPosComp()
    {
        if (CompareXPositions()) { xComponentDir = -1; addXStartPosComponent = false; }
        else { xComponentDir = 1; addXStartPosComponent = true; }
    }

    protected void CheckIfBallHasTouchedGround(float offset, Ball ballNextState)
    {
        if (ball.transform.position.y <= endPos.y - offset)
        {

            nextState = ballNextState;
            stage = EVENT.EXIT;
        }
    }

    protected void SetPos(GameObject showObj, GameObject hideObj, Vector2 pos)
    {
        if(showObj.activeSelf == false)
            showObj.SetActive(true);

        if (hideObj.activeSelf == true)
            hideObj.SetActive(false);

        showObj.transform.position = pos;
    }


    private Vector2 framePos = Vector2.zero;
    private Vector2 lastFramePos = Vector2.zero;
    private Vector2 movementDir = Vector2.zero;
    private float deltaX;
    private float deltaY;

    protected void SetBallRotation()
    {
        lastFramePos = framePos;
        framePos = ball.transform.position;

        deltaX = framePos.x - lastFramePos.x;
        deltaY = framePos.y - lastFramePos.y;

        movementDir = new Vector2(deltaX, deltaY).normalized;

        float ballAngle = Vector2.SignedAngle(Vector2.right, movementDir);

        ball.transform.rotation = Quaternion.Euler(0, 0, ballAngle);

    }



   
    protected bool ReturnRandomBool()
    {
        if (Random.Range(0, 2) == 0)
            return true;
        else
            return false;
    }

    protected Vector2 ReturnRandomPos()
    {
        return new Vector2(Random.Range(leftLimit, rightLimit), Random.Range(bottomLimit, topLimit));
    }

    protected virtual void SetShadowBallPos() { }

    public void SetDifficulty(float _newGravity, float _newWallReboundSpd, float _newReceivedSpd, float _newArmadaSpd)
    {
        gravity = _newGravity;
        ballReboteParedSpeed = _newWallReboundSpd;
        ballRecibidaSpeed = _newReceivedSpd;
        ballArmadaSpeed = _newArmadaSpd;
    }
}
