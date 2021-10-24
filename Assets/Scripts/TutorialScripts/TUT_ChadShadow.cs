using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TUT_ChadShadow : Shadow
{

    [Header("ChadComponents")]
    private TUT_ChadMovement movementScript;


    private void Start()
    {
        movementScript = gameObject.GetComponentInParent<TUT_ChadMovement>();
    }

    private void Update()
    {
        SetShadowPos();
    }


    void SetShadowPos()
    {
        if (movementScript.InAir == false)
        {
            SetShadowGroundedPos();
        }
        else
        {
            SetShadowInAirPos();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawRay(gameObject.transform.parent.position, Vector2.down * posOffset);
    }
}
