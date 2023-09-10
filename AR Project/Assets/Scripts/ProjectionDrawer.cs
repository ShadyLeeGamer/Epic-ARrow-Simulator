using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectionDrawer : MonoBehaviour
{
    [SerializeField] int numPoints;
    [SerializeField] float timeBetweenPoints;
    [SerializeField] LayerMask planeMask;

    LineRenderer lineRenderer;

    ARPlanesTrackedManager planeTrackedManager;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        planeTrackedManager = ARPlanesTrackedManager.Instance;
    }

    public void Draw(Vector3 startPos, Vector3 startVelocity)
    {
        lineRenderer.positionCount = numPoints;

        List<Vector3> points = new();
        for (float t = 0; t < numPoints; t += timeBetweenPoints)
        {
            Vector3 newPoint = startPos + t * startVelocity;
            newPoint.y = startPos.y + startVelocity.y * t + Physics.gravity.y / 2f * t * t;
            points.Add(newPoint);

            if (Physics.OverlapSphere(newPoint, 0.05f, planeMask).Length > 0 ||
                newPoint.y <= planeTrackedManager.BedrockPosY)
            {
                lineRenderer.positionCount = points.Count;
                break;
            }
        }

        lineRenderer.SetPositions(points.ToArray());
    }
}