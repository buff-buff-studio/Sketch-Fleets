using System;
using System.Collections.Generic;
using UnityEngine;

public class CreateLine : MonoBehaviour
{
    public LineRenderer lineRenderer;
    
    private List<Vector2> points = new List<Vector2>();

    public int pointCount => points.Count;

    private float pointsMinDist = 0.1f;

    public void AddPoint(Vector2 newPoint)
    {
        if (points.Count >= 1 && Vector2.Distance(newPoint, GetLastPoint()) < pointsMinDist)
            return;
        
        points.Add(newPoint);
        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPosition(points.Count-1, newPoint);
    }

    public Vector2 GetLastPoint()
    {
        return (Vector2)lineRenderer.GetPosition(points.Count-1);
    }

    public void SetLineCOlor(Gradient colorGradient)
    {
        lineRenderer.colorGradient = colorGradient;
    }

    public void SetPointsMinDistance(float distance)
    {
        pointsMinDist = distance;
    }

    public void SetLineWidht(float widht)
    {
        lineRenderer.startWidth = widht;
        lineRenderer.endWidth = widht;
    }

    public Tuple<Vector3, float> GetLineCenter(string shape)
    {
        int pCount = lineRenderer.positionCount;
        Vector3 a = Vector3.zero;
        Vector3 b = Vector3.zero;
        float distance = 0;

        if (shape == "Triangle")
        {
            distance = Vector3.Distance(lineRenderer.GetPosition(0), lineRenderer.GetPosition(1));
            
            a = Vector3.Lerp(lineRenderer.GetPosition(0), lineRenderer.GetPosition(1), .5f);
            b = Vector3.Lerp(lineRenderer.GetPosition(0), lineRenderer.GetPosition(pCount-2), .5f);
            Vector3 c = Vector3.Lerp(lineRenderer.GetPosition(1), lineRenderer.GetPosition(pCount-2), .5f);

            Vector3 d = Vector3.Lerp(a, lineRenderer.GetPosition(pCount-2), .5f);
            Vector3 e = Vector3.Lerp(b, lineRenderer.GetPosition(1), .5f);
            a = Vector3.Lerp(c, lineRenderer.GetPosition(0), .5f);
            b = Vector3.Lerp(d, e, .5f);
        }
        else if (shape == "Square")
        {
            distance = Vector3.Distance(lineRenderer.GetPosition(0), lineRenderer.GetPosition((pCount-1)/2));
            
            a = Vector3.Lerp(lineRenderer.GetPosition(0), lineRenderer.GetPosition((pCount-1)/2), .5f);
            b = Vector3.Lerp(lineRenderer.GetPosition((pCount-1)/4), lineRenderer.GetPosition(pCount-2), .5f);
            Debug.Log((pCount-1)/4);
            Debug.Log((pCount-1)/2);
        }
        else
        {
            lineRenderer.Simplify(2);
            pCount = lineRenderer.positionCount;

            if (pCount <= 4)
            {
                distance = Vector3.Distance(lineRenderer.GetPosition(0), lineRenderer.GetPosition(1));
                
                a = Vector3.Lerp(lineRenderer.GetPosition(0), lineRenderer.GetPosition(1), .5f);
                b = Vector3.Lerp(lineRenderer.GetPosition(0), lineRenderer.GetPosition(pCount-2), .5f);
                Vector3 c = Vector3.Lerp(lineRenderer.GetPosition(1), lineRenderer.GetPosition(pCount-2), .5f);

                Vector3 d = Vector3.Lerp(a, lineRenderer.GetPosition(pCount-2), .5f);
                Vector3 e = Vector3.Lerp(b, lineRenderer.GetPosition(1), .5f);
                a = Vector3.Lerp(c, lineRenderer.GetPosition(0), .5f);
                b = Vector3.Lerp(d, e, .5f);
            }
            else
            {
                distance = Vector3.Distance(lineRenderer.GetPosition(0), lineRenderer.GetPosition((pCount-1)/2));

                a = Vector3.Lerp(lineRenderer.GetPosition(0), lineRenderer.GetPosition((pCount - 1) / 2), .5f);
                b = Vector3.Lerp(lineRenderer.GetPosition((pCount - 1) / 4), lineRenderer.GetPosition(pCount - 2), .5f);
            }
        }
        
        return Tuple.Create(Vector3.Lerp(a, b, .5f), distance);
    }
    
}
