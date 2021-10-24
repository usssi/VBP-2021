using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitsScript : MonoBehaviour
{
    [SerializeField] private Vector2 lineDirection;
    [SerializeField] private bool redColor;
    [SerializeField] private bool yellowColor;

    private void OnDrawGizmos()
    {
        if (redColor && !yellowColor)
            Gizmos.color = Color.red;
        else
            Gizmos.color = Color.blue;

        if (!redColor && yellowColor)
            Gizmos.color = Color.yellow;

        Gizmos.DrawRay(transform.position, lineDirection * 50);
        Gizmos.DrawRay(transform.position, -lineDirection * 50);

    }
}
