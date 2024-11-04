using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Transform m_camera;
    [SerializeField] private float speed;
    [SerializeField] private float current_speed;
    private Rigidbody rb;

    float horizontal;
    float vertical;

    [SerializeField] private bool on_ground = false;
    [SerializeField] private float jump_force;
    [SerializeField] private LayerMask groundMask;

    private bool sprint = false;
    private bool jet = true;

    float timer = 0;

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
        if (Input.GetKeyDown(KeyCode.LeftShift))
            sprint = true;
        else if (sprint)
            sprint = false;

        if (!on_ground && Input.GetKey(KeyCode.Space) && timer < 1 && jet)
        {
            rb.AddForce(transform.up * (jump_force / 2), ForceMode.Acceleration);
        }

        if (on_ground && jet)
        {
            StartCoroutine(JetRecoil());
        }

        timer += Time.deltaTime;
    }

    IEnumerator JetRecoil()
    {
        jet = false;
        yield return new WaitForSeconds(0.5f);
        jet = true;
        timer = 0;
    }

    void FixedUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.up, out hit, 1.1f, groundMask))
        {
            on_ground = true;
            current_speed = speed;
        }
        else
            on_ground = false;

        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        if (horizontal != 0 && on_ground && !sprint)
            current_speed = speed * 0.75f;

        if (vertical > 0 && on_ground)
        {
            current_speed = speed;
            if (sprint)
                current_speed = speed * 2;
            else
                current_speed = speed;
        } 
        else
            current_speed = speed * 0.5f;


        Vector3 mov = new Vector3(horizontal, 0, vertical) * current_speed;
        Vector3 vel = transform.TransformDirection(mov);

        rb.velocity = new Vector3(vel.x, rb.velocity.y, vel.z);
    }
}