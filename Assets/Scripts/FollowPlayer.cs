using UnityEngine;
public class FollowPlayer : MonoBehaviour
{
    public Transform player;
    public float distance_to_player = 15f;
    public float height_above_player = 5f;
    public float follow_speed = 0.1f;

    public float camaraAngle;

    private Quaternion targetRotation;
    private Vector3 targetPosition;

    void Start()
    {
        transform.rotation = Quaternion.Euler(camaraAngle, 0, 0);
    }

    void LateUpdate()
    {
        // calculate the target position and rotation to follow the player
        Vector3 playerPosition = player.position;
        Vector3 cameraOffset = -player.forward * distance_to_player + Vector3.up * height_above_player;
        targetPosition = playerPosition + cameraOffset;
        targetRotation = Quaternion.Euler(camaraAngle, player.rotation.eulerAngles.y, player.rotation.eulerAngles.z);

        // smoothly interpolate towards the target position and rotation
        transform.position = Vector3.Lerp(transform.position, targetPosition, follow_speed);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, follow_speed);
    }
}