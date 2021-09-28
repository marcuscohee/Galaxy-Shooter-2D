using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private float _speed = 8.0f;
    void Update()
    {

        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        if(transform.position.y > 8f)
        {

            if(transform.parent == true)
            {
                Destroy(transform.parent.gameObject, 2.0f);
            }
            Destroy(this.gameObject);
        }

    }
}
