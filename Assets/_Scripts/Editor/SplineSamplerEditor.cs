using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SplineSampler))]
public class SplineSamplerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Generate Road Mesh"))
        {
            ((SplineSampler)target).GenerateRoadMesh();
        }
        
        if (GUILayout.Button("Generate Road Colliders"))
        {
            ((SplineSampler)target).GenerateRoadColliders();
        }
    }
}
