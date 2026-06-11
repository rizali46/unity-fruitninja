using UnityEngine;

public class SliceableObject : MonoBehaviour
{
    [Header("Juice Effects")]
    [SerializeField] private GameObject explosionParticlePrefab; // Optional clean effect

    public void OnSliced()
    {
        Debug.Log("Object was sliced");
        GameManager.Instance.AddPoints(1);
        // 1. Instantiate visual satisfaction (Particles) if you have them
        if (explosionParticlePrefab != null)
        {
            Instantiate(explosionParticlePrefab, transform.position, Quaternion.identity);
        }

        // 2. Destroy the target object instantly
        Destroy(gameObject);
    }
}