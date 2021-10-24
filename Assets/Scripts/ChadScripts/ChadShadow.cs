using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChadShadow : Shadow
{

    [Header("ChadComponents")]
    private ChadMovement movementScript;


    private void Start()
    {
        movementScript = gameObject.GetComponentInParent<ChadMovement>();
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
