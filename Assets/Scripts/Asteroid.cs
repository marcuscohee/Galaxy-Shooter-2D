using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private float _rotateSpeed = 3.0f;
    [SerializeField] private Animator _onAsteroidDeath;
    void Start()
    {
        _onAsteroidDeath = GameObject.Find("Explosion").GetComponent<Animator>();
        if(_onAsteroidDeath == null)
        {
            Debug.LogError("The Explosion Animation is NULL");
        }
    }


    void Update()
    {
        transform.Rotate(new Vector3(0, 0, 3) * _rotateSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            _onAsteroidDeath.SetTrigger("OnAsteroidDeath");
            StartCoroutine(AsteroidExplosionRoutine());
        }

        IEnumerator AsteroidExplosionRoutine()
        {
            yield return new WaitForSeconds(0.2f);
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            gameObject.GetComponent<CircleCollider2D>().enabled = false;
            yield return new WaitForSeconds(1.8f);
            Destroy(this.gameObject);
        }
    }
}
