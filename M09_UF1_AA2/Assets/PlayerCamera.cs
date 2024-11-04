using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private float camera_speed;
    [SerializeField] private Transform m_camera;
    private float camera_x;
    private float camera_y;
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Update()
    {
        camera_x += Input.GetAxis("Mouse X") * camera_speed * Time.deltaTime;
        camera_y += Input.GetAxis("Mouse Y") * camera_speed * Time.deltaTime;

        camera_y = Mathf.Clamp(camera_y, -60, 65);

        transform.localRotation = Quaternion.Euler(0, camera_x + transform.localRotation.x, 0);
        m_camera.localRotation = Quaternion.Euler(-camera_y + transform.localRotation.y, 0, 0);
    }
}