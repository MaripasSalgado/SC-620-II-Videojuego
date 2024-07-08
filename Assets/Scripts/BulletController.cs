using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField]
    float speed;

    [SerializeField]
    int damage;


    Rigidbody2D _rigidbody;

    Vector2 _direction;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        _rigidbody.velocity = _direction * speed;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            CentinelController controller = other.gameObject.GetComponent<CentinelController>();
            if (controller != null)
            {
                float points = controller.GetPoints();
                UIController.Instance.IncreaseScore(points);
                Destroy(other.gameObject);
            }
        }
        Destroy(gameObject);

        if (other.gameObject.CompareTag("Boss"))
        {
            BossController controller = other.gameObject.GetComponent<BossController>();
            if (controller != null)
            {
                controller.TakeDamage(damage);
            }
        }

        Destroy(gameObject);
    }

   
       
   

    public void SetDirection(Vector2 direction)
    {
        _direction = direction;
    }
}