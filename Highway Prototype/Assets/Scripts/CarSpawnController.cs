using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawnController : MonoBehaviour
{
    [SerializeField] GameObject carObject;
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] Transform[] endPoints;

    [SerializeField] float spawnDelay = 2f;
    private float spawnTime;

    private int carCount = 1;

    // Start is called before the first frame update
    void Start()
    {
        spawnTime = Time.time + spawnDelay;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time > spawnTime)
        {
            InstantiateCar();
            spawnTime = Time.time + spawnDelay;
        }
    }

    private void InstantiateCar()
    {
        int lane = Random.Range(0, spawnPoints.Length);

        GameObject car = Instantiate(carObject, spawnPoints[lane].position, Quaternion.identity);

        car.GetComponent<CarController>().Initialize(carCount, endPoints[lane]);
        car.transform.LookAt(endPoints[lane].position);

        HighwayController.Instance.AddCarToLane(lane + 1, car);
        HighwayController.Instance.AddCar(carCount + 1, car);

        carCount++;
    }
}
