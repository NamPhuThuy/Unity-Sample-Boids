using System;
using System.Collections;
using System.Collections.Generic;
using _Project.Scripts;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoidManager : Singleton<BoidManager>
{
    [Header("Boid information")]
    [SerializeField] private GameObject _boidPrefab;
    [SerializeField] private float _maxSpeed;
    public List<BoidObject> _boidsList;
    public int boidsToSpawn = 0;

    
    
    [Header("Boid Simulation")] 
    [SerializeField] private bool _useCohesion = true;
    [SerializeField] private bool _useSeparation = true;
    [SerializeField] private bool _useAlignment = true;

    [SerializeField] public int _forceCohesion = 10;
    [SerializeField] public int _forceSeparation = 80;
    [SerializeField] public int _forceAlignment = 10;

    private void Start()
    {
        Debug.Log($"num: {60 / 100f}");
    }

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
            
            if (_useCohesion)
                boid._velocity += boid.CohesionCalculate() * (_forceCohesion/100f);
            
            if (_useSeparation)
                boid._velocity += boid.SeparationCalculate() * (_forceSeparation/100f);
            
            if (_useAlignment)
                boid._velocity += boid.AlignmentCalculate() * (_forceAlignment/100f);
            
            boid._velocity = boid._velocity.normalized;
            boid.HandleRotation();
        }
    }
}