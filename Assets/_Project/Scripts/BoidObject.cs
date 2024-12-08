

using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace _Project.Scripts
{
    public class BoidObject : MonoBehaviour
    {
        [Header("Simple movement")]
        [SerializeField] public float _speed = 1f;
        [SerializeField] public Vector2 _direction;
        [SerializeField] private GameObject _visual;
        [SerializeField] private Transform _transform;
        
        [Header("Boid Simulation")]
        [SerializeField] private List<GameObject> _cohensionList = new List<GameObject>();
        [SerializeField] private List<GameObject> _separationList = new List<GameObject>();
        [SerializeField] private List<BoidObject> _alignmentList = new List<BoidObject>();
        
        
        private float _cohensionRange = 2.8f;
        private float _separationRange = 0.7f;
        private float _alignmentRange = 1.5f;
        
        void Start()
        {
            _transform = GetComponent<Transform>();
            
            _direction = Helpers.Instance.GenerateRandomVector();
        
            HandleRotation();
            
            StartCoroutine(ChangeDirection());
        }
        
        private void Update()
        {
            
           // Move();
            // CheckNeighbor();
            //
            //
            //
            // Vector2 v1 = CohesionCalculate();
            // Vector2 v2 = SeparationCalculate();
            // Vector2 v3 = AlignmentCalculate();
            //
            // _direction = (_direction + v1 + v2 + v3).normalized;
            //
            // HandleRotation();
        }

        public void Move()
        {
            transform.Translate(_direction * (Time.deltaTime * _speed));
        }
        
        public IEnumerator ChangeDirection()
        {
            while (true)
            {
                yield return new WaitForSeconds(.75f);

                Vector2 tmp = Helpers.Instance.GenerateRandomVector();
                _direction.x = _direction.x * 2f + tmp.x;
                _direction.y = _direction.y * 2f + tmp.y;

                _direction = _direction.normalized;
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
                if (dist <= _cohensionRange)
                {
                    // if (!_neightborsList.Contains(boid)) _neightborsList.Add(boid);
                
                    if (!_cohensionList.Contains(boid.gameObject)) _cohensionList.Add(boid.gameObject);
                }
                else
                {
                    // if (_neightborsList.Contains(boid)) _neightborsList.Remove(boid);
                    if (_cohensionList.Contains(boid.gameObject)) _cohensionList.Remove(boid.gameObject);
                }

                if (dist <= _separationRange)
                {
                    if (!_separationList.Contains(boid.gameObject)) _separationList.Add(boid.gameObject);
                }
                else
                {
                    if (_separationList.Contains(boid.gameObject)) _separationList.Remove(boid.gameObject);
                }

                if (dist <= _alignmentRange)
                {
                    if (!_alignmentList.Contains(boid)) _alignmentList.Add(boid);
                }
                else
                {
                    if (_alignmentList.Contains(boid)) _alignmentList.Remove(boid);
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

                if (dist < _cohensionRange)
                {
                    numCohesion++;
                    sumCohesion += (Vector2) boid._transform.position;
                }

                if (dist < _separationRange)
                {
                    numSeparation++;
                    sumSeparation += (currentPosi - (Vector2) boid._transform.position);
                }

                if (dist < _alignmentRange)
                {
                    numAlighnment++;
                    sumAlignment += boid._direction;
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
            if (_cohensionList.Count <= 0) return res;
        
            Vector2 sum = Vector2.zero;
            sum += (Vector2) transform.position;
            
            foreach (GameObject boid in _cohensionList)
            {
                sum += (Vector2) boid.transform.position;
            }

            Vector2 centerPoint = sum / (_cohensionList.Count + 1);
            res = (centerPoint - (Vector2) transform.position).normalized;
        
            return res;
        }

        public Vector2 SeparationCalculate()
        {
            Vector2 res = Vector2.zero;
            if (_separationList.Count <= 0) return res;
        
            Vector2 sum = new Vector2(0f, 0f);
            foreach (GameObject boid in _separationList)
            {
                sum += (Vector2) (transform.position - boid.transform.position);
            }

            res = sum.normalized;

            return res;
        }

        public Vector2 AlignmentCalculate()
        {
            Vector2 res = Vector2.zero;
            Vector2 sum = new Vector2(0f, 0f);
            
            foreach (BoidObject boid in _alignmentList)
            {
                sum += boid._direction;
            }

            res = sum / _alignmentList.Count;
            return res;
        }

        #endregion
        
        #region Visual

        void OnDrawGizmos()
        {
            // Set the color of the Gizmo line
            Gizmos.color = Color.cyan;

            // Draw the line from startPoint in the specified direction
            Gizmos.DrawLine(transform.position, (Vector2)transform.position + _direction);
        }
        
        /// <summary>
        /// Modify the visual's rotation to match with the boid's direction
        /// </summary>
        public void HandleRotation()
        {
            float tanVal = _direction.y / _direction.x;
            float degree = Mathf.Atan(tanVal) * Mathf.Rad2Deg;
            float degreeBuffer = -90f;
            float degreeBuffer_2 = 90f;
            // transform.localRotation.z = degree;

            if (_direction.x >= 0f)
            {
                Debug.Log("TNam - rotate case 1: -90f");
                _visual.transform.rotation = Quaternion.Euler(new Vector3(_visual.transform.rotation.x, _visual.transform.rotation.y, degree + degreeBuffer));    
            }
            else if (_direction.x < 0f)
            {
                Debug.Log("TNam - rotate case 2: +90f");
                _visual.transform.rotation = Quaternion.Euler(new Vector3(_visual.transform.rotation.x, _visual.transform.rotation.y, degree + degreeBuffer_2));
            }
        
        }

        #endregion
    }
}