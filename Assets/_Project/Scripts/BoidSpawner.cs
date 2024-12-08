using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidSpawner : Singleton<BoidSpawner>
{
    [SerializeField] private GameObject _boidPrefab;
    
    public void SpawnBoids(int num)
    {
        StartCoroutine(SpawnBoid(num));
    }

    IEnumerator SpawnBoid(int num)
    {
        for (int i = 0; i < num; i++)
        {
            yield return null;
            Instantiate(_boidPrefab, transform.position, Quaternion.identity);
        }
    }
}
