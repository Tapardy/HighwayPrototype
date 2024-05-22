using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CarPlacementIndicator : MonoBehaviour
{
    private Vector3 targetPosition;
    private GameObject ghostObject;
    private Quaternion ghostRotationSnapshot;
    private bool isPlacementEnabled = false;
    private bool locationReached = true;

    [SerializeField] private Material colMat;
    [SerializeField] private GameObject ghostObjectPrefab;
    [SerializeField] private string roadTag = "Road";
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float stopDistance = 1f;
    [SerializeField] private Button toggleButton;
    [SerializeField] private TMP_Text buttonText;

    void Start()
    {
        targetPosition = transform.position;
        toggleButton.onClick.AddListener(TogglePlacement);

        AdjustGhostOpacity(ghostObjectPrefab);
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

                    if (ghostObject != null)
                    {
                        ghostRotationSnapshot = ghostObject.transform.rotation;
                    }
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

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                TogglePlacement();
            }

            RotateGhostObject();
        }

        MoveAndRotateCar();
    }

    private void RotateGhostObject()
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

    private void MoveAndRotateCar()
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
            transform.rotation = Quaternion.RotateTowards(transform.rotation, ghostRotationSnapshot,
                (rotationSpeed * 5) * Time.deltaTime);

            if (Quaternion.Angle(transform.rotation, ghostRotationSnapshot) < 0.1f)
            {
                locationReached = true;
            }
        }
    }

    private void TogglePlacement()
    {
        isPlacementEnabled = !isPlacementEnabled;

        if (isPlacementEnabled)
        {
            buttonText.text = "Exit placement mode";
            if (ghostObject != null)
            {
                ghostObject.SetActive(true);
            }
        }
        else
        {
            buttonText.text = "Enter placement mode";
            if (ghostObject != null)
            {
                ghostObject.SetActive(false);
            }
        }
    }

    private void AdjustGhostOpacity(GameObject ghostObject)
    {
        if (ghostObject != null)
        {
            MeshRenderer[] renderers = ghostObject.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer ren in renderers)
            {
                if (ren.GetComponent<TextMeshPro>()) return;
                ren.material = colMat;
            }
        }
    }
}