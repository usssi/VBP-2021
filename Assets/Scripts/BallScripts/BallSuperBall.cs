using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSuperBall : Ball
{
    private Animator explosion;
  
    private Vector2 defaultBlowPos;
    private Vector2 blowDir;
    private GameManager gameManager;
   public BallSuperBall(GameObject _ball, Animator _ballAnim, Transform _player, GameObject _redCross, GameObject _greenCircle, GameObject _sagashiGObj, ChadMovement _chadScript
        , float _leftL, float _rightL, float _bottomL, float _topL, float _ballRemSpd, float _ballRebParSpd, float _ballRecSpd, float _ballArmSpd, GameObject _ballShadow, int _comboCount, float _gravity)
        : base(_ball, _ballAnim, _player, _redCross, _greenCircle, _sagashiGObj, _chadScript, _leftL, _rightL, _bottomL, _topL
            , _ballRemSpd, _ballRebParSpd, _ballRecSpd, _ballArmSpd, _ballShadow, _comboCount, _gravity)
    {
        defaultBlowPos = GameObject.FindGameObjectWithTag("DefaultBlowPos").transform.position;
        endPos = defaultBlowPos;
        speed = 10;
        comboCount = 0;
        ball.transform.GetChild(1).gameObject.SetActive(true);
   }

    protected override void Enter()
    {
        base.Enter();
        soundsManager.PlaySound("superBall");
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        explosion = GameObject.FindGameObjectWithTag("Explosion").GetComponent<Animator>();
        explosion.SetTrigger("playExplosion");
        redCross.gameObject.SetActive(false);
        greenCircle.gameObject.SetActive(false);
        ballAnim.SetTrigger("superBallFastSpeed");
        endPosOffset = 0;
        blowDir = (endPos - startPos);
    }

    protected override void Update()
    {
        base.Update();

        ball.transform.Translate(blowDir * speed * Time.deltaTime, Space.World);

        if (ball.transform.position.x >= endPos.x)
        {
            GameObject[] anims = gameManager.GetAnimalList().ToArray();

            foreach (GameObject anim in anims)
            {
                AnimalState animState = anim.GetComponent<AnimalStateMachine>().CurrentState;
                animState.AnimalToppedOff();
            }

            nextState = new BallReboteAnimal(ball, ballAnim, player, redCross, greenCircle,
                sagashiGameObject, chadScript, leftLimit, rightLimit, bottomLimit, topLimit,
                ballRematadaSpeed, ballReboteParedSpeed, ballRecibidaSpeed, ballArmadaSpeed, ballShadow, comboCount, gravity);

           

            stage = EVENT.EXIT;
        }

        SetShadowBallPos();
    }

    protected override void Exit()
    {
        ball.transform.GetChild(1).gameObject.SetActive(false);
        base.Exit();
    }


    //Este script necesita tomar referencia del centro de la cancha y, cuando logramos hacer los 4 golpes debe salir la pelota roja.
    //Al golpear el suelo ocurre que los animalitos son todos brutalemente deleteados. Esto lo puedo hacer que,  cuando llega al endPos
    //llamo en el Exit la función que los hace irse a todos hacia atrás.
}
