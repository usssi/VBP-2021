using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TUT_BallRebotePared : TUT_Ball
{
    private float xMinPosReceibe;
    private float xMaxPosReceibe;
    private float yMinPosReceibe;
    private float yMaxPosReceibe;
    private float receibeOffset = 0.3f;

    //private Vector2 startPos;
    //private Vector2 endPos;

    private Vector2 wallReboundPos; 

    private Transform receibePos;


    private float _endPosOffset = 0.1f;

    private float shadowYPosPreviousState;


    public TUT_BallRebotePared(GameObject _ball, Animator _ballAnim, Transform _player, GameObject _redCross, GameObject _greenCircle, GameObject _sagashiGObj, TUT_ChadMovement _chadScript
        , float _leftL, float _rightL, float _bottomL, float _topL, float _ballRemSpd, float _ballRebParSpd, float _ballRecSpd, float _ballArmSpd, GameObject _ballShadow, int _comboCount, float _gravity)
        : base(_ball, _ballAnim, _player, _redCross, _greenCircle, _sagashiGObj, _chadScript, _leftL, _rightL, _bottomL, _topL
            , _ballRemSpd, _ballRebParSpd, _ballRecSpd, _ballArmSpd, _ballShadow, _comboCount, _gravity)
    {
        actualState = STATE.REBOTEPARED;
        startPos = ball.transform.position;
        speed = ballReboteParedSpeed;

        endPos = ReturnRandomPos();
        endPosOffset = _endPosOffset;
        xMinPosReceibe = endPos.x - receibeOffset;
        xMaxPosReceibe = endPos.x + receibeOffset;
        yMinPosReceibe = endPos.y - receibeOffset;
        yMaxPosReceibe = endPos.y + receibeOffset;

        wallReboundPos = GameObject.Find("/Limits/CorralLimits/CorralRightLimit").transform.position;
    }

    protected override void Enter()
    {
        base.Enter();
        ballSoundsManager.PlaySound("rebotePared");
        CalculateAngle(false);
        shadowYPosPreviousState = ballShadow.transform.position.y;
        receibePos = GameObject.FindGameObjectWithTag("ReceibePos").GetComponent<Transform>();
        SetPos(redCross, greenCircle, endPos);
        CheckIfAddxStartPosComp();
        ballAnim.ResetTrigger("ballFastSpeed");
        ballAnim.SetTrigger("ballNormal");

    }

    protected override void Update()
    {
        base.Update();
        ball.transform.position = ApplyMovement();
        CheckIfPlayerReceibed();

        //Debug.Log("UPDATE BALLREBOTEPARED");
        if(TUT_Ball.isInTutorial == false)
        {
            CheckIfBallHasTouchedGround(endPosOffset, new TUT_BallCaida(ball, ballAnim, player, redCross, greenCircle, sagashiGameObject, chadScript
                 , leftLimit, rightLimit, bottomLimit, topLimit, ballRematadaSpeed, ballReboteParedSpeed, ballRecibidaSpeed, ballArmadaSpeed, ballShadow, comboCount, gravity));
        }
        else
        {
            if(ball.transform.position.x <= endPos.x - 0.1f)
            {
                ball.transform.position = wallReboundPos;

                nextState = new TUT_BallRebotePared(ball, ballAnim, player, redCross, greenCircle, sagashiGameObject, chadScript
           , leftLimit, rightLimit, bottomLimit, topLimit, ballRematadaSpeed, ballReboteParedSpeed, ballRecibidaSpeed, ballArmadaSpeed, ballShadow, comboCount, gravity);

                stage = EVENT.EXIT;
            }
        }
        SetShadowBallPos();


    }

    protected override void Exit()
    {
        ballAnim.ResetTrigger("ballNormal");

        //Debug.Log("EXIT BALLREBOTEPARED");
        base.Exit();
    }


    private ChadSoundsManager chadSoundsManager;

    void CheckIfPlayerReceibed()
    {
        if (chadScript.InAir == false)
        {
            if (receibePos.position.x >= xMinPosReceibe && receibePos.position.x <= xMaxPosReceibe && receibePos.position.y >= yMinPosReceibe && receibePos.position.y <= yMaxPosReceibe)
            {
                if (ball.transform.position.x >= xMinPosReceibe && ball.transform.position.x <= xMaxPosReceibe && ball.transform.position.y >= yMinPosReceibe && ball.transform.position.y <= yMaxPosReceibe)
                {
                    chadSoundsManager = GameObject.FindGameObjectWithTag("Player").GetComponent<ChadSoundsManager>();
                    chadSoundsManager.ReceibeSound();

                    nextState = new TUT_BallRecibida(ball, ballAnim, player, redCross, greenCircle, sagashiGameObject, chadScript
                   , leftLimit, rightLimit, bottomLimit, topLimit, ballRematadaSpeed, ballReboteParedSpeed, ballRecibidaSpeed, ballArmadaSpeed, ballShadow, comboCount, gravity);

                    stage = EVENT.EXIT;
                }
            }
        }
    }

    protected override void SetShadowBallPos()
    {
        float xComp = ball.transform.position.x;
        float yComp;
        Vector2 pos;
        yComp = ((endPos.y - endPosOffset - shadowYPosPreviousState)) / (endPos.x - startPos.x) * (xComp - startPos.x) + shadowYPosPreviousState;


        pos = new Vector2(xComp, yComp);
        ballShadow.transform.position = pos;
    }
}
