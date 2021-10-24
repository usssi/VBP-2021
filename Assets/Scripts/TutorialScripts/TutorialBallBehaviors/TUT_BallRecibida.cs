using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TUT_BallRecibida : TUT_Ball
{
    private Vector2 randomPos;

    private Vector2 pos;
    private Animator chadAnim;

    public TUT_BallRecibida(GameObject _ball, Animator _ballAnim, Transform _player, GameObject _redCross, GameObject _greenCircle, GameObject _sagashiGObj, TUT_ChadMovement _chadScript
        , float _leftL, float _rightL, float _bottomL, float _topL, float _ballRemSpd, float _ballRebParSpd, float _ballRecSpd, float _ballArmSpd, GameObject _ballShadow, int _comboCount, float _gravity)
        : base(_ball, _ballAnim, _player, _redCross, _greenCircle, _sagashiGObj, _chadScript, _leftL, _rightL, _bottomL, _topL
            , _ballRemSpd, _ballRebParSpd, _ballRecSpd, _ballArmSpd, _ballShadow, _comboCount, _gravity)

    {
        endPosOffset = 0;
        actualState = STATE.RECIBIDA;
        speed = ballRecibidaSpeed;
        startPos = ball.transform.position;
        sagashiGameObject = GameObject.FindGameObjectWithTag("Chino");
        endPos = sagashiGameObject.transform.position;        //ChineseHead
        ballAnim.SetTrigger("ballNormal");
        chadAnim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        chadAnim.SetTrigger("defend");
    }

    protected override void Enter()
    {
        TUT_Ball.timesRecibida++;

        CheckIfAddxStartPosComp();

        CalculateAngle(ReturnRandomBool());

        //randomPos = new Vector2(Random.Range(-8f, 1f), Random.Range(-4f, 3f));
        pos = GameObject.FindGameObjectWithTag("BallPosCaida").transform.position; ;
        SetPos(greenCircle, redCross, pos);

        //redCross.transform.position = randomPos;
        //redCross.gameObject.SetActive(true);

        //Debug.Log("ENTER BALLRECIBIDA");
        base.Enter();
        comboCount++;
    }

    protected override void Update()
    {
        ball.transform.position = ApplyMovement();
        if (CheckIfPosEqualsChineseHead() == true)
        {
            nextState = new TUT_BallArmada(ball, ballAnim, player, redCross, greenCircle, sagashiGameObject, chadScript,
            leftLimit, rightLimit, bottomLimit, topLimit, ballRematadaSpeed, ballReboteParedSpeed, ballRecibidaSpeed, ballArmadaSpeed, ballShadow, comboCount, gravity, pos);

            stage = EVENT.EXIT;


        }
        SetShadowBallPos();
    }

    protected override void Exit()
    {
        base.Exit();
    }


    bool hasPassedFormDown = false;
    bool CheckIfPosEqualsChineseHead()
    {
        if (angle != 90)
        {
            if (addXStartPosComponent == true)
            {
                if (ball.transform.position.x >= endPos.x)
                {
                    return true;
                }
            }
            else
            {
                if (ball.transform.position.x <= endPos.x)
                {
                    return true;
                }
            }
        }
        else
        {
            if (ball.transform.position.y >= endPos.y)
            {
                if (hasPassedFormDown == false)
                {
                    hasPassedFormDown = true;
                }
            }

            if (ball.transform.position.y <= endPos.y)
            {
                if (hasPassedFormDown == true)
                {
                    return true;
                }
            }
        }
        return false;
    }

    protected override void SetShadowBallPos()
    {
        float xComponent = ball.transform.position.x;
        float yComponent = ((endPos.y - startPos.y) / (endPos.x - startPos.x)) * (ball.transform.position.x - startPos.x) + startPos.y;

        Vector2 pos = new Vector2(xComponent, yComponent);
        ballShadow.transform.position = pos;
    }
}
