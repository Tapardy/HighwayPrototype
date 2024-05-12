using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighwayController : MonoBehaviour
{
    public Dictionary<int, List<GameObject>> lanes = new Dictionary<int, List<GameObject>>();
    public Dictionary<int, bool> laneStatuses = new Dictionary<int, bool>();
    public Dictionary<int, GameObject> cars = new Dictionary<int, GameObject>();

    [SerializeField] float laneCount;

    public static HighwayController Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        } else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i<laneCount; i++)
        {
            lanes.Add(i+1, new List<GameObject>());
            laneStatuses.Add(i+1,true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddCar(int id, GameObject car) 
    {
        cars.Add(id, car);
    }

    public void RemoveCar(int id)
    {
        cars.Remove(id);
    }

    public void AddCarToLane(int laneid, GameObject Car)
    {
        lanes[laneid].Add(Car);
    }

    public void RemoveCarFromLane(int laneid, GameObject Car)
    {
        lanes[laneid].Remove(Car);
    }

    public void CloseLane(int id)
    {
        laneStatuses[id] = false;
        foreach(GameObject Car in lanes[id])
        {
            // Force Cars to different lane
        }
    }

    public void OpenLane(int id)
    {
        laneStatuses[id] = true;
    }
}
