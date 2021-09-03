using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed = 4.0f;

    private Player _player;
    [SerializeField] private Animator _onEnemyDeath;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _explosionClip;
    [SerializeField] private AudioClip _laserClip;
    [SerializeField] private GameObject _enemyLaserPrefab;
    [SerializeField] private Shield _shield;
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
        StartCoroutine(FireEnemyLaserRoutine());
    }

    void Update()
    {
        CalculateMovement();
        
    }

    void CalculateMovement()
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
            _isEnemyDead = true;
            Player player = other.transform.GetComponent<Player>();
            if(player != null)
            {
                player.Damage();
            }
            _speed = 0;
            _onEnemyDeath.SetTrigger("OnEnemyDeath");
            Destroy(GetComponent<PolygonCollider2D>());
            _audioSource.clip = _explosionClip;
            _audioSource.Play();
            
            Destroy(this.gameObject, 2.0f);
        }

        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            _isEnemyDead = true;
            if (_player != null)
            {
                _player.AddScore(10);
            }
            _speed = 0;
            _onEnemyDeath.SetTrigger("OnEnemyDeath");
            Destroy(GetComponent<PolygonCollider2D>());
            _audioSource.clip = _explosionClip;
            _audioSource.Play();
            Destroy(this.gameObject, 2.0f);
        }
        if (other.tag == "Shield")
        {
            other.GetComponent<Shield>().DamageShield();
            _isEnemyDead = true;
            _speed = 0;
            _onEnemyDeath.SetTrigger("OnEnemyDeath");
            Destroy(GetComponent<PolygonCollider2D>());
            _audioSource.clip = _explosionClip;
            _audioSource.Play();
            Destroy(this.gameObject, 2.0f);
        }
    }

    IEnumerator FireEnemyLaserRoutine()
    {
        _audioSource.clip = _laserClip;
        //Assigns the Laser sound effect to the Audio Source component.
        _audioSource.Play();
        //Then plays the clip.
        Instantiate(_enemyLaserPrefab, transform.position + (Vector3.down * 0.75f), Quaternion.identity);
            //Fires laser at spawn!                (Vector3.down * 0.75f) is the laser offset, same as Player.
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(3f, 7f));
                //Now the game will wait 3-7 seconds, then call this if statement.
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
