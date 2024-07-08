using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [SerializeField]
    GameObject bulletPrefabEnemy;

    [SerializeField]
    int maxHealth;

    [SerializeField]
    Transform firePoint;

    [SerializeField]
    float points;

    [SerializeField]
    float fireRate;

    [Header("Animations")]
    [SerializeField]
    float dieTimeout;

    int currentHealth;
    bool _isDying = false;

    private Transform playerTransform;

    private void Start()
    {
        currentHealth = maxHealth;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(FireRoutine());
    }

    public float GetPoints()
    {
        return points;
    }

    private IEnumerator FireRoutine()
    {
        while (true)
        {
            Vector2 direction = (playerTransform.position - firePoint.position).normalized;
            FireBullet(direction);
            yield return new WaitForSeconds(1 / fireRate);
        }
    }

    private void FireBullet(Vector2 direction)
    {
        GameObject bullet = Instantiate(bulletPrefabEnemy, firePoint.position, Quaternion.identity);
        BulletControllerEnemy controller = bullet.GetComponent<BulletControllerEnemy>();
        if (controller != null)
        {
            controller.SetDirection(direction);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Boss took damage. Current health: " + currentHealth);

        if (currentHealth <= 0)
        {
            float points = GetPoints();
            UIController.Instance.IncreaseScore(points);
            Die();
  
        }
    }

    public void Die()
    {
        Debug.Log("Die method called on BossController.");
        _isDying = true;
        StartCoroutine(DieCoroutine());
    }

    private IEnumerator DieCoroutine()
    {
        SpriteRenderer renderer = GetComponentInChildren<SpriteRenderer>();
        Color color = renderer.color;

        while (color.a > 0.0f)
        {
            color.a -= 0.1f;
            renderer.color = color;
            yield return new WaitForSeconds(dieTimeout);
           
        }
        Destroy(gameObject);
    }
}