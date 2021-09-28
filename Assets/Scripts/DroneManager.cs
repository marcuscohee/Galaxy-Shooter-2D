using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _drones;
    [SerializeField] private GameObject _homingDroneGameObject;
    [SerializeField] private float _fireRate = 0.15f;
    [SerializeField] private float _canFire = -1f;

    void Update()
    {
        _drones = GameObject.FindGameObjectsWithTag("Drone");
        if (Input.GetKeyDown(KeyCode.E) && _drones.Length >= 1 && Time.time > _canFire)
        {
            _canFire = Time.time + _fireRate;
            _drones[0].GetComponent<HomingDrone>().ReleaseDrone();
        }
    }

    public void DronePickup()
    {
        //instantiate drones into the bays when powerup is picked up.
        if (transform.childCount == 0)
        {
            Instantiate(_homingDroneGameObject, transform.position, Quaternion.identity, transform.parent = gameObject.transform);
        }
    }
}
