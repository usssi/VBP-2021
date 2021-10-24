using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallRematada : Ball
{
    private GameObject animal;
    private Vector2 blowDir;
    private Vector2 animalPos;
    private Vector2 defaultBlowPos; // = new Vector2(11, 4.7f);

    private float blowSpeed;

    private GameManager gameManager;
    public BallRematada(GameObject _ball, Animator _ballAnim, Transform _player, GameObject _redCross, GameObject _greenCircle, GameObject _sagashiGObj, ChadMovement _chadScript
        , float _leftL, float _rightL, float _bottomL, float _topL, float _ballRemSpd, float _ballRebParSpd, float _ballRecSpd, float _ballArmSpd, GameObject _ballShadow, int _comboCount, float _gravity)
        : base(_ball, _ballAnim, _player, _redCross, _greenCircle, _sagashiGObj, _chadScript, _leftL, _rightL, _bottomL, _topL
            , _ballRemSpd, _ballRebParSpd, _ballRecSpd, _ballArmSpd, _ballShadow, _comboCount, _gravity)

    {
        defaultBlowPos = GameObject.FindGameObjectWithTag("DefaultBlowPos").transform.position;
        actualState = STATE.REMATADA;
        speed = ballRematadaSpeed;
    }

    protected override void Enter()
    {
        base.Enter();
        soundsManager.PlaySound("rematada");
        redCross.gameObject.SetActive(false);
        greenCircle.gameObject.SetActive(false);
        
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        animal = gameManager.GetFirsAnimal();
        startPos = ball.transform.position;
        endPosOffset = 0;
        ballAnim.ResetTrigger("ballNormal");

        ballAnim.SetTrigger("ballFastSpeed");
        if (animal != null)
        {
            endPos = animal.transform.position;
            blowDir = (endPos - startPos);    
        }
        else
        {
            endPos = defaultBlowPos;
            blowDir = (endPos - startPos);
        }
    }

    protected override void Update()
    {

        base.Update();
        //Debug.Log("UPDATE BALLREMATADA");
        ball.transform.Translate(blowDir * speed * Time.deltaTime, Space.World);

        if(ball.transform.position.x >= endPos.x)
        {

            if (animal != null)
                animal.GetComponent<AnimalStateMachine>().CurrentState.AnimalToppedOff();

            nextState = new BallReboteAnimal(ball, ballAnim, player, redCross, greenCircle,
                sagashiGameObject, chadScript, leftLimit, rightLimit, bottomLimit, topLimit, 
                ballRematadaSpeed, ballReboteParedSpeed, ballRecibidaSpeed, ballArmadaSpeed, ballShadow, comboCount, gravity);

            
     
            stage = EVENT.EXIT;
        }

        SetShadowBallPos();
    }

    protected override void Exit()
    {
        //Debug.Log("EXIT BALLREMATADA");
        base.Exit();
    }

    protected override void SetShadowBallPos()
    {
        float posOffset = 0.5f;
        float xComp = ball.transform.position.x;
        float yComp;
        Vector2 pos;
        yComp = ((endPos.y - (startPos.y - posOffset)) / (endPos.x - startPos.x)) * (ball.transform.position.x - startPos.x) + startPos.y - posOffset;
        
        

        pos = new Vector2(xComp, yComp);
        ballShadow.transform.position = pos;
    }
}
