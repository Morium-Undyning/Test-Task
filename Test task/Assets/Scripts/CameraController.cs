using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    float speedRotY;
    float speedRotX;
    public float Mouse1 = 200f;

    public GameObject Player;
    void Start()
    {
    //    Cursor.lockState = CursorLockMode.Locked;

    }

    void Update()
    {
        Camera();

    }
    void Camera(){
        speedRotY = Input.GetAxis("Mouse X") * Mouse1 * Time.deltaTime;
        speedRotX = Input.GetAxis("Mouse Y") * Mouse1 * Time.deltaTime;
        Player.transform.Rotate(-speedRotY*new Vector3(0, 1, 0) );
        transform.Rotate(-speedRotX * new Vector3(1, 0, 0));
    }
}
