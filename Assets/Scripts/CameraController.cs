using UnityEngine;
using UnityEngine.Rendering;

public class CameraController : MonoBehaviour
{
    //room camera
    [SerializeField] private float speed;
    private float CurrPosX;
    private Vector3 velocity = Vector3.zero;

    //follow player
    [SerializeField] private Transform player;
    [SerializeField] private float aheadDistance;
    [SerializeField] private float cameraSpeed;
    private float lookAhead;

    private void Update()
    {
        //room camera
        //transform.position = Vector3.SmoothDamp(transform.position, new Vector3(CurrPosX, transform.position.y, transform.position.z),ref velocity, speed);

        //follow player
        transform.position = new Vector3(player.position.x + lookAhead, transform.position.y, transform.position.z);
        //gradually shifts to the aheaddist from the present lookahead dist in the specified time 
        lookAhead = Mathf.Lerp(lookAhead, (aheadDistance * player.localScale.x), cameraSpeed * Time.deltaTime);
    }

    public void MoveToNewRoom(Transform _newRoom)
    {
        CurrPosX = _newRoom.position.x;
    }
}
