using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Transform m_camera;
    [SerializeField] private float speed;
    private float current_speed;
    private Rigidbody rb;

    float horizontal;
    float vertical;

    [SerializeField] private bool on_ground = false;
    [SerializeField] private float jump_force;
    [SerializeField] private LayerMask groundMask;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        current_speed = speed;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (on_ground)
                rb.AddForce(transform.up * jump_force, ForceMode.Impulse);
        }
    }

    void FixedUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.up, out hit, 1.1f, groundMask))
        {
            on_ground = true;
        }
        else
            on_ground = false;

        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        if (vertical > 0)
            current_speed = speed;
        else
            current_speed = speed * 0.5f;

        if (horizontal != 0)
            current_speed = speed * 0.75f;

        Vector3 mov = new Vector3(horizontal, 0, vertical) * current_speed;
        Vector3 vel = transform.TransformDirection(mov);

        rb.velocity = new Vector3(vel.x, rb.velocity.y, vel.z);
    }
}