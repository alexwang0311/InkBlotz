using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InfiniteLine : MonoBehaviour {

    public LineRenderer lineRenderer;
    public EdgeCollider2D edgeCollider;
    private List<Vector2> points;

    private void Start()
    {
        float x = transform.position.x;
        float y = transform.position.y;
        edgeCollider.offset = new Vector2(-x, -y);
    }

    public void UpdateLine(Vector2 point)
    {
        if (points == null)
        {
            points = new List<Vector2>();
            SetPoint(point);
            return;
        }

        if (Vector2.Distance(points.Last(), point) > 0.1f)
        {
            SetPoint(point);
        }
    }

    private void SetPoint(Vector2 point)
    {
        points.Add(point);
        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPosition(points.Count - 1, point);

        if (points.Count > 1)
        {
            edgeCollider.points = points.ToArray();
        }
    }
}
