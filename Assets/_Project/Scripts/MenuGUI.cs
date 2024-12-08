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

        if (GUILayout.Button("Spawn 10", GUILayout.Width(200), GUILayout.Height(80))) SpawnTen();
        if (GUILayout.Button("Spawn 30", GUILayout.Width(200), GUILayout.Height(80))) SpawnThirty();
        GUILayout.EndArea();
    }
    
    private void SpawnTen()
    {
        Debug.Log("TNam - Clicked Spawn 10");
    }
    
    private void SpawnThirty()
    {
        Debug.Log("TNam - Clicked Spawn 30");   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
