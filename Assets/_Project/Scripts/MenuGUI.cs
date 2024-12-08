using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuGUI : MonoBehaviour
{
    private int _buttonWidth = 200;
    private int _buttonHeigth = 80;
    private GUIStyle customButtonStyle;
    
    void OnGUI()
    {
        customButtonStyle = new GUIStyle(GUI.skin.button);
        customButtonStyle.fontSize = 16; 
        GUILayout.BeginArea(new Rect(10, 10, 200, 300));
        
        if (GUILayout.Button("Spawn 1", GUILayout.Width(_buttonWidth), GUILayout.Height(_buttonHeigth))) SpawnTBoids(1);
        if (GUILayout.Button("Spawn 10", GUILayout.Width(_buttonWidth), GUILayout.Height(_buttonHeigth))) SpawnTBoids(10);
        if (GUILayout.Button("Spawn 30", GUILayout.Width(_buttonWidth), GUILayout.Height(_buttonHeigth))) SpawnTBoids(30);
        GUILayout.EndArea();
    }

    private void SpawnTBoids(int num)
    {
        BoidManager.Instance.SpawnBoids(num);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
