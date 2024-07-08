using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolController : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float lifeTime;
    [SerializeField] int maxPathPoints;

    Rigidbody2D _rigidbody;
    Transform[] _pathPoints;
    Transform _nextPoint;

    float _aliveTime;
    int _maxPathPoints;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        _rigidbody.position = _pathPoints[0].position;
        _maxPathPoints = Random.Range(maxPathPoints / 2, maxPathPoints);

        Next();
    }

    void FixedUpdate()
    {
        if (_nextPoint == null)
        {
            Next();
        }

        Vector2 currentPosition = _rigidbody.position;
        Vector2 targetPosition = _nextPoint.position;
        Vector2 movePosition = Vector2.MoveTowards(currentPosition, targetPosition, speed * Time.deltaTime);

        _rigidbody.MovePosition(movePosition);

        if (Vector2.Distance(_rigidbody.position, targetPosition) < 0.2F)
        {
            if (_aliveTime <= 0.0F)
            {
                Next();
                _maxPathPoints--;
            }
            else
            {
                _aliveTime -= Time.deltaTime;
            }
        }
    }

    void Next()
    {
        if (_pathPoints == null || _pathPoints.Length == 0)
        {
            Debug.LogError("Path points are not set in PatrolController.");
            return;
        }

        if (_maxPathPoints <= 0)
        {
            Destroy(gameObject);
            return;
        }

        int pointNumber = Random.Range(0, _pathPoints.Length);
        _nextPoint = _pathPoints[pointNumber];

        _aliveTime = lifeTime;
    }

    public void SetPathPoints(Transform[] pathPoints)
    {
        _pathPoints = pathPoints;
        enabled = true;
    }
}