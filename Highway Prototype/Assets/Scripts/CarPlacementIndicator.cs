using UnityEngine;

public class CarPlacementIndicator : MonoBehaviour
{
    Vector3 targetPosition;
    private GameObject ghostObject;
    public GameObject ghostObjectPrefab;

    public string roadTag = "Road";
    public float moveSpeed = 5f;
    public float rotationSpeed = 5f;
    public float stopDistance = 0.1f;

    void Start()
    {
        targetPosition = transform.position;
    }

    void Update()
    {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            
            if (Input.GetMouseButtonDown(0))
            {
                if (Physics.Raycast(ray, out hit))
                {
                    targetPosition = hit.point;
                }
            }

            if (Physics.Raycast(ray, out hit) && hit.collider.CompareTag(roadTag))
            {
                if (ghostObject == null)
                {
                    ghostObject = Instantiate(ghostObjectPrefab, hit.point, Quaternion.identity);
                }
                else
                {
                    ghostObject.transform.position = hit.point;
                }
            }

            MoveAndRotateCar();
    }
    

    void MoveAndRotateCar()
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, targetPosition);

        if (distance > stopDistance)
        {
            float dotProduct = Vector3.Dot(transform.forward, direction);
            bool shouldMoveBackward = dotProduct < 0;

            Quaternion targetRotation = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            if (shouldMoveBackward)
            {
                transform.position -= transform.forward * moveSpeed * Time.deltaTime;
            }
            else
            {
                transform.position += transform.forward * moveSpeed * Time.deltaTime;
            }
        }
    }
}