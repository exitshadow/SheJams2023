using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
using Unity.Mathematics;
using UnityEditor;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[ExecuteInEditMode()]
public class SplineSampler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SplineContainer splineContainer;
    [SerializeField] private int splineIndex;
    [SerializeField] [Range(0f, 1f)] private float t;
    [SerializeField] private MeshFilter meshFilter;

    [Header("Parameters")]
    [SerializeField] private float roadWidth = 10f;
    [SerializeField] private float roadThickness = .5f;
    [SerializeField] private int subdivisions = 100;

    [Header("Visualization Parameters")]
    [SerializeField] private float gizmoBallSize = 1f;


    private float3 position;
    private float3 tangent;
    private float3 upVector;

    private List<Vector3> topRightHandVertices;
    private List<Vector3> topLeftHandVertices;
    private List<Vector3> bottomRightHandVertices;
    private List<Vector3> bottomLeftHandVertices;


    private void Update()
    {
        GetVertices();
    }

    private void OnDrawGizmos()
    {
        DrawPositions();
    }

    public void GetVertices()
    {
        topRightHandVertices = new List<Vector3>();
        topLeftHandVertices = new List<Vector3>();
        bottomRightHandVertices = new List<Vector3>();
        bottomLeftHandVertices = new List<Vector3>();


        float step = 1f / (float)subdivisions;

        for (int i = 0; i < subdivisions; i++)
        {
            float t = step * i;
            SamplePoints(t, out Vector3 posTopRight, out Vector3 posTopLeft, out Vector3 posBottomRight, out Vector3 posBottomLeft);
            topRightHandVertices.Add(posTopRight);
            topLeftHandVertices.Add(posTopLeft);
            bottomRightHandVertices.Add(posBottomRight);
            bottomLeftHandVertices.Add(posBottomLeft);
        }
    }

    public void SamplePoints(   float t,
                                out Vector3 topRightHandPoint,
                                out Vector3 topLeftHandPoint,
                                out Vector3 bottomRightHandPoint,
                                out Vector3 bottomLeftHandPoint)
    {
        splineContainer.Evaluate(splineIndex, t, out position, out tangent, out upVector);

        // since tangent value is the formard direction, we can find the left and right hand of it to draw our points
        float3 rightVector = Vector3.Cross(tangent, upVector).normalized;

        float thickness = roadThickness / 2;
        
        topRightHandPoint = position - (rightVector * roadWidth / 2) + upVector * thickness;
        topLeftHandPoint = position + (rightVector * roadWidth / 2) + upVector * thickness;

        bottomRightHandPoint = position - (rightVector * roadWidth / 2) - upVector * thickness;
        bottomLeftHandPoint = position + (rightVector * roadWidth / 2) - upVector * thickness;
    }

    private void DrawPositions()
    {
        if(topRightHandVertices == null) return;

        for (int i = 0; i < topRightHandVertices.Count; i++)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(topRightHandVertices[i], gizmoBallSize);
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(topLeftHandVertices[i], gizmoBallSize);

            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(bottomRightHandVertices[i], gizmoBallSize);
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(bottomLeftHandVertices[i], gizmoBallSize);


            Gizmos.DrawLine(topRightHandVertices[i], topLeftHandVertices[i]);
        }
    }

    public void GenerateMesh()
    {
        Mesh roadMesh = new Mesh();
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        int offset = 0;
        int loopLength = topRightHandVertices.Count;

        // write a face from vertices
        for (int i = 1; i < loopLength; i++)
        {
            Vector3 p1 = topRightHandVertices[i-1] - transform.position;
            Vector3 p2 = topLeftHandVertices[i-1] - transform.position;

            Vector3 p3 = topRightHandVertices[i] - transform.position;
            Vector3 p4 = topLeftHandVertices[i] - transform.position;

            Vector3 p5 = bottomRightHandVertices[i-1] - transform.position;
            Vector3 p6 = bottomLeftHandVertices[i-1] - transform.position;

            Vector3 p7 = bottomRightHandVertices[i] - transform.position;
            Vector3 p8 = bottomLeftHandVertices[i] - transform.position;


            offset = 8 * (i-1);

        // TOP FACE
            // assigning first triangle indexes
            int t1 = offset;
            int t2 = offset + 3;
            int t3 = offset + 2;
            // assigning second triangle indexes
            int t4 = offset;
            int t5 = offset + 1;
            int t6 = offset + 3;

        // RIGHT FACE
            // 1st triangle indexes
            int r1 = t3;
            int r2 =  offset + 6;
            int r3 = offset + 4;
            // 2nd triangle indexes
            int r4 = r3;
            int r5 = t1;
            int r6 = t3;
        
        // LEFT FACE
            // 1st triangle indexes
            int l1 = offset + 3;
            int l2 = offset + 1;
            int l3 = offset + 7;
            // 2nd triangle indexes
            int l4 = offset + 5;
            int l5 = offset + 7;
            int l6 = offset + 1;

            vertices.AddRange(new List<Vector3> { p1, p2, p3, p4, p5, p6, p7, p8 });
            triangles.AddRange(new List<int> {  t1, t2, t3, t4, t5, t6,
                                                r1, r2, r3, r4, r5, r6,
                                                l1, l2, l3, l4, l5, l6 });
        }

        roadMesh.SetVertices(vertices);
        roadMesh.SetTriangles(triangles, 0);
        roadMesh.RecalculateNormals();

        meshFilter.mesh = roadMesh;
        meshFilter.name = "Road Mesh";

    }

}
