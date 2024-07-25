using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineMap : MonoBehaviour
{
    private LineRenderer Lr;
    // [SerializeField]private Transform[] Points;

    private void Awake()
    {
        Lr = GetComponent<LineRenderer>();
    }
    private void Start()
    {
        Lr.SetWidth(0.5f,0.2f);
        // setupline(Points);
    }
    // public void setupline(Transform[] points)
    // {
    //     Lr.positionCount = points.Length;
    //     //this.Points = points;
    // }
    // private void Update()
    // {
    //     for(int i=0; i < Points.Length; i++)
    //     {
    //         Lr.SetPosition(i, Points[i].position);
    //     }
    // }
}