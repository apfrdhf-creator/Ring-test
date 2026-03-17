using UnityEngine;

public class RailFollower : MonoBehaviour
{
    [Header("Rail Setup")]
    [Tooltip("Drag all your invisible waypoint objects in here in order")]
    public Transform[] railNodes;

    [Header("Speed Settings")]
    public float speed = 15f;
    public float rotationSpeed = 10f;

    // This keeps track of which node we are currently heading towards
    private int currentNodeIndex = 0;

    void Update()
    {
        // If we haven't assigned any nodes, or we reached the end, do nothing
        if (railNodes.Length == 0 || currentNodeIndex >= railNodes.Length) return;

        // 1. Find our current target node
        Transform targetNode = railNodes[currentNodeIndex];

        // 2. FORCE MOVEMENT: Move the ring exactly toward the target node
        transform.position = Vector3.MoveTowards(transform.position, targetNode.position, speed * Time.deltaTime);

        // 3. FORCE ROTATION: Make the ring smoothly turn to look at the next node so it curves with the track
        Vector3 direction = targetNode.position - transform.position;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // 4. CHECK DISTANCE: If we are basically touching the target node, switch to the next one!
        if (Vector3.Distance(transform.position, targetNode.position) < 0.2f)
        {
            currentNodeIndex++;
        }
    }
}