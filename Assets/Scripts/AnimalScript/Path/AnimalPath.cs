using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalPath : MonoBehaviour
{
    [SerializeField] private Transform[] pathPoints;
    public Transform[] PathPoints { get => pathPoints; }
    private Vector2 pathStartPos;
    public Vector2 PathStartPos { get => pathStartPos; }

    private void Start()
    {
        pathStartPos = pathPoints[0].position;
        //Debug.LogWarning("PathStartPos: " + pathStartPos);
    }



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;

        if(pathPoints.Length > 2)
            for (int i = 0; i < pathPoints.Length - 1; i++)
                Gizmos.DrawLine(pathPoints[i].position, pathPoints[i + 1].position);
    }
}
