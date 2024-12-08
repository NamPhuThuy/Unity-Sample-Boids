using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BoidManager))]
public class BoidManagerEditor : Editor
{
    private BoidManager myScript;
    private void OnEnable()
    {
        // Get the script instance
         myScript = (BoidManager)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        

        // Add some space before the custom button
        EditorGUILayout.Space();

        // Create a custom button
        if (GUILayout.Button("Spawn 1 boid")) myScript.SpawnBoids(1);
        if (GUILayout.Button("Spawn 3 boid")) myScript.SpawnBoids(3);
        if (GUILayout.Button("Spawn 7 boid")) myScript.SpawnBoids(7);
        

        // Optional: Colored or styled button
        if (GUILayout.Button("Danger Button", GUILayout.Height(30)))
        {
            // You can change button appearance
            GUI.backgroundColor = Color.red;
            Debug.LogWarning("Danger button clicked!");
        }

        // You can even add conditional buttons
        /*if (myScript.someValue > 5)
        {
            if (GUILayout.Button("Special Action"))
            {
                Debug.Log("Special action for high values!");
            }
        }*/
    }
}
