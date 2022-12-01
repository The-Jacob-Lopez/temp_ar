using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class drawing
{

    public Vector2[] points = new Vector2[] { new Vector2(5,1), new Vector2(4,3), new Vector2(2,4), new Vector2(4,5), new Vector2(5,7), new Vector2(6,5), new Vector2(8,4), new Vector2(6,3) };

    Vector2[] AngularSort(Vector2[] points, Vector2 center)
    {
        return points.OrderBy(point => Math.Atan2(point.x - center.x, point.y - center.y)).ToArray<Vector2>();
    }

    Vector2 CenterOfMassOfPoints(Vector2[] points)
    {
        float sumOfMassX = 0;
        float sumOfMassY = 0;
        int numPoints = points.Length;
        foreach (Vector2 point in points)
        {
            sumOfMassX += point.x;
            sumOfMassY += point.y;
        }
        return new Vector2(sumOfMassX / (float)numPoints, sumOfMassY / (float)numPoints );
    }

    Vector3 makeVertexFrom2DPoint(Vector2 point)
    {
        return new Vector3(point.x, point.y, 0);
    }

    Vector2 make2DPointFromVector(Vector3 point)
    {
        return new Vector2(0, 0);
    }

    Mesh GenerateMeshFrom2DPoints(Vector2[] points)
    {
        Vector2 centerOfMass = CenterOfMassOfPoints(points);
        Vector2[] sortedPoints = AngularSort(points, centerOfMass);

        //Making the new mesh object
        Vector3[] vertices = new Vector3[points.Length + 1];
        int[] newTriangles = new int[3 * points.Length];
        int centerOfMassIndex = sortedPoints.Length;
        vertices[centerOfMassIndex] = makeVertexFrom2DPoint(centerOfMass);

        for (int i = 0; i < sortedPoints.Length; i++)
        {
            // Find index of relevant points
            int firstPointIndex = i;
            int secondPointIndex = (i + 1) % sortedPoints.Length;

            //add current point to vertices
            Vector2 firstPoint = sortedPoints[i];
            vertices[firstPointIndex] = makeVertexFrom2DPoint(firstPoint);

            //add current indices to triangles
            newTriangles[3 * i] = firstPointIndex;
            newTriangles[3 * i + 1] = secondPointIndex;
            newTriangles[3 * i + 2] = centerOfMassIndex;
        }

        Mesh newMesh = new Mesh();
        newMesh.vertices = vertices;
        newMesh.triangles = newTriangles;
        return newMesh;
    }
}
