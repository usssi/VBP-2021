using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadow : MonoBehaviour
{
    [Header("ShadowVars")]
    [SerializeField] protected float posOffset;
    private Vector2 shadowBase;
    
    protected virtual void SetShadowGroundedPos()
    {
        this.transform.position = new Vector2(transform.parent.position.x, transform.parent.position.y - posOffset);
        shadowBase = new Vector2(transform.parent.position.x, transform.parent.position.y - posOffset);
        //Debug.Log("Setting grounded pos");
    }

    protected virtual void SetShadowInAirPos()
    {
        transform.position = new Vector2(transform.parent.position.x, shadowBase.y);
        //Debug.Log("SETTING IN AIR POS");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawRay(gameObject.transform.parent.position, Vector2.down * posOffset);
    }

}
