using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public float speed;

    public LayerMask raycastLayerMask;
    public Material colMat;

    private float targetSpeed;
    private float speedLimit;
    private float prevTargetSpeed;
    private bool appliedSpeedLimit = false;

    private int id;

    private float speedShift = 5f;

    private Vector3 targetPosition;

    private float distanceDelay = 0.2f;
    private float nextCheck;
    private float lastDistance;

    // Start is called before the first frame update
    void Start()
    {
        speed = Mathf.Round(Random.Range(80f, 90f));
        speedLimit = speed;
        targetSpeed = speed;
        nextCheck = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        GetComponentInChildren<TextMeshPro>().text =
            $"{id}\n" +
            $"Speed: {speed}\n" +
            $"Target Speed: {targetSpeed}\n" +
            $"Distance to Next: {CheckDistanceInfront()}"; 

        if (Time.time >= nextCheck || appliedSpeedLimit)
        {
            float distance = CheckDistanceInfront();

            if (distance > 30f && distance <= 60f)
            {
                if(distance < lastDistance)
                {
                    if (appliedSpeedLimit)
                    {
                        targetSpeed = prevTargetSpeed;
                        appliedSpeedLimit = false;
                    }
                    targetSpeed -= 20f;
                    if (targetSpeed <= 0f) targetSpeed = 0f;
                    Debug.Log($"CAR {id} SLOWING DOWN to {targetSpeed}");
                }
                lastDistance = distance;
            } else if(distance > 0f && distance <= 30f)
            {
                if (distance < lastDistance)
                {
                    if (appliedSpeedLimit)
                    {
                        targetSpeed = prevTargetSpeed;
                        appliedSpeedLimit = false;
                    }
                    targetSpeed -= 35f;
                    if (targetSpeed <= 0f) targetSpeed = 0f;
                    Debug.Log($"CAR {id} RAPIDLY BREAKING to {targetSpeed}");
                }
                lastDistance = distance;
            } else
            {
                targetSpeed += 20f;
                if (targetSpeed > speedLimit) targetSpeed = speedLimit;
                Debug.Log($"CAR {id} SPEEDING UP to {targetSpeed}");
            }

            nextCheck = Time.time + distanceDelay;
        }

        if(speed != targetSpeed)
        {
            speed = Mathf.Lerp(speed, targetSpeed, speedShift * Time.deltaTime);
        }
    
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        if(transform.position == targetPosition) {
            HighwayController.Instance.RemoveCar(id);
            Destroy(gameObject); 
        }
        appliedSpeedLimit = false;
    }

    public void Initialize(int carid, Transform endPos)
    {
        id = carid;
        SetTargetDestination(endPos);
    }

    private float CheckDistanceInfront()
    {
        RaycastHit hit;
        Vector3 castPosition = new Vector3(transform.position.x+20,transform.position.y+8,transform.position.z);
        if (Physics.Raycast(castPosition, transform.forward, out hit, 100f, raycastLayerMask))
        {
            Debug.DrawLine(castPosition, hit.point, Color.red);
            if (hit.collider.CompareTag("Car"))
            {
                return hit.distance;
            }
        }
        return 0f;
    }

    public void SetSpeedLimit(float tspeed)
    {
        speedLimit = tspeed;
        prevTargetSpeed = targetSpeed;
        targetSpeed = speedLimit;
        appliedSpeedLimit = true;
    }

    public void SetTargetDestination(Transform tf)
    {
        targetPosition = tf.position;
    }

    public void MergeLane(string direction)
    {
        switch(direction)
        {
            case "left":
                break;
            case "right":
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Car")
        {
            foreach(MeshRenderer ren in GetComponentsInChildren<MeshRenderer>()) {
                if (ren.GetComponent<TextMeshPro>()) return;
                ren.material = colMat;
            }
        }
    }
}
