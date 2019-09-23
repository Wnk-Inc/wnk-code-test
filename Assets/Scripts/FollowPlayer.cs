using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player;
    public new Transform camera;

    void Update()
    {
        camera.transform.position = new Vector3(player.position.x, player.position.y, -500);
    }
}