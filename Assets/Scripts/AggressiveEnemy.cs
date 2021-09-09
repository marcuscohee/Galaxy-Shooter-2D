using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AggressiveEnemy : MonoBehaviour
{
    [SerializeField] private float _speed = 6.0f;
    private Player _player;
    private Transform _playerPosition;
    [SerializeField] private Animator _onEnemyDeath;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _explosionClip;
    [SerializeField] private bool _isEnemyDead;
    [SerializeField] private GameObject _damageAnim;
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("The Player Script is NULL");
        }
        _playerPosition = GameObject.Find("Player").GetComponent<Transform>();
        if (_playerPosition == null)
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
    }

    void Update()
    {
        CalculateMovement();

        if (Vector3.Distance(_playerPosition.position, transform.position) <= 4f && _isEnemyDead == false)
        {
            RammingAttack();
            //If the Player gets within 4 meters of the Aggressive_Enemy, it will attempt to ram the player before dying.
                //Once the enemy dies, the animation will continue to move towards the player if _isEnemyDead is false. 
                    //So once true, the RammingAttack will stop.
        }
    }

    void RammingAttack()
    {
        Vector3 direction = _playerPosition.position - transform.position; //Tracks where the player's location is.
        transform.Translate(direction * Time.deltaTime); //Moves towards the Player at meters/seconds.
            //Since speed is already being called, it doesn't need to be here.
    }


    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -6.5f)
        {
            EnemyDeath(); //Since this Enemy is already damaged, it dies when it leaves the screen and gives no points.
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
                _player.AddScore(50); //Adds 50 points to the UI Display!
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
        _speed = 0;
        _damageAnim.SetActive(false);
        _isEnemyDead = true;
        _onEnemyDeath.SetTrigger("OnEnemyDeath");
        Destroy(GetComponent<PolygonCollider2D>());
        _audioSource.clip = _explosionClip;
        _audioSource.Play();
        Destroy(this.gameObject, 2.0f);
    }
}
