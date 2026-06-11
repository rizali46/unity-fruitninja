using System.Collections;
using UnityEngine;

public class FruitSpawner : MonoBehaviour
{
    [Header("Spawning Settings")]
    public GameObject cubePrefab; // Drag your Cube Prefab here
    public float spawnInterval = 2f; // Time between spawns

    [Header("Physics Settings")]
    public float minForce = 12f;
    public float maxForce = 16f;
    public float minTorque = -10f; // For spinning
    public float maxTorque = 10f;

    [Header("Spawn Area")]
    public float spawnXRange = 5f; // How wide the spawn area is
    public float spawnYPosition = -6f; // Should be below the camera view

    void Start()
    {
        // Start the infinite spawning loop
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true) // Keep spawning forever
        {
            yield return new WaitForSeconds(spawnInterval);

            // Spawn 3 cubes at the same time
            for (int i = 0; i < 3; i++)
            {
                SpawnCube();
            }
        }
    }

    private void SpawnCube()
    {
        // 1. Pick a random X position at the bottom of the screen
        Vector3 spawnPosition = new Vector3(Random.Range(-spawnXRange, spawnXRange), spawnYPosition, 0);

        // 2. Create the cube
        GameObject spawnedCube = Instantiate(cubePrefab, spawnPosition, Random.rotation);

        // 3. Throw it upwards!
        Rigidbody rb = spawnedCube.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Apply upward force with a slight random tilt so they arc
            Vector3 throwDirection = new Vector3(Random.Range(-0.5f, 0.5f), 1f, 0f).normalized;
            float randomForce = Random.Range(minForce, maxForce);

            rb.AddForce(throwDirection * randomForce, ForceMode.Impulse);

            // Add random spinning (Torque) to make it look dynamic
            rb.AddTorque(Random.Range(minTorque, maxTorque), Random.Range(minTorque, maxTorque), Random.Range(minTorque, maxTorque), ForceMode.Impulse);
        }

        // 4. Cleanup: Destroy the cube after 5 seconds so it doesn't fall infinitely and crash your game
        Destroy(spawnedCube, 5f);
    }
}