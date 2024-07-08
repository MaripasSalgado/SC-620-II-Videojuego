using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletControllerEnemy : MonoBehaviour
{
    [SerializeField]
    float speed;

    Rigidbody2D _rigidbody;
    Vector2 _direction;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _rigidbody.velocity = _direction * speed * Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SpaceshipController controller = collision.gameObject.GetComponent<SpaceshipController>();
            if (controller != null)
            {
                controller.Die();
            }
        }

        Destroy(gameObject);
    }

    public void SetDirection(Vector2 direction)
    {
        _direction = direction.normalized;
    }

    private void OnEnable()
    {
        GameObject boss = GameObject.FindGameObjectWithTag("Boss");
        if (boss != null)
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), boss.GetComponent<Collider2D>());
        }
    }
}