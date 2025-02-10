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
        
        // myScript.boidsToSpawn = EditorGUILayout.IntSlider("Cohesion force applied", myScript._forceCohesion, 1, 100);
        // myScript.boidsToSpawn = EditorGUILayout.IntSlider("Separation force applied", myScript._forceSeparation, 1, 100);
        // myScript.boidsToSpawn = EditorGUILayout.IntSlider("Alignment force applied", myScript._forceAlignment, 1, 100);
        
        
        // myScript.boidsToSpawn = EditorGUILayout.IntField("Boids to Spawn", myScript.boidsToSpawn);
        // myScript.boidsToSpawn = Mathf.Max(0, myScript.boidsToSpawn);
        
        if (GUILayout.Button("Spawn Boids")) myScript.SpawnBoids(myScript.boidsToSpawn);
        
        /*EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Spawn 10 boids")) myScript.SpawnBoids(10);
        if (GUILayout.Button("Spawn 50 boids")) myScript.SpawnBoids(50);
        if (GUILayout.Button("Spawn 100 boids")) myScript.SpawnBoids(100);
        EditorGUILayout.EndHorizontal();*/

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
