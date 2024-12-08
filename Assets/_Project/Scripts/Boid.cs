using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class Boid : MonoBehaviour
{
    [SerializeField] private float _speed = 1f;
    [SerializeField] private Vector2 _direction;
    [SerializeField] private GameObject _visual;
    
    void Start()
    {
        print(Mathf.Atan(1f));
        print(Mathf.Rad2Deg * Mathf.Atan(1f));
        _direction = GenerateRandomVector();
        HandleRotation();
        
        StartCoroutine(ChangeSpeed());
    }

    public IEnumerator ChangeSpeed()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);

            Vector2 tmp = GenerateRandomVector();
            _direction.x = _direction.x * 2f + tmp.x;
            _direction.y = _direction.y * 2f + tmp.y;

            _direction = _direction.normalized;
            HandleRotation();
        }
    }

    private Vector2 GenerateRandomVector()
    {
        float x = Random.Range(-1f, 1f);
        float y = Random.Range(-1f, 1f);
        return new Vector2(x, y);
    }

    private void Update()
    {
        transform.Translate(_direction * (Time.deltaTime * _speed));
        
    }

    private void HandleRotation()
    {
        float tanVal = _direction.y / _direction.x;
        float degree = Mathf.Atan(tanVal) * Mathf.Rad2Deg;
        float degreeBuffer = -90f;
        // transform.localRotation.z = degree;
        _visual.transform.rotation = Quaternion.Euler(new Vector3(_visual.transform.rotation.x, _visual.transform.rotation.y, degree + degreeBuffer));
    }

    #region Visualize

    void OnDrawGizmos()
    {
        // Set the color of the Gizmo line
        Gizmos.color = Color.cyan;

        // Draw the line from startPoint in the specified direction
        Gizmos.DrawLine(transform.position, (Vector2)transform.position + _direction);
    }

    #endregion
}
