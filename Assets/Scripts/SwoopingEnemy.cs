using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwoopingEnemy : MonoBehaviour
{
    [SerializeField] private float _speed = 4.0f;
    private Player _player;
    [SerializeField] private Animator _onEnemyDeath;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _explosionClip;
    [SerializeField] private AudioClip _laserClip;
    [SerializeField] private GameObject _enemyLaserPrefab;
    [SerializeField] private int _spawnDirection;
    private bool _isEnemyDead = false;
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("The Player is NULL");
        }
        _onEnemyDeath = gameObject.GetComponent<Animator>();
        if (_onEnemyDeath == null)
        {
            Debug.LogError("The OnEnemyDeath Animation is NULL");
        }
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.LogError("The Enemy Explosion Audio Source is NULL");
        }
        SpawnDirection();
        StartCoroutine(FireEnemyLaserRoutine());
    }
    void Update()
    {
        CalculateMovement();
    }

    void SpawnDirection()
    {
        _spawnDirection = Random.Range(1, 3); //random.range(1, 3) will be used to determine the starting spawn location.
        if (_spawnDirection == 1) //start on the left
        {
            transform.position = new Vector3(-11.1f, 5.6f, 0);
        }
        else if (_spawnDirection == 2) //start on the right
        {
            transform.position = new Vector3(11.1f, 5.6f, 0);
        }
    }

    void CalculateMovement()
    {
        
        if (_spawnDirection == 1) //start on the left
        {
            transform.Translate(new Vector3(0.87f, -0.13f, 0) * _speed * Time.deltaTime);
        }
        else if(_spawnDirection == 2) //start on the right
        {
            transform.Translate(new Vector3(-0.87f, -0.13f, 0) * _speed * Time.deltaTime);
        }
    
        if (transform.position.y < 2.3f)
        {
            transform.tag = "Enemy"; // if targeted, it will make the drone reroll its target on respawn.
            SpawnDirection();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }
            EnemyDeath();
        }

        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            if (_player != null)
            {
                _player.AddScore(10);
            }
            EnemyDeath();
        }

        if (other.tag == "Shield")
        {
            other.GetComponent<Shield>().DamageShield();
            EnemyDeath();
        }
    }

    void EnemyDeath()
    {
        transform.tag = "Targeted_Enemy";
        _isEnemyDead = true;
        _speed = 0;
        _onEnemyDeath.SetTrigger("OnEnemyDeath");
        Destroy(GetComponent<PolygonCollider2D>());
        _audioSource.clip = _explosionClip;
        _audioSource.Play();
        Destroy(this.gameObject, 2.0f);
    }

    IEnumerator FireEnemyLaserRoutine()
    {

        while (true)
        {
            yield return new WaitForSeconds(1f);
            //Now the game will wait 3-7 seconds, then call the if statement.
            if (_isEnemyDead == false)
            {
                //If _isEnemyDead = false, then fire laser again!
                //If true, then the Enemy is dead and can't fire.
                Instantiate(_enemyLaserPrefab, transform.position + (Vector3.down * 0.75f), Quaternion.identity);
                _audioSource.clip = _laserClip;
                _audioSource.Play();
            }
        }
    }


}
