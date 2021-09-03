using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Player : MonoBehaviour
{
    //Player Movement
    [SerializeField] private float _speed = 5f;
    [SerializeField] private bool _isSpeedBoostActive = false;
    //Firing Laser
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private float _fireRate = 0.15f;
    private float _canFire = -1f;
    [SerializeField] private GameObject _tripleShot;
    [SerializeField] private bool _isTripleShotActive = false;
    //Damage
    [SerializeField] private int _lives = 3;
    [SerializeField] private GameObject _rightEngine;
    [SerializeField] private GameObject _leftEngine;
    [SerializeField] private GameObject _shieldVisualizer;
    [SerializeField] private bool _isShieldActive = false;
    [SerializeField] private Transform _player_Explosion;
    [SerializeField] private Explosion _player_Explosion_Anim;
    //SpawnManager/UIManager
    private SpawnManager _spawnManager;
    [SerializeField] private int _score;
    private UIManager _uiManager;
    //Sound
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _laserSound;
    [SerializeField] private AudioClip _powerupSound;
    [SerializeField] private AudioClip _explosionSound;


    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is NULL");
        }
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_uiManager == null)
        {
            Debug.LogError("The UI Manager is NULL");
        }
        _audioSource = GetComponent<AudioSource>();
        if(_audioSource == null)
        {
            Debug.LogError("The Audio Source on the Player is NULL");
        }
        _player_Explosion = GameObject.Find("Player_Explosion").GetComponent<Transform>();
        _player_Explosion_Anim = GameObject.Find("Player_Explosion").GetComponent<Explosion>();
        if(_player_Explosion == null)
        {
            Debug.LogError("The Player Explosion is NULL");
        }
    }


    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }
    }
    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        
        //movement
        if(_isSpeedBoostActive == true)
        {
            _speed = 10;
            transform.Translate(_speed * Time.deltaTime * direction);
        }
        else
        {
            _speed = 5f;
            transform.Translate(_speed * Time.deltaTime * direction);
        }
        

        //vertical player bounds
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);

        //horizontal player wrapping
        if (transform.position.x >= 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x <= -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
    }
    void FireLaser()
    {
      _canFire = Time.time + _fireRate;
      
        if(_isTripleShotActive == true)
        {
            Instantiate(_tripleShot, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + (Vector3.up * 1.05f), Quaternion.identity);
        }

        _audioSource.clip = _laserSound;
        _audioSource.Play();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy_Laser")
            //if Enemy_Laser hits the Player.
        {
            Destroy(other.gameObject);
                //Destroy the Laser
            Damage();
                //Then call the Damage() method, thus losing a life!
        }
    }

    public void Damage()
    {
        
        if(_isShieldActive == true)
        {
            return;
        }

             
        _lives -= 1;

        if (_lives == 2)
        {
            _rightEngine.SetActive(true);
        }

        else if (_lives == 1)
        {
            _leftEngine.SetActive(true);
        }

        _uiManager.UpdateLives(_lives);

        if(_lives < 1)
        {
            PlayerDeath();
            _spawnManager.OnPlayerDeath();
        }
    }
    
    void PlayerDeath()
    {
        _spawnManager.OnPlayerDeath();
        _player_Explosion_Anim.OnDeathExplosion();
        _player_Explosion.position = transform.position;
        Destroy(this.gameObject);
    }
    public void ActivateShieldPowerup()
    {
        _isShieldActive = true;
        _audioSource.PlayOneShot(_powerupSound, 1);
        _shieldVisualizer.GetComponent<Shield>().ActivateShield();
    }

    public void OnShieldBreak()
    {
        _isShieldActive = false;
    }

    public void ActivateTripleShot()
    {
        _isTripleShotActive = true;
        _audioSource.PlayOneShot(_powerupSound, 1);
        StartCoroutine(TripleShotPowerDownRoutine());
    }
    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }

    public void ActivateSpeedBoostPowerup()
    {
        _isSpeedBoostActive = true;
        _audioSource.PlayOneShot(_powerupSound, 1);
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isSpeedBoostActive = false;
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }
    
}

