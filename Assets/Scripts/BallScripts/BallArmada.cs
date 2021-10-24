using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallArmada : Ball
{
    private float xMinPosToBlow;
    private float xMaxPosToBlow;
    private float yMinPosToBlow;
    private float yMaxPosToBlow;
    private float blowPosOffset = 0.5f;


    private Animator sagashiAnim;

    private GameObject chadBlowPos;


    private float _endPosOffset = 0.8f;

    public BallArmada(GameObject _ball, Animator _ballAnim, Transform _player, GameObject _redCross, GameObject _greenCircle, GameObject _sagashiGObj, ChadMovement _chadScript,
        float _leftL, float _rightL, float _bottomL, float _topL, float _ballRemSpd, float _ballRebParSpd, float _ballRecSpd, float _ballArmSpd, GameObject _ballShadow, int _comboCount, float _gravity,Vector2 _randomPos)
        : base(_ball, _ballAnim,_player, _redCross, _greenCircle,  _sagashiGObj, _chadScript, _leftL, _rightL, _bottomL, _topL
            ,  _ballRemSpd,  _ballRebParSpd,  _ballRecSpd,  _ballArmSpd, _ballShadow, _comboCount, _gravity)

    {
        actualState = STATE.ARMADA;
        speed = ballArmadaSpeed;
        endPosOffset = _endPosOffset;
        endPos = _randomPos;
        startPos = ball.transform.position;
        xMinPosToBlow = endPos.x - blowPosOffset;
        xMaxPosToBlow = endPos.x + blowPosOffset;
        yMinPosToBlow = endPos.y - blowPosOffset;
        yMaxPosToBlow = endPos.y + blowPosOffset;
        sagashiAnim = GameObject.FindGameObjectWithTag("Chino").GetComponent<Animator>();
        //Debug.Log("StartPos: " + startPos);
        //Debug.Log("EndPos: " + endPos);
    }

    protected override void Enter()
    {
        if (comboCount != 3)
            ballAnim.SetTrigger("ballNormal");
        else
        {
            ballAnim.SetTrigger("superBallNormal");
            GameObject.FindObjectOfType<AudioManager>().Play("superAnnouncer");

        }

        chadBlowPos = GameObject.FindGameObjectWithTag("chadBlowPos");
        sagashiAnim.SetTrigger("armar");
        CheckIfAddxStartPosComp();

        CalculateAngle(false);
        //Debug.Log("ENTER BALLARMADA");
        base.Enter();
    }

    protected override void Update()
    {
        ball.transform.position = ApplyMovement();

        CheckIfBallHasTouchedGround(endPosOffset, new BallCaida(ball, ballAnim, player, redCross, greenCircle, sagashiGameObject, chadScript
            , leftLimit, rightLimit, bottomLimit, topLimit, ballRematadaSpeed, ballReboteParedSpeed, ballRecibidaSpeed, ballArmadaSpeed, ballShadow, comboCount, gravity));

        CheckIfPlayerIsInsideBlowPosition();

        SetShadowBallPos();
        //Debug.Log("UPDATE BALLARMADA");
    }

    protected override void Exit()
    {
        ballAnim.ResetTrigger("superBallNormal");
        ballAnim.ResetTrigger("ballNormal");

        //Debug.Log("EXIT BALLARMADA");
        base.Exit();
    }


  
    void CheckIfPlayerIsInsideBlowPosition()
    {
        //if(player.position.x >= xMinPosToBlow && player.position.x <= xMaxPosToBlow && player.position.y >= yMinPosToBlow && player.position.y <= yMaxPosToBlow)
        if (chadBlowPos.transform.position.x >= xMinPosToBlow && chadBlowPos.transform.position.x <= xMaxPosToBlow && chadBlowPos.transform.position.y >= yMinPosToBlow && chadBlowPos.transform.position.y <= yMaxPosToBlow)
        {
            if (CheckIfBallIsInsideBlowPosition())
            {
                if (chadScript.InAir == true && chadScript.hasBlow == true)
                {
                    if (comboCount != 3)
                    {
                        nextState = new BallRematada(ball, ballAnim, player, redCross, greenCircle, sagashiGameObject, chadScript
                     , leftLimit, rightLimit, bottomLimit, topLimit, ballRematadaSpeed, ballReboteParedSpeed, ballRecibidaSpeed, ballArmadaSpeed, ballShadow, comboCount, gravity);

                        stage = EVENT.EXIT;

                    }
                    else
                    {
                        nextState = new BallSuperBall(ball, ballAnim, player, redCross, greenCircle, sagashiGameObject, chadScript
                     , leftLimit, rightLimit, bottomLimit, topLimit, ballRematadaSpeed, ballReboteParedSpeed, ballRecibidaSpeed, ballArmadaSpeed, ballShadow, comboCount, gravity);

                        stage = EVENT.EXIT;
                    }
                }
            }
          
        }
       

    }

    bool CheckIfBallIsInsideBlowPosition()
    {
        if (ball.transform.position.x >= xMinPosToBlow && ball.transform.position.x <= xMaxPosToBlow && ball.transform.position.y >= yMinPosToBlow && ball.transform.position.y <= yMaxPosToBlow)
        {
            return true;
        }

         return false;

    }


    protected override void SetShadowBallPos()
    {
        float xComponent = ball.transform.position.x;
        float yComponent = ((endPos.y- blowPosOffset - startPos.y) / (endPos.x - startPos.x)) * (ball.transform.position.x - startPos.x) + startPos.y - endPosOffset/ 2;

        Vector2 pos = new Vector2(xComponent, yComponent);
        ballShadow.transform.position = pos;
    }

}
