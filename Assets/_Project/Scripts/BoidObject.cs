

using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Serialization;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace _Project.Scripts
{
    public class BoidObject : MonoBehaviour
    {
        [Header("Simple movement")]
        [SerializeField] public float _speed = 1f;
        [SerializeField] public Vector2 _velocity;
        [SerializeField] private GameObject _visual;
        [SerializeField] private Transform _transform;
        
        [FormerlySerializedAs("_cohensionList")]
        [Header("Boid Simulation")]
        [SerializeField] private List<BoidObject> _neighborList = new List<BoidObject>();
        [SerializeField] private List<GameObject> _separationList = new List<GameObject>();
        
        
        private float _neighborRange = 2.8f;
        private float _separationRange = 0.7f;
        
        void Start()
        {
            _transform = GetComponent<Transform>();
            
            _velocity = Helpers.Instance.GenerateRandomVector();
        
            HandleRotation();
            
            StartCoroutine(ChangeDirection());
        }

        public void Move()
        {
            transform.Translate(_velocity * (Time.deltaTime * _speed));
        }
        
        public IEnumerator ChangeDirection()
        {
            while (true)
            {
                yield return new WaitForSeconds(.75f);

                Vector2 tmp = Helpers.Instance.GenerateRandomVector();
                _velocity.x = _velocity.x * 2f + tmp.x;
                _velocity.y = _velocity.y * 2f + tmp.y;

                _velocity = _velocity.normalized;
                HandleRotation();
            }
        }
        
        

        #region Boid Simulates
        public void CheckNeighbor()
        {
            foreach (BoidObject boid in BoidManager.Instance._boidsList)
            {
                if (boid.gameObject == gameObject) return;

                float dist = Vector2.Distance(transform.position, boid.transform.position); 
                if (dist <= _neighborRange)
                {
                    // if (!_neightborsList.Contains(boid)) _neightborsList.Add(boid);
                
                    if (!_neighborList.Contains(boid)) _neighborList.Add(boid);
                }
                else
                {
                    // if (_neightborsList.Contains(boid)) _neightborsList.Remove(boid);
                    if (_neighborList.Contains(boid)) _neighborList.Remove(boid);
                }

                if (dist <= _separationRange)
                {
                    if (!_separationList.Contains(boid.gameObject)) _separationList.Add(boid.gameObject);
                }
                else
                {
                    if (_separationList.Contains(boid.gameObject)) _separationList.Remove(boid.gameObject);
                }
            }
        }

        public Vector2 ProcessBoidCalculate()
        {
            Vector2 res = Vector2.zero;
            
            Vector2 currentPosi = transform.position;
            
            //Result of each rule
            Vector2 cohesionRes = Vector2.zero;
            Vector2 separationRes = Vector2.zero;
            Vector2 alignmentRes = Vector2.zero;
            
            // Temp sum of each rule
            Vector2 sumCohesion = Vector2.zero;
            Vector2 sumSeparation = Vector2.zero;
            Vector2 sumAlignment = Vector2.zero;
            
            // Number of neighbors of each rule
            int numCohesion = 0;
            int numSeparation = 0;
            int numAlighnment = 0;
    
            foreach (BoidObject boid in BoidManager.Instance._boidsList)
            {
                if (boid.gameObject == gameObject) continue;
                float dist = Vector2.Distance(currentPosi, boid._transform.position);

                if (dist < _neighborRange)
                {
                    numCohesion++;
                    sumCohesion += (Vector2) boid._transform.position;
                    
                    
                    numAlighnment++;
                    sumAlignment += boid._velocity;
                }

                if (dist < _separationRange)
                {
                    numSeparation++;
                    sumSeparation += (currentPosi - (Vector2) boid._transform.position);
                }
            }

            cohesionRes = (sumCohesion / numCohesion) - currentPosi;
            separationRes = sumSeparation.normalized;
            alignmentRes = sumAlignment / numAlighnment;

            res = (cohesionRes + separationRes + alignmentRes).normalized;

            return res;
        }
        
        public Vector2 CohesionCalculate()
        {
            Vector2 res = Vector2.zero;
            if (_neighborList.Count <= 0) return res;
        
            Vector2 sum = Vector2.zero;
            // sum += (Vector2) transform.position;
            
            foreach (BoidObject boid in _neighborList)
            {
                sum += (Vector2) boid._transform.position;
            }
            
            // sum / _cohensionList.Count: the center point of neighbor boids
            res = (sum / _neighborList.Count) - (Vector2) transform.position;
        
            return res;
        }

        public Vector2 SeparationCalculate()
        {
            Vector2 res = Vector2.zero;
            if (_separationList.Count <= 0) return res;
            
            foreach (GameObject boid in _separationList)
            {
                res += (Vector2) (transform.position - boid.transform.position).normalized;
            }

            return res;
        }

        public Vector2 AlignmentCalculate()
        {
            Vector2 res = Vector2.zero;
            if (_neighborList.Count <= 0) return res;
            
            foreach (BoidObject boid in _neighborList)
            {
                res += boid._velocity;
            }

            res /= _neighborList.Count;
            return res;
        }

        #endregion
        
        #region Visual

        void OnDrawGizmos()
        {
            // Set the color of the Gizmo line
            Gizmos.color = Color.cyan;

            // Draw the line from startPoint in the specified direction
            Gizmos.DrawLine(transform.position, (Vector2)transform.position + _velocity);
        }
        
        /// <summary>
        /// Modify the visual's rotation to match with the boid's direction
        /// </summary>
        public void HandleRotation()
        {
            float tanVal = _velocity.y / _velocity.x;
            float degree = Mathf.Atan(tanVal) * Mathf.Rad2Deg;
            float degreeBuffer = -90f;
            float degreeBuffer_2 = 90f;
            // transform.localRotation.z = degree;

            if (_velocity.x >= 0f)
            {
                _visual.transform.rotation = Quaternion.Euler(new Vector3(_visual.transform.rotation.x, _visual.transform.rotation.y, degree + degreeBuffer));    
            }
            else if (_velocity.x < 0f)
            {
                _visual.transform.rotation = Quaternion.Euler(new Vector3(_visual.transform.rotation.x, _visual.transform.rotation.y, degree + degreeBuffer_2));
            }
        
        }

        #endregion
    }
}