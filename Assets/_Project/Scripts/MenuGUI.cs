using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuGUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 200, 200));

        if (GUILayout.Button("Spawn 10", GUILayout.Width(200), GUILayout.Height(80))) SpawnTBoids(10);
        if (GUILayout.Button("Spawn 30", GUILayout.Width(200), GUILayout.Height(80))) SpawnTBoids(30);
        GUILayout.EndArea();
    }

    private void SpawnTBoids(int num)
    {
        BoidSpawner.Instance.SpawnBoids(num);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
