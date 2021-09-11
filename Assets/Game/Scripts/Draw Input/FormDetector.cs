using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FormDetector
{
    public static int Detector(LineRenderer lineRenderer, List<ShapeType> shapes)
    {
        int n = lineRenderer.positionCount;

        int i = 0;
        foreach (var shapeType in shapes)
        {
            if (n <= shapeType.shapeMaxVertices)
                break;
            i++;
        }

        return i;
    }
    
    public static float GetAngle(Vector2 a,Vector2 b,Vector2 c) {
        var ab = Mathf.Sqrt(Mathf.Pow(b.x-a.x,2)+ Mathf.Pow(b.y-a.y,2));    
        var bc = Mathf.Sqrt(Mathf.Pow(b.x-c.x,2)+ Mathf.Pow(b.y-c.y,2)); 
        var ac = Mathf.Sqrt(Mathf.Pow(c.x-a.x,2)+ Mathf.Pow(c.y-a.y,2));
        return Mathf.Acos((bc*bc+ab*ab-ac*ac)/(2*bc*ab));
    }
}
