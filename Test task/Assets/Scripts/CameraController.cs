using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform targetC;
    float speedRotY;
    public float Mouse1 = 360f;
    void Start()
    {
      //  Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        Camera();
    }
    void Camera(){
        speedRotY -= Input.GetAxis("Mouse X") * Mouse1 * Time.deltaTime;
        transform.position = targetC.transform.position;
        transform.rotation = Quaternion.Euler(0,speedRotY,0);
    }
}
