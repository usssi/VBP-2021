using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TUT_BallReboteAnimal : TUT_Ball
{
    private Vector2 wallReboundPos;
    private Vector2 animalReboundDir;

    public TUT_BallReboteAnimal(GameObject _ball, Animator _ballAnim, Transform _player, GameObject _redCross, GameObject _greenCircle, GameObject _sagashiGObj, TUT_ChadMovement _chadScript
        , float _leftL, float _rightL, float _bottomL, float _topL, float _ballRemSpd, float _ballRebParSpd, float _ballRecSpd, float _ballArmSpd, GameObject _ballShadow, int _comboCount, float _gravity)
        : base(_ball, _ballAnim, _player, _redCross, _greenCircle, _sagashiGObj, _chadScript, _leftL, _rightL, _bottomL, _topL
            , _ballRemSpd, _ballRebParSpd, _ballRecSpd, _ballArmSpd, _ballShadow, _comboCount, _gravity)

    {
        actualState = STATE.REBOTEANIMAL;
        speed = ballRematadaSpeed;
    }

    protected override void Enter()
    {
        base.Enter();
        ballSoundsManager.PlaySound("reboteAnimal");
        ballAnim.ResetTrigger("superBallFastSpeed");
        ballAnim.SetTrigger("ballFastSpeed");
        endPosOffset = 0;
        wallReboundPos = GameObject.Find("/Limits/CorralLimits/CorralRightLimit").transform.position;
        animalReboundDir = (wallReboundPos - (Vector2)ball.transform.position);
        startPos = ball.transform.position;
        endPos = wallReboundPos;
        //Debug.Log("ENTER BALLREBOTEANIMAL");
    }

    protected override void Update()
    {
        base.Update();
        ball.transform.Translate(animalReboundDir * speed * Time.deltaTime, Space.World);
        if (ball.transform.position.x >= wallReboundPos.x)
        {

            nextState = new TUT_BallRebotePared(ball, ballAnim, player, redCross, greenCircle, sagashiGameObject, chadScript
            , leftLimit, rightLimit, bottomLimit, topLimit, ballRematadaSpeed, ballReboteParedSpeed, ballRecibidaSpeed, ballArmadaSpeed, ballShadow, comboCount, gravity);

            stage = EVENT.EXIT;
        }

        SetShadowBallPos();
    }

    protected override void Exit()
    {
        //Debug.Log("EXIT BALLREBOTEANIMAL");
        base.Exit();
    }

    protected override void SetShadowBallPos()
    {
        float xComp = ball.transform.position.x;
        float yComp;
        Vector2 pos;
        yComp = startPos.y;


        pos = new Vector2(xComp, yComp);
        ballShadow.transform.position = pos;
    }

}
