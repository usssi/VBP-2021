using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TUT_BallShadow : MonoBehaviour
{
    private float xScale;
    private float yScale;

    private float abs;
    private float denominador;
    private Vector2 scale;

    public Vector2 startPos;
    public Vector2 endPos;

    private GameObject ball;
    private TUT_BallStateMachine ballStateMachineScript;


    private void Start()
    {
        xScale = transform.localScale.x;
        yScale = transform.localScale.y;
        scale = new Vector2(xScale, yScale);
        ball = GameObject.FindGameObjectWithTag("Ball");
        ballStateMachineScript = ball.GetComponent<TUT_BallStateMachine>();

    }

    private void Update()
    {
        startPos = ballStateMachineScript.StartPos;
        endPos = ballStateMachineScript.EndPos;
        transform.rotation = Quaternion.identity;
        SetShadowSize();

        //this.transform.position = new Vector2(ball.transform.position.x, ball.transform.position.y - positionOffset);
    }

    void SetShadowSize()
    {
        if (endPos.y <= startPos.y)
        {
            abs = Mathf.Abs((endPos.y - ball.transform.position.y));
            denominador = Mathf.Clamp(abs, 1f, 100);
            this.transform.localScale = scale / denominador;
        }
        else
        {
            abs = Mathf.Abs((startPos.y - ball.transform.position.y + (startPos.y - endPos.y)));
            denominador = Mathf.Clamp(abs, 1f, 100);
            this.transform.localScale = scale / denominador;
        }


    }

}
