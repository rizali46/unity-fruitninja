using UnityEngine;

public class MissZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Check if the thing falling through us is a sliceable cube
        if (other.CompareTag("Sliceable"))
        {
            // Tell the GameManager we missed!
            GameManager.Instance.RegisterMiss();

            // Destroy the cube so it doesn't fall forever
            Destroy(other.gameObject);
        }
    }
}