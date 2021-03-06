using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Player : MonoBehaviour
{
    //Player Movement
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _speedMultiplier = 2;
    [SerializeField] private float _thrusterLimit = 100;
    [SerializeField] private bool _isThrusterCoolingDown = false;
    //Firing Laser
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private float _fireRate = 0.15f;
    private float _canFire = -1f;
    [SerializeField] private int _laserCount = 15;
    [SerializeField] private GameObject _outOfAmmoLight_Left;
    [SerializeField] private GameObject _outOfAmmoLight_Right;
    [SerializeField] private GameObject _tripleShot;
    [SerializeField] private bool _isTripleShotActive = false;
    [SerializeField] private DroneManager[] _droneManager;
    [SerializeField] private GameObject _sprayShot;
    [SerializeField] private bool _isSprayShotActive = false;
    [SerializeField] private bool _areLasersJammed = false;
    //Damage
    [SerializeField] private int _lives = 3;
    [SerializeField] private GameObject _rightEngine;
    [SerializeField] private GameObject _leftEngine;
    [SerializeField] private GameObject _shieldVisualizer;
    [SerializeField] private bool _isShieldActive = false;
    [SerializeField] private Transform _player_Explosion;
    [SerializeField] private Explosion _player_Explosion_Anim;
    [SerializeField] private Boss _boss;
    //SpawnManager/UIManager
    private SpawnManager _spawnManager;
    [SerializeField] private int _score;
    private UIManager _uiManager;
    //Sound
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _laserSound;
    [SerializeField] private AudioClip _powerupSound;

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
        _laserCount = 15;
    }


    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && _areLasersJammed == false && Time.time > _canFire)
        {
            FireLaser();
        }
        
        if (Input.GetKeyDown(KeyCode.C))
        {
            PullPowerups();
        }
    }
    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        //thruster
        if (Input.GetKey(KeyCode.LeftShift) && _thrusterLimit > 0)
        {
            //When the Left Shift is pressed and the _thrusterLimit is greater than 0, do this.
            StopCoroutine("ThrusterCoolDownRoutine");
            _isThrusterCoolingDown = false;
            //Since you are using the Thruster, its to hot to recharge.
            _thrusterLimit -= (20 * Time.deltaTime);
            //This is the rate of burn which is roughly 5 seconds
            transform.Translate((_speed + 2) * Time.deltaTime * direction);
            //Boost for 5 seconds.
        }
        else
        {
            //When the Left Shift is not pressed, so this.
            StartCoroutine("ThrusterCoolDownRoutine");
            //This coroutine is below, after 2 seconds, it will fully recharge in 2 seconds
                //But only if Shift isn't pressed again.
            transform.Translate(_speed * Time.deltaTime * direction);
            //Normal movement.
        }

        if(_thrusterLimit >= 100)
        {
            //This makes it so _thrusterLimit can't go above 100.
            _thrusterLimit = 100;
        }
        else if(_isThrusterCoolingDown == true)
        {
            //if allowed, this will recharge the _thrusterLimit in 2 seconds.
            _thrusterLimit += (50 * Time.deltaTime);
        }

        _uiManager.ThrusterGauge(_thrusterLimit);
        //This is used for the Thruster Gauge in the UI.

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

    IEnumerator ThrusterCoolDownRoutine()
    {
        yield return new WaitForSeconds(2.0f);
        _isThrusterCoolingDown = true;
        //If the thrusters aren't used for 2 seconds.
            //Start the recharge!
    }

    void FireLaser()
    {
        
        if (_laserCount >= 1)
        {
            //if ammo is 1 or more, then fire laser.
            _laserCount--;
            //Subtract 1 from _ammoCount.
            _canFire = Time.time + _fireRate;

            if (_isTripleShotActive == true)
            {
                Instantiate(_tripleShot, transform.position, Quaternion.identity);
            }
            else if(_isSprayShotActive == true)
            {
                Instantiate(_sprayShot, transform.position, Quaternion.identity);
            }
            else
            {
                Instantiate(_laserPrefab, transform.position + (Vector3.up * 1.05f), Quaternion.identity);
            }

            _audioSource.clip = _laserSound;
            _audioSource.Play();
            _uiManager.LaserCount(_laserCount);
            //Send _ammoCount value over to the UIManager.
        }
        else if(_laserCount < 1)
        {
            StartCoroutine(WhenOutOfAmmoRoutine());
            //Makes the ship blink when out of ammo.
        }
    }
    public void AmmoPickup()
    {
        _laserCount += 5;
        //Giving the Player 5 more ammo.
        if(_laserCount >= 15)
        {
            _laserCount = 15;
        }
        _uiManager.LaserCount(_laserCount);
        //Tell the UIManager our ammo count.
        _audioSource.PlayOneShot(_powerupSound, 1);
        //Powerup Sound
    }

    IEnumerator WhenOutOfAmmoRoutine()
    {
        _outOfAmmoLight_Left.gameObject.SetActive(true);
        _outOfAmmoLight_Right.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        _outOfAmmoLight_Left.gameObject.SetActive(false);
        _outOfAmmoLight_Right.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        _outOfAmmoLight_Left.gameObject.SetActive(true);
        _outOfAmmoLight_Right.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        _outOfAmmoLight_Left.gameObject.SetActive(false);
        _outOfAmmoLight_Right.gameObject.SetActive(false);
    }

    public void JamLasers()
    {
        _areLasersJammed = true;
        StartCoroutine(UnjammingRoutine());
    }
   
    IEnumerator UnjammingRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        _areLasersJammed = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy_Laser")
            //if Enemy_Laser hits the Player.
        {
            Destroy(other.gameObject);
                //Destroy the Laser
            Damage();
                //Then call the Damage() method.
        }
    }

    public void Damage()
    {
        
        if(_isShieldActive == true)
        {
            return;
        }

        StartCoroutine("InvulnerabilityRoutine");
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
            _lives = 0;
            PlayerDeath();
            _spawnManager.OnPlayerDeath();
        }
    }

    public void ExtraLife()
    {
        _lives++;
        //Add a life.
        if(_lives >= 3)
        {
            _lives = 3;
            _rightEngine.SetActive(false);
            //Heal the right damaged engine.
        }

        else if (_lives == 2)
        {
            _leftEngine.SetActive(false);
            //Heal the left damaged engine.
        }

        _uiManager.UpdateLives(_lives);
        //Send the UiManager that we got an extra life.
            //Then reflect it on the UI.
        _audioSource.PlayOneShot(_powerupSound, 1);
        //Powerup Sound
    }
    
    void PlayerDeath()
    {
        _spawnManager.OnPlayerDeath();
        _player_Explosion.gameObject.SetActive(true);
        _player_Explosion.position = transform.position;
        Destroy(this.gameObject);
    }
   

    IEnumerator InvulnerabilityRoutine()
    { //TODO: Set this to while statement after blink with a int variable that repeats blinking 3 times

        GetComponent<PolygonCollider2D>().enabled = false;
        //Turn off the Collider, this makes you invulnerable.
        GetComponent<SpriteRenderer>().enabled = false;
            //Turn off the SpriteRenderer to simulate blinking.
        yield return new WaitForSeconds(0.20f);
            //Then after 0.20 seconds, turn the SpriteRenderer on.
        GetComponent<SpriteRenderer>().enabled = true;
        yield return new WaitForSeconds(0.20f);
        GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(0.20f);
        GetComponent<SpriteRenderer>().enabled = true;
        yield return new WaitForSeconds(0.20f);
        GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(0.20f);
        GetComponent<SpriteRenderer>().enabled = true;
            //At this point, 1 second has passed and blinked 3 times.
        GetComponent<PolygonCollider2D>().enabled = true;
            //Turn the Collider back on, making you vulnerable again.
    }

    void PullPowerups()
    {
        GameObject[] callPowerups = GameObject.FindGameObjectsWithTag("Powerup");
        if (callPowerups.Length >= 1)
        {
            Debug.Log("Powerups on screen: " + callPowerups.Length);
            callPowerups[0].GetComponent<Powerup>().PlayerIsPullingPowerup();
            callPowerups[0].tag = "Pulled_Powerup";
        }
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

    void AmmoRefresher()
    {
        _isSprayShotActive = false;
        _isTripleShotActive = false;
    }
    public void ActivateTripleShot()
    {
        StopCoroutine("TripleShotPowerDownRoutine");
        AmmoRefresher();
        _isTripleShotActive = true;
        _audioSource.PlayOneShot(_powerupSound, 1);
        StartCoroutine("TripleShotPowerDownRoutine");
    }
    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }

    public void ActivateSprayShot()
    {
        StopCoroutine("SprayShotPowerDownRoutine");
        AmmoRefresher();
        _isSprayShotActive = true;
        _audioSource.PlayOneShot(_powerupSound, 1);
        StartCoroutine("SprayShotPowerDownRoutine");
    }

    IEnumerator SprayShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isSprayShotActive = false;
    }

    public void ActivateHomingDrone()
    {
        _audioSource.PlayOneShot(_powerupSound, 1);
        //when you pick up a drone powerup, add all 4 drones around the player.
        _droneManager[0].GetComponent<DroneManager>().DronePickup();
        _droneManager[1].GetComponent<DroneManager>().DronePickup();
        _droneManager[2].GetComponent<DroneManager>().DronePickup();
        _droneManager[3].GetComponent<DroneManager>().DronePickup();
    }

    public void ActivateSpeedBoostPowerup()
    {
        _speed *= _speedMultiplier;
        _audioSource.PlayOneShot(_powerupSound, 1);
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _speed /= _speedMultiplier;
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }    
}