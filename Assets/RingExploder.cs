using System.Collections;
using UnityEngine;

public class RingExploder : MonoBehaviour
{
    [Header("Explosion Effects")]
    [Tooltip("The explosion for the ring")]
    public GameObject ringExplosionParticles;
    [Tooltip("The explosion for the wall")]
    public GameObject wallExplosionParticles;

    [Header("Timing")]
    [Tooltip("Seconds to wait before the wall blows up")]
    public float delayBetweenExplosions = 1.0f;

    // This runs the exact frame the ring's trigger overlaps with the wall
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            // Start the timeline sequence, passing in the wall we just hit
            StartCoroutine(DoubleExplosionSequence(other.gameObject));
        }
    }

    // IEnumerator is a special Unity function that can pause time using "yield"
    IEnumerator DoubleExplosionSequence(GameObject wallObject)
    {
        // 1. THE RING EXPLODES
        if (ringExplosionParticles != null)
        {
            Instantiate(ringExplosionParticles, transform.position, transform.rotation);
        }

        // 2. FAKE THE RING'S DEATH
        // Turn off the trigger so it doesn't hit anything else
        GetComponent<Collider>().enabled = false;

        // Find the 3D visual model inside the ring and turn it invisible
        foreach (MeshRenderer mesh in GetComponentsInChildren<MeshRenderer>())
        {
            mesh.enabled = false;
        }

        // 3. WAIT FOR IT...
        // This pauses the script (but not the game) for the delay you set
        yield return new WaitForSeconds(delayBetweenExplosions);

        // 4. THE WALL EXPLODES
        // Make sure the wall still exists before blowing it up
        if (wallExplosionParticles != null && wallObject != null)
        {
            Instantiate(wallExplosionParticles, wallObject.transform.position, wallObject.transform.rotation);

            // Destroy the wall entirely
            Destroy(wallObject);
        }

        // 5. ACTUAL DEATH
        // Now that the job is done, destroy the invisible ring object
        Destroy(gameObject);
    }
}