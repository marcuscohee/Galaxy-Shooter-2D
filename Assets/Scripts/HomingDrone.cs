using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingDrone: MonoBehaviour
{
    [SerializeField] private float _speed = 3;
    [SerializeField] private Transform _target;

    //Thank you GDHQ for the tutorial on lockon missles!
    //It helped a lot! There were lots of bugs I had to work out but I learned a lot!
    void Update()
    {
        if (transform.position.y > 8f) // if the Drone goes above 8y, then clean it up.
        {
            Destroy(this.gameObject);
        }

        if (_target == null)
        {
            bool targetFound = AcquireTarget(); //ask if target is found, if true, skip the if statement.
            if (targetFound == false) // If false, travel up and act like a normal Laser.
            {
                transform.Translate(Vector3.up * _speed * Time.deltaTime);
                
                return;// since there was no target, don't call anything below.
            }
        }
        // This is only called if a target is found.
        Vector3 direction = _target.position - transform.position; // How the Drone can track the Enemy.
        direction.Normalize(); // This makes "direction" = 1. If this is not there and the Enemy is far away, it will travel really fast towards it.
        transform.Translate(direction * _speed * Time.deltaTime); //Travel at _speed towards the Enemy at meter/second.
    }

    bool AcquireTarget()
    {
        GameObject[] allTargets = GameObject.FindGameObjectsWithTag("Enemy"); //Finds targets and adds them to an array.
        if (allTargets.Length > 0)//if there is at least one target, then continue.
        {
            int _chosenTarget = Random.Range(0, allTargets.Length);//variable to choose its target at random.
            _target = allTargets[_chosenTarget].transform; // Since _target is a Transform variable, it finds the chosenTarget's Transform, then homes on it!
            _target.tag = "Targeted_Enemy";//When an Target is chosen, change its tag to Targeted.Enemy so it can't be targeted again.
            return true;// return and say that AcquireTarget = true.
        }
        else //if there are no targets, then return and call AcquireTarget() false.
        {
            _target = null;
            return false;
        }
    }
}
