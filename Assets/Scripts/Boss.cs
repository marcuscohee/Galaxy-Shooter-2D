using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] private float _speed = 1f;
    [SerializeField] private float _rotateSpeed = 15f;
    [SerializeField] private GameObject _mainLaser;
    [SerializeField] private Transform _mainLaserTurret;
    [SerializeField] private Transform[] _sideLensesTurrets;
    [SerializeField] private GameObject _sideLaser;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private bool _isBossAlive = true;
    [SerializeField] private bool _isPlayerAlive = true;
    [SerializeField] private bool _isBossMoving = true;
    private GameObject _player;
    [SerializeField] private UIManager _UIManager;
    [SerializeField] private SpawnManager _spawnManager;
    [SerializeField] private int _health = 50;
    [SerializeField] private GameObject _explosion;

    void Start()
    {
        _player = GameObject.Find("Player");
        if(_player == null)
        {
            Debug.LogError("The Player is NULL");
        }
        _UIManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if(_UIManager == null)
        {
            Debug.LogError("The UIManager Script is NULL");
        }
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if(_spawnManager == null)
        {
            Debug.LogError("The Spawn_Manager Script is NULL");        
        }
        transform.position = new Vector2(0, 10);
        StartCoroutine(FireMainLaser());
        StartCoroutine(FireSideLasers());
        _UIManager.BossWave();
    }

    void Update()
    {        
        if (transform.position.y > 5.4f)
        {
            transform.Translate(Vector2.down * _speed * Time.deltaTime, Space.World);
            return;
        }

        _isBossMoving = false;
        transform.Rotate(Vector3.forward * _rotateSpeed * Time.deltaTime);
    }

    IEnumerator FireMainLaser()
    {
        yield return new WaitForSeconds(5.0f);
        while (_isBossAlive == true && _isPlayerAlive == true)
        {
            var newRotation = Quaternion.LookRotation(transform.position - _player.transform.position, Vector3.back);
            newRotation.x = 0.0f;
            newRotation.y = 0.0f;
            Instantiate(_mainLaser, transform.position, _mainLaserTurret.transform.rotation = newRotation);
            _audioSource.Play();
            yield return new WaitForSeconds(2.0f);
        }
    }

    IEnumerator FireSideLasers()
    {
        yield return new WaitForSeconds(5.0f);
        while(_isBossAlive == true)
        {
            Instantiate(_sideLaser, _sideLensesTurrets[0].transform.position, _sideLensesTurrets[0].transform.rotation);
            Instantiate(_sideLaser, _sideLensesTurrets[1].transform.position, _sideLensesTurrets[1].transform.rotation);
            Instantiate(_sideLaser, _sideLensesTurrets[2].transform.position, _sideLensesTurrets[2].transform.rotation);
            Instantiate(_sideLaser, _sideLensesTurrets[3].transform.position, _sideLensesTurrets[3].transform.rotation);
            _audioSource.Play();
            yield return new WaitForSeconds(1.5f);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Laser")
        {
            Destroy(other.gameObject);
            Damage();
        }

        if(other.tag == "Drone")
        {
            Destroy(other.gameObject);
            Damage();
        }
    }

    void Damage()
    {
        if(_isBossMoving == true)
        {
            return;
        }

        _health--;

        _UIManager.BossHealth(_health);

        if(_health <= 0)
        {
            _isBossAlive = false;
            _player.GetComponent<Player>().AddScore(10000);
            _spawnManager.OnBossDeath();
            //_UIManager.YouWin(); will manage this in the UIManager.
            StartCoroutine(OnBossDeath());
        }
    }

    IEnumerator OnBossDeath()
    {
        Destroy(GetComponent<PolygonCollider2D>());
        _explosion.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        Destroy(GetComponent<SpriteRenderer>());
        yield return new WaitForSeconds(1.8f);
        Destroy(this.gameObject);
    }

    public void OnPlayerDeath()
    {
        _isPlayerAlive = false;
    }


}
