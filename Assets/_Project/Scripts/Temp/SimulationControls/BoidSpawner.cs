using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidSpawner : MonoBehaviour
{
    public List<SpawnEntry> SpawnEntries;
    public BoidChunkManager ResponsibleManager;
    public float AreaRadius = 10;
    

    /// <summary>
    /// 
    /// </summary>
    void Start()
    {
        foreach(var entry in SpawnEntries)
            entry.Count.Times(() => Spawn(entry.Prefab));
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="prefab"></param>
    public void Spawn(Boids prefab)
    {
        var startPosition = Random.insideUnitSphere * AreaRadius + transform.position;
        Boids boids = Instantiate(
            prefab,
            startPosition,
            Random.rotation,
            ResponsibleManager.transform
        );
        ResponsibleManager.Add(boids);
    }


    /// <summary>
    /// 
    /// </summary>
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, AreaRadius);
    }


    /// <summary>
    /// 
    /// </summary>
    [System.Serializable]
    public struct SpawnEntry
    {
        public Boids Prefab;
        public int Count;
    }
}