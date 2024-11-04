using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPick : MonoBehaviour
{
    [SerializeField] private float pickupDistance;
    [SerializeField] private float minPickupDistance;
    [SerializeField] private float maxPickupDistance;
    [SerializeField] private LayerMask pickupLayer;

    private Camera m_camera;
    private GameObject pickedObject;
    private Rigidbody picked_rb;
    private float currentDistance;
    private Vector3 previousPosition;

    void Start()
    {
        m_camera = Camera.main;
        currentDistance = pickupDistance;
    }

    void Update()
    {
        if (pickedObject == null)
        {
            TryPickupObject();
        }
        else
        {
            HoldPickedObject();
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            currentDistance = Mathf.Clamp(currentDistance - scroll, minPickupDistance, maxPickupDistance);
        }

        if (pickedObject != null && Input.GetMouseButtonUp(1))
        {
            DropObject();
        }
    }

    void TryPickupObject()
    {
        Ray ray = new Ray(m_camera.transform.position, m_camera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, pickupDistance, pickupLayer))
        {
            Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.green);

            if (Input.GetMouseButtonDown(1))
            {
                pickedObject = hit.transform.gameObject;
                picked_rb = pickedObject.GetComponent<Rigidbody>();

                if (picked_rb != null)
                {
                    currentDistance = hit.distance;
                    previousPosition = pickedObject.transform.position;
                    StartCoroutine(GetPosition());
                }
            }
        }
        else
        {
            Debug.DrawRay(ray.origin, ray.direction * pickupDistance, Color.red);
        }
    }

    void HoldPickedObject()
    {
        if (picked_rb != null)
        {
            Vector3 targetPosition = m_camera.transform.position + m_camera.transform.forward * currentDistance;
            picked_rb.MovePosition(targetPosition);
        }
    }

    IEnumerator GetPosition()
    {
        previousPosition = pickedObject.transform.position;
        yield return new WaitForSeconds(0.5f);
        if (picked_rb != null)
            StartCoroutine(GetPosition());
        else
            StopCoroutine(GetPosition());
    }

    void DropObject()
    {
        if (picked_rb != null)
        {
            Vector3 releaseVelocity = (pickedObject.transform.position - previousPosition) / Time.deltaTime;
            picked_rb.velocity = releaseVelocity / 50;
            Debug.Log(releaseVelocity);

            picked_rb.isKinematic = false;
            picked_rb.useGravity = true;

            pickedObject = null;
            picked_rb = null;
        }
    }
}