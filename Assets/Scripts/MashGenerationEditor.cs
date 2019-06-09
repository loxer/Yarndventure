﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MeshGenerator))]
public class MashGenerationEditor : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        //base.OnInspectorGUI();

        MeshGenerator generatorScript = (MeshGenerator) target;
        if (GUILayout.Button("Generate") && EditorApplication.isPlaying)
            generatorScript.generateMesh();
    }
}