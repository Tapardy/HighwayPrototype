using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MatrixSignController : MonoBehaviour
{
    private TextMeshPro signText;
    [SerializeField] float defaultSpeed;

    public bool status = true;

    private bool laneStatus;
    private float laneSpeed;

    // Start is called before the first frame update
    void Start()
    {
        signText = GetComponentInChildren<TextMeshPro>();
        laneSpeed = defaultSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        SetLaneStatus(status);

        if(laneStatus)
        {
            signText.text = laneSpeed.ToString();
        } else
        {
            signText.text = "X";
        }
        
    }

    public void SetLaneSpeed(float speed)
    {
        laneSpeed = speed;
    }

    public void SetLaneStatus(bool lane)
    {
        if(lane)
        {
            signText.color = Color.white;
            laneStatus = true;
        } else
        {
            signText.color = Color.red;
            laneStatus = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Car")
        {
            other.gameObject.GetComponent<CarController>().SetSpeedLimit(laneSpeed);
        }
    }
}
