using System.Runtime.CompilerServices;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpaceshipController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField]
    float speed = 10.0f;

    [SerializeField]
    Vector2 edges;

    [SerializeField]
    bool handleClamp;

    [Header("Rotation")]
    [SerializeField]
    float rotationSpeed;

    [SerializeField]
    float rotationTime;

    [SerializeField]
    bool mouseRotation;

    [Header("Fire")]
    [SerializeField]
    Transform firePoint;

    [SerializeField]
    GameObject bulletPrefab;

    [SerializeField]
    float bulletLifeTime;

    [SerializeField]
    float fireTimeout;

    [Header("Animations")]
    [SerializeField]
    float dieTimeout;

    [SerializeField]
    float dieWaitTime;

    [SerializeField]
    float gameOverWaitTime;

    Vector2 _move = Vector2.zero;
    Vector2 _mousePoint;

    Rigidbody2D _rigidbody;

    float _rotationDirection;
    float _fireTimer;
    bool _isDying = false;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (_isDying) return;

        HandleInputMove();
        HandleInputRotation();
        HandleFire();
    }

    private void FixedUpdate()
    {
        if (_isDying) return;

        HandleRotation();

        if (_move.sqrMagnitude == 0.0f)
        {
            return;
        }

        HandleMove();
        HandleClamp();
        HandleTeleport();
    }

    private void HandleInputMove()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        _move = new Vector2(x, y);
    }

    private void HandleInputRotation()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            _rotationDirection = 1.0f;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            _rotationDirection = -1.0f;
        }
        else
        {
            _rotationDirection = 0.0f;
        }

        _mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void HandleFire()
    {
        _fireTimer -= Time.deltaTime;

        if (Input.GetButtonUp("Fire1"))
        {
            if (_fireTimer > 0.0f)
            {
                return;
            }

            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, transform.rotation);

            Vector2 direction = (firePoint.position - transform.position).normalized;

            BulletController controller = bullet.GetComponent<BulletController>();
            controller.SetDirection(direction);

            Destroy(bullet, bulletLifeTime);
            _fireTimer = fireTimeout;
        }
    }

    private void HandleMove()
    {
        Vector2 direction = _move.normalized;
        Vector2 currentPosition = _rigidbody.position;
        _rigidbody.MovePosition(currentPosition + direction * speed * Time.fixedDeltaTime);
    }

    private void HandleRotation()
    {
        if (mouseRotation)
        {
            Vector2 currentPoint = _rigidbody.position;
            Vector2 direction = (_mousePoint - currentPoint).normalized;
            float angleZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            _rigidbody.MoveRotation(angleZ);
            return;
        }

        float currentRotation = _rigidbody.rotation;
        if (_rotationDirection != 0.0f)
        {
            float targetRotation = currentRotation + _rotationDirection * rotationSpeed * Time.fixedDeltaTime;
            float rotation = Mathf.Lerp(currentRotation, targetRotation, rotationTime);
            _rigidbody.rotation = rotation;
        }
    }

    private void HandleClamp()
    {
        if (!handleClamp)
        {
            return;
        }

        float x = Mathf.Clamp(_rigidbody.position.x, -edges.x, edges.x);
        float y = Mathf.Clamp(_rigidbody.position.y, -edges.y, edges.y);
        _rigidbody.position = new Vector2(x, y);
    }

    private void HandleTeleport()
    {
        if (handleClamp)
        {
            return;
        }

        Vector2 currentPosition = _rigidbody.position;
        if (currentPosition.x > 0.0f && currentPosition.x >= edges.x)
        {
            _rigidbody.position = new Vector2(-edges.x + 0.01f, currentPosition.y);
        }
        else if (currentPosition.x < 0.0f && currentPosition.x <= -edges.x)
        {
            _rigidbody.position = new Vector2(edges.x - 0.01f, currentPosition.y);
        }

        if (currentPosition.y > 0.0f && currentPosition.y >= edges.y)
        {
            _rigidbody.position = new Vector2(currentPosition.x, -edges.y + 0.01f);
        }
        else if (currentPosition.y < 0.0f && currentPosition.y <= -edges.y)
        {
            _rigidbody.position = new Vector2(currentPosition.x, edges.y - 0.01f);
        }
    }

    public void Die()
    {
        Debug.Log("Die method called on SpaceshipController.");
        _isDying = true;
        Collider2D collider = GetComponent<Collider2D>();
        collider.enabled = false;

        UIController.Instance.DecreaseLives();

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

        if (UIController.Instance.HasLives())
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            Debug.Log("Game Over.");
            LevelManager levelManager = FindObjectOfType<LevelManager>();
            levelManager.LastLevel();
            yield return new WaitForSeconds(gameOverWaitTime);
            UIController.Instance.ResetLives();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}