using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CarPlacementIndicator : MonoBehaviour
{
    Vector3 targetPosition;
    private GameObject ghostObject;
    public GameObject ghostObjectPrefab;

    private bool isPlacementEnabled = false;
    private bool locationReached = true;
    public string roadTag = "Road";
    public float moveSpeed = 5f;
    public float rotationSpeed = 5f;
    public float stopDistance = 0.1f;

    public Button toggleButton;
    public TMP_Text buttonText;

    void Start()
    {
        targetPosition = transform.position;
        toggleButton.onClick.AddListener(TogglePlacement);
    }

    void Update()
    {
        if (isPlacementEnabled)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                if (Physics.Raycast(ray, out hit))
                {
                    locationReached = false;
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

            RotateGhostObject();
        }

        MoveAndRotateCar();
    }

    void RotateGhostObject()
    {
        if (ghostObject != null)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                ghostObject.transform.Rotate(Vector3.up * 5f, Space.Self);
            }
            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                ghostObject.transform.Rotate(Vector3.down * 5f, Space.Self);
            }
        }
    }

    void MoveAndRotateCar()
    {
        if (locationReached) return;

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
        else
        {
            locationReached = true;
        }
    }

    void TogglePlacement()
    {
        isPlacementEnabled = !isPlacementEnabled;

        if (isPlacementEnabled)
        {
            buttonText.text = "Exit placement mode";
            ghostObject.SetActive(true);
        }
        else
        {
            buttonText.text = "Enter placement mode";
            ghostObject.SetActive(false);
        }
    }
}
