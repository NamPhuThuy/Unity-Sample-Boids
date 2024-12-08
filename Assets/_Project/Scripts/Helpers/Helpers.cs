using UnityEngine;

public class Helpers : Singleton<Helpers>
{
    public Vector2 GenerateRandomVector()
    {
        float x = Random.Range(-1f, 1f);
        float y = Random.Range(-1f, 1f);
        return new Vector2(x, y);
    }
}