using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed = 4.0f;
    [SerializeField] private bool _isEnemyDead = false;

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

        if (transform.position.y < -6.5f && _isEnemyDead == true)
        {
            Destroy(this.gameObject);
        }

        if (_isEnemyDead == true)
        {
            transform.GetComponent<BoxCollider2D>().enabled = false;
        }

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
            _isEnemyDead = true;
            StartCoroutine(EnemyDeathAnimRoutine());
        }

        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            
            if(_player != null)
            {
                _player.AddScore(10);
            }
            _isEnemyDead = true;
            StartCoroutine(EnemyDeathAnimRoutine());
        }

        IEnumerator EnemyDeathAnimRoutine()
        {
            _onEnemyDeath.SetTrigger("OnEnemyDeath");
            yield return new WaitForSeconds(2.38f);
            Destroy(this.gameObject);
        }
       
    }
}
