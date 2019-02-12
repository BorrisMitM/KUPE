using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour {
    private LineRenderer lineRenderer;
    [SerializeField]
    public Transform start;     //arm location
    private Transform anchorPoint;  //where the hand of karen is currently
    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }
    public void Draw()
    {
        lineRenderer.enabled = true;
        Vector3[] verticies = { start.position, anchorPoint.position };
        lineRenderer.SetPositions(verticies);
    }
    public void SetAnchorPoint(Transform _anchorPoint)
    {
        anchorPoint = _anchorPoint;
    }
    public void Disable()
    {
        lineRenderer.enabled = false;
    }
}
