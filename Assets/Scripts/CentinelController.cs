using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class CentinelController : MonoBehaviour
{
    [SerializeField]
    float points;
 
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Collision with Player detected.");
 
            SpaceshipController controller = other.gameObject.GetComponent<SpaceshipController>();
            if (controller != null)
            {
                controller.Die();
                Debug.Log("Player should be dying.");
            }
            else
            {
                Debug.LogError("SpaceshipController not found on Player.");
            }
 
            Destroy(gameObject);
        }
    }
 
    public float GetPoints()
    {
        return points;
    }
}
