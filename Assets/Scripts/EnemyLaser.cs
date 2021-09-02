using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaser : MonoBehaviour
{
    [SerializeField] private float _speed = -8.0f;
    void Update()
    {

        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        if(transform.position.y < -5.2f)
        {

            if(transform.parent == true)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }

    }
}
