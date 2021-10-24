using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TUT_BallCaida : TUT_Ball
{
    float timeInFloor = 1.5f;
    float timeInFloorCronometer;

    float shadowOffset = 0.2f;
    bool playSound;

    public TUT_BallCaida(GameObject _ball, Animator _ballAnim, Transform _player, GameObject _redCross, GameObject _greenCircle, GameObject _sagashiGObj, TUT_ChadMovement _chadScript
        , float _leftL, float _rightL, float _bottomL, float _topL, float _ballRemSpd, float _ballRebParSpd, float _ballRecSpd, float _ballArmSpd, GameObject _ballShadow, int _comboCount, float _gravity, bool _playSound = true)
        : base(_ball, _ballAnim, _player, _redCross, _greenCircle, _sagashiGObj, _chadScript, _leftL, _rightL, _bottomL, _topL
            , _ballRemSpd, _ballRebParSpd, _ballRecSpd, _ballArmSpd, _ballShadow, _comboCount, _gravity)

    {
        actualState = STATE.CAIDASUELO;
        playSound = _playSound;
    }

    protected override void Enter()
    {
        base.Enter();
        
        if(playSound == true)
            ballSoundsManager.PlaySound("suelo");
        comboCount = 0;
        ballShadow.transform.position = new Vector2(ball.transform.position.x, ball.transform.position.y - shadowOffset);
        ballAnim.SetTrigger("ballCaida");
        timeInFloorCronometer = timeInFloor;
        greenCircle.gameObject.SetActive(false);
        //Debug.Log("ENTER BALLCAIDASUELO");
        endPos = ball.transform.position;
        startPos = ball.transform.position;
    }

    protected override void Update()
    {
        timeInFloorCronometer -= Time.deltaTime;

        if (timeInFloorCronometer <= 0)
        {

            //Vector2 pos = new Vector2(-2.5f, -0.4f);
            //Vector2 pos = new Vector2(8.5f, 5.5f);
            Vector2 pos = GameObject.FindGameObjectWithTag("BallPosCaida").transform.position;
            ball.transform.position = sagashiGameObject.transform.position;
            SetPos(greenCircle, redCross, pos);


            nextState = new TUT_BallArmada(ball, ballAnim, player, redCross, greenCircle, sagashiGameObject, chadScript,
                leftLimit, rightLimit, bottomLimit, topLimit, ballRematadaSpeed, ballReboteParedSpeed, ballRecibidaSpeed, ballArmadaSpeed, ballShadow, comboCount, gravity, pos);

            stage = EVENT.EXIT;
        }
        //Debug.Log("UPDATE BALLCAIDASUELO");
    }

    protected override void Exit()
    {
        ballAnim.ResetTrigger("ballCaida");
        //Debug.Log("EXIT BALLCAIDASUELO");
        base.Exit();
    }

    void Bling()
    {
        Debug.Log("ANIMACION PARPADEAR");
        //Acá se corre la naimación de parpadear.

    }

}
