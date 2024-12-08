using System;
using System.Collections;
using System.Collections.Generic;
using _Project.Scripts;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoidManager : Singleton<BoidManager>
{
    [SerializeField] private GameObject _boidPrefab;
    public List<BoidObject> _boidsList;

    [Header("Boid Simulation")] 
    [SerializeField] private bool _useCohesion = true;
    [SerializeField] private bool _useSeparation = true;
    [SerializeField] private bool _useAlignment = true;

    public void SpawnBoids(int num)
    {
        StartCoroutine(SpawnBoid(num));
    }
    
    IEnumerator SpawnBoid(int num)
    {
        for (int i = 0; i < num; i++)
        {
            yield return null;
            GameObject newBoid = Instantiate(_boidPrefab, GetRandomPositionInCircle(3f), Quaternion.identity);
            _boidsList.Add(newBoid.GetComponent<BoidObject>());
        }
    }
    
    public Vector2 GetRandomPositionInCircle(float radius)
    {
        // Generate a random angle
        float angle = Random.Range(0f, Mathf.PI * 2);
        
        // Generate a random radius (square root for uniform distribution)
        float randomRadius = Mathf.Sqrt(Random.Range(0f, 1f)) * radius;
        
        // Convert polar coordinates to Cartesian
        float x = randomRadius * Mathf.Cos(angle);
        float y = randomRadius * Mathf.Sin(angle);
        
        return new Vector2(x, y);
    }


    private void Update()
    {
        foreach (BoidObject boid in _boidsList)
        {
            boid.Move();
            boid.CheckNeighbor();

            Vector2 v1 = Vector2.zero;
            Vector2 v2 = Vector2.zero;
            Vector2 v3 = Vector2.zero;
            
            if (_useCohesion)
                v1 = boid.CohesionCalculate();
            
            if (_useSeparation)
                v2 = boid.SeparationCalculate();
            
            if (_useAlignment)
                v3 = boid.AlignmentCalculate();
            
            boid._direction = (boid._direction + v1 + v2 + v3).normalized;
            boid.HandleRotation();
        }
    }
}