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
    [SerializeField] private bool refreshOnModification = true;

    [Header("Visualization Parameters")]
    [SerializeField] private float gizmoBallSize = 1f;

    private float3 position;
    private float3 tangent;
    private float3 upVector;

    private List<Vector3> topRightHandVertices;
    private List<Vector3> topLeftHandVertices;
    private List<Vector3> bottomRightHandVertices;
    private List<Vector3> bottomLeftHandVertices;

    private GameObject colliderHolder;

    private void OnEnable()
    {
        Spline.Changed += OnSplineChanged;
    }

    private void Update()
    {
        #if (UNITY_EDITOR)
        if (EditorApplication.isPlaying) return;
        GetVertices();
        #endif
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

    public void GenerateRoadMesh()
    {
        Mesh roadMesh = new Mesh();
        Mesh sideRoadMesh = new Mesh();

        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<int> sideTriangles = new List<int>();

        int vertexCount = 4;

        // add vertices
        for (int i = 0; i < topLeftHandVertices.Count; i++)
        {
            Vector3 a = topRightHandVertices[i] - transform.position;
            Vector3 b = topLeftHandVertices[i] - transform.position;
            Vector3 c = bottomRightHandVertices[i] - transform.position;
            Vector3 d = bottomLeftHandVertices[i] - transform.position;

            vertices.AddRange(new List<Vector3> { a, b, c, d });
        }

        // add triangles
        for (int i = 0; i < topLeftHandVertices.Count - 1; i++)
        {
            int root = i * vertexCount;
            int rootNext = (i + 1) * vertexCount;

        // TOP FACE
            // triangle 1
            int t1 = root;          // point a
            int t2 = root + 1;      // point b
            int t3 = rootNext + 1;  // point b’
            // triangle 2
            int t4 = root;          // point a
            int t5 = t3;            // point b’
            int t6 = rootNext;      // point a’

            triangles.AddRange(new List<int> {t1, t2, t3, t4, t5, t6});
        
        // RIGHT SIDE FACE
            // triangle 1
            int r1 = root + 2;      // point c
            int r2 = root;          // point a
            int r3 = rootNext;      // point a’
            // triangle 2
            int r4 = r3;            // point a’
            int r5 = rootNext + 2;  // point c’
            int r6 = r1;            // point c

            triangles.AddRange(new List<int> {r1, r2, r3, r4, r5, r6});

        // LEFT SIDE FACE
            // triangle 1
            int l1 = root + 1;      // point b
            int l2 = root + 3;      // point d
            int l3 = rootNext + 3;  // point d’
            // triangle 2
            int l4 = l3;            // point d’
            int l5 = rootNext + 1;  // point b’
            int l6 = l1;            // point b

            triangles.AddRange(new List<int> {l1, l2, l3, l4, l5, l6});

        }

        roadMesh.SetVertices(vertices);
        roadMesh.SetTriangles(triangles, 0);
        roadMesh.RecalculateNormals();

        meshFilter.mesh = roadMesh;
        

    }

    /// <summary>
    /// Iterates through all the road spline samples and creates colliders
    /// </summary>
    public void GenerateRoadColliders()
    {
        if (colliderHolder == null)
        {
            colliderHolder  = new GameObject("Collider Holder");
            colliderHolder.transform.parent = transform;
        }

        // clear previous colliders
        if (colliderHolder.transform.childCount != 0)
        {
            foreach (Transform child in colliderHolder.transform)
            {
                GameObject.Destroy(colliderHolder);
            }
        }

        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        int offset = 0;
        int loopLength = topLeftHandVertices.Count;
        Mesh collMesh = new Mesh();

        // write a face from vertices
        for (int i = 1; i < loopLength; i++)
        {
            collMesh.Clear();

            GameObject holder = new GameObject();
            holder.transform.parent = colliderHolder.transform;

            MeshCollider collider = holder.AddComponent<MeshCollider>();

            Vector3 p1 = topRightHandVertices[i-1];
            Vector3 p2 = topLeftHandVertices[i-1];

            Vector3 p3 = topRightHandVertices[i];
            Vector3 p4 = topLeftHandVertices[i];

            Vector3 p5 = bottomRightHandVertices[i-1];
            Vector3 p6 = bottomLeftHandVertices[i-1];

            Vector3 p7 = bottomRightHandVertices[i];
            Vector3 p8 = bottomLeftHandVertices[i];

            offset = 0;

        // TOP FACE
            // assigning first triangle indexes
            int t1 = offset;
            int t2 = offset + 3;
            int t3 = offset + 2;
            // assigning second triangle indexes
            int t4 = offset;
            int t5 = offset + 1;
            int t6 = offset + 3;

        // DOWN FACE
            // 1st tri
            int d1 = offset + 7;
            int d2 = offset + 5;
            int d3 = offset + 6;
            // 2nd tri
            int d4 = offset + 5;
            int d5 = offset + 4;
            int d6 = offset + 6;
        
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

        // FRONT FACE
            // 1st tri
            int f1 = offset + 6;
            int f2 = offset + 2;
            int f3 = offset + 3;
            // 2nd tri
            int f4 = offset + 3;
            int f5 = offset + 7;
            int f6 = offset + 6;
        
        // BACK FACE
            // 1st tri
            int b1 = offset + 5;
            int b2 = offset + 1;
            int b3 = offset;
            // 2nd tri
            int b4 = offset;
            int b5 = offset + 4;
            int b6 = offset + 5;
        

        // todo: back, front and down faces
        // separated meshes to a collection of colliders

            collMesh.SetVertices(new List<Vector3> { p1, p2, p3, p4, p5, p6, p7, p8 });

            collMesh.SetTriangles(new List<int> {  t1, t2, t3, t4, t5, t6,
                                                    d1, d2, d3, d4, d5, d6, 
                                                    r1, r2, r3, r4, r5, r6,
                                                    l1, l2, l3, l4, l5, l6,
                                                    f1, f2, f3, f4, f5, f6,
                                                    b1, b2, b3, b4, b5, b6 }, 0);

            collider.sharedMesh = collMesh;
        }


    }

    private void OnSplineChanged(Spline spline, int knotIndex, SplineModification modification)
    {
        if (spline != splineContainer.Spline || !refreshOnModification) return;

        Debug.Log("Spline has been changed.");

        GenerateRoadMesh();
        Debug.Log("The mesh has been updated.");
    }

}
