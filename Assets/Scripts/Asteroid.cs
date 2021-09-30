using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private float _rotateSpeed = 3.0f;
    [SerializeField] private Animator _onAsteroidDeath;
    [SerializeField] private SpawnManager _spawnManager;
    [SerializeField] private AudioSource _onAsteroidDeathAudioClip;
    void Start()
    {
        _onAsteroidDeath = GameObject.Find("Asteroid_Explosion").GetComponent<Animator>();
        if(_onAsteroidDeath == null)
        {
            Debug.LogError("The Explosion Animation is NULL");
        }
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn_Manager is NULL");
        }
        _onAsteroidDeathAudioClip = GetComponent<AudioSource>();
        if (_onAsteroidDeathAudioClip == null)
        {
            Debug.LogError("The AudioSource from Asteroid is NULL");
        }
    }


    void Update()
    {
        transform.Rotate(Vector3.forward * _rotateSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            _onAsteroidDeath.SetTrigger("Explosion");
            StartCoroutine(AsteroidExplosionRoutine());
        }
    }
    IEnumerator AsteroidExplosionRoutine()
    {
        _onAsteroidDeathAudioClip.Play();
        gameObject.GetComponent<PolygonCollider2D>().enabled = false;
        yield return new WaitForSeconds(0.2f);
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(1.8f);
        _spawnManager.StartSpawning();
        Destroy(this.gameObject);
    }
}
