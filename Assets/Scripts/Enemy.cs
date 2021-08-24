using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed = 4.0f;

    private Player _player;
    [SerializeField] private Animator _onEnemyDeath;
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if(_player == null)
        {
            Debug.LogError("The Player is NULL");
        }
        _onEnemyDeath = gameObject.GetComponent<Animator>();
        if (_onEnemyDeath == null)
        {
            Debug.LogError("The OnEnemyDeath Animation is NULL");
        }
    }

    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -6.5f)
        {
            float respawn = Random.Range(-9.5f, 9.5f);
            transform.position = new Vector3(respawn, 7.4f, 0);
        }
       
        

    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if(player != null)
            {
                player.Damage();
            }
            _speed = 0;
            _onEnemyDeath.SetTrigger("OnEnemyDeath");
            StartCoroutine(ExplosionRoutine());
            Destroy(this.gameObject, 2.0f);
        }

        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);

            if (_player != null)
            {
                _player.AddScore(10);
            }
            _speed = 0;
            _onEnemyDeath.SetTrigger("OnEnemyDeath");
            StartCoroutine(ExplosionRoutine());
            Destroy(this.gameObject, 2.0f);
        }   
    }

    IEnumerator ExplosionRoutine()
    {
        yield return new WaitForSeconds(0.4f);
        GetComponent<BoxCollider2D>().enabled = false;
    }
}
