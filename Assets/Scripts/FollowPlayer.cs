using UnityEngine;

public class FollowPlayer : MonoBehaviour
{

    public Transform player;

    public float distance_to_player = 20f;

    void Start()
    {
        transform.rotation = Quaternion.Euler(90, 0, 0);
    }

    void Update()
    {
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y + distance_to_player, player.transform.position.z);
    }
}
