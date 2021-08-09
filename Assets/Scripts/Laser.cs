using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private float _speed = 8.0f;
    void Update()
    {

        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        //if laser position is greater than 8 on the y
            //destroy laser
        if(transform.position.y > 8f)
        {
            Destroy(this.gameObject);
        }

    }
}
