using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class FormDetector
{
    public static int Detector(LineRenderer lineRenderer, IEnumerable<ShapeType> shapes)
    {
        int vertexCount = lineRenderer.positionCount;
        return shapes.TakeWhile(shapeType => vertexCount > shapeType.shapeMaxVertices).Count();
    }
}