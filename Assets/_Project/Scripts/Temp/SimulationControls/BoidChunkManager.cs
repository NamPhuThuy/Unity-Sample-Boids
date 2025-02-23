using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BoidChunkManager : MonoBehaviour
{
    [Header("Chunk manager settings...")]
    public float ChunkSize;

    [Header("Boid count control...")]
    public BoidSpawner Spawner;
    [FormerlySerializedAs("NewBoidPrefab")] public Boids newBoidsPrefab;
    public int DesiredFPS;
    public int TimeSamples;

    [HideInInspector]
    public int FPS;
    [HideInInspector]
    public int BoidCount;

    private IChunkManager<Boids, AggregatedBoidChunk> ChunkManager;
    private Queue<float> DeltaTimeSamples;
    private float LastCountChangeDelta;


    /// <summary>
    /// 
    /// </summary>
    void Start()
    {
        ChunkManager = new ChunkManager<Boids, AggregatedBoidChunk>(ChunkSize);
        DeltaTimeSamples = new Queue<float>();
    }

    /// <summary>
    /// 
    /// </summary>
    void Update()
    {
        EvaluateInfos();
        ChangeBoidCount();
        ChunkManager.UpdateChunks();
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="boids"></param>
    public void Add(Boids boids)
    {
        ChunkManager.Add(boids);
    }


    /// <summary>
    /// 
    /// </summary>
    private void ChangeBoidCount()
    {
        var fpsDifference = Mathf.Max(1, Mathf.Abs(FPS - DesiredFPS));
        var changeRate = 1 / Mathf.Pow(fpsDifference, 2);

        LastCountChangeDelta += Time.deltaTime;
        if (LastCountChangeDelta > changeRate)
        {
            if (FPS > DesiredFPS)
                Spawner.Spawn(newBoidsPrefab);
            else if (FPS < DesiredFPS)
            {
                var boid = ChunkManager.Entities[ChunkManager.Entities.Count - 1];
                ChunkManager.Remove(boid);
                Destroy(boid.gameObject);
            }

            LastCountChangeDelta = 0;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private void EvaluateInfos()
    {
        //sample delta time
        DeltaTimeSamples.Enqueue(Time.deltaTime);
        if (DeltaTimeSamples.Count >= TimeSamples)
            DeltaTimeSamples.Dequeue();

        //calculate FPS
        var averageTime = 0f;
        foreach (var time in DeltaTimeSamples)
            averageTime += time;
        averageTime /= DeltaTimeSamples.Count;

        FPS = (int)(1f / averageTime);

        //provide boid count
        BoidCount = ChunkManager.Entities.Count;
    }
}