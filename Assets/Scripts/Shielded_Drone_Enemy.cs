using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shielded_Drone_Enemy : MonoBehaviour
{
    [SerializeField] private float _speed = 4.0f;
    private Player _player;
    [SerializeField] private GameObject _explosionAnimPrefab;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _laserClip;
    [SerializeField] private GameObject _doubleLaserPrefab;
    [SerializeField] private GameObject _enemyShield;
    [SerializeField] private bool _isShieldActive = true;
    private bool _isEnemyDead = false;
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("The Player is NULL");
        }
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.LogError("The Enemy Explosion Audio Source is NULL");
        }
        StartCoroutine(FireEnemyLaserRoutine());
        _enemyShield.gameObject.SetActive(true);
        _isShieldActive = true;
    }

    void Update()
    {
        CalculateMovement();
    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime, Space.World);

        if (transform.position.y < -6.5f)
        {
            transform.tag = "Enemy"; // if targeted, it will stop the drone from making a B-line to this Enemy after respawn.
            float respawn = Random.Range(-9.5f, 9.5f);
            transform.position = new Vector3(respawn, 7.4f, 0);
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
                _player.AddScore(50);
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
        if(_isShieldActive == true)
        {
            _enemyShield.gameObject.SetActive(false);
            _isShieldActive = false;
            transform.tag = "Enemy"; // this will allow a second drone to hit it after the shield goes down.
            return;
        }
        transform.tag = "Targeted_Enemy";
        _isEnemyDead = true;
        _speed = 0;
        StartCoroutine(ExplosionRoutine());
        Destroy(GetComponent<PolygonCollider2D>());
        _audioSource.Play();
        Destroy(this.gameObject, 2.0f);
    }
    IEnumerator ExplosionRoutine()
    {
        Instantiate(_explosionAnimPrefab, transform.position, Quaternion.identity);
        //_explosionAnimPrefab.GetComponent<Explosion>().OnDeathExplosion();
        yield return new WaitForSeconds(0.2f);
        GetComponent<SpriteRenderer>().enabled = false;
    }

    IEnumerator FireEnemyLaserRoutine()
    {
        _audioSource.clip = _laserClip;
        //Assigns the Laser sound effect to the Audio Source component.
        _audioSource.Play();
        //Then plays the clip.
        Instantiate(_doubleLaserPrefab, transform.position + (Vector3.down * 0.65f), Quaternion.identity);
        //Fires laser at spawn!
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(3f, 7f));
            //Now the game will wait 3-7 seconds, then call this if statement.
            if (_isEnemyDead == false)
            {
                //If _isEnemyDead = false, then fire laser again!
                //If true, then the Enemy is dead and can't fire.
                Instantiate(_doubleLaserPrefab, transform.position + (Vector3.down * 0.65f), Quaternion.identity);
                _audioSource.clip = _laserClip;
                _audioSource.Play();
            }
        }
    }
}
