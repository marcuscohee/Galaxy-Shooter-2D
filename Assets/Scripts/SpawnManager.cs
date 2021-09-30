using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject _enemyContainer;
    [SerializeField] private GameObject[] _commonEnemyPrefab;
    [SerializeField] private GameObject[] _uncommonEnemyPrefab;
    [SerializeField] private GameObject[] _rareEnemyPrefab;
    [SerializeField] private int _waveNumber = 1;
    [SerializeField] private bool _canSpawn = false;
    [SerializeField] private int _enemiesSpawned = 0;
    [SerializeField] private bool _areEnemiesStillSpawned = false;
    [SerializeField] private GameObject[] _commonPowerups;
    [SerializeField] private GameObject[] _uncommonPowerups;
    [SerializeField] private GameObject[] _rarePowerups;
    [SerializeField] private int _chance;
    [SerializeField] private UIManager _UIManager;

    private void Start()
    {
        _UIManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_UIManager == null)
        {
            Debug.LogError("The UIManager Script is NULL.");
        }
        _waveNumber = 1;
        _UIManager.WaveDisplay(_waveNumber);
    }
    public void StartSpawning()
    {
        _canSpawn = true;
        _areEnemiesStillSpawned = true;
        StartCoroutine(SpawnPowerupRoutine());
        StartCoroutine(SpawnEnemyRoutine());
    }

    private void Update()
    {
        _chance = Random.Range(1, 101);
    }

    IEnumerator WaveBreak()
    {
        while(_enemyContainer.transform.childCount > 0)
        {
            _areEnemiesStillSpawned = true;
            yield return new WaitForSeconds(0.1f);
        }
        _areEnemiesStillSpawned = false;
        _enemiesSpawned = 0;
        _waveNumber++;
        _UIManager.WaveDisplay(_waveNumber);
        yield return new WaitForSeconds(5.0f);
        _canSpawn = true;
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }
    IEnumerator SpawnEnemyRoutine()
    {
        while (_canSpawn == true)
        {
            if (_chance <= 50)
            {
                Vector3 enemySpawn = new Vector3(Random.Range(-9.5f, 9.5f), 7.4f, 0);
                GameObject newEnemy = Instantiate(_commonEnemyPrefab[Random.Range(0, 1)], enemySpawn, Quaternion.identity);
                newEnemy.transform.parent = _enemyContainer.transform;
            }
            else if (_chance >= 51 && _chance <= 80)
            {
                Vector3 enemySpawn = new Vector3(Random.Range(-9.5f, 9.5f), 7.4f, 0);
                GameObject newEnemy = Instantiate(_uncommonEnemyPrefab[Random.Range(0, 2)], enemySpawn, Quaternion.identity);
                newEnemy.transform.parent = _enemyContainer.transform;
            }
            else if (_chance >= 81 && _waveNumber >= 3)
            {
                Vector3 enemySpawn = new Vector3(Random.Range(-9.5f, 9.5f), 7.4f, 0);
                GameObject newEnemy = Instantiate(_rareEnemyPrefab[Random.Range(0, 1)], enemySpawn, Quaternion.identity);
                newEnemy.transform.parent = _enemyContainer.transform;
            }
            _enemiesSpawned++;
            if (_enemiesSpawned >= 15)
            {
                _canSpawn = false;
                StartCoroutine(WaveBreak());
            }
            yield return new WaitForSeconds(Random.Range(1.0f, 2.5f));
        }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        while (_areEnemiesStillSpawned == true || _canSpawn == true)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-9.5f, 9.5f), 7.4f, 0);
            if (_chance <= 50)
            {
                Instantiate(_commonPowerups[Random.Range(0, 1)], posToSpawn, Quaternion.identity);
                print("Common Powerup");
            }
            else if (51 <= _chance && _chance <= 85)
            {
                Instantiate(_uncommonPowerups[Random.Range(0, 4)], posToSpawn, Quaternion.identity);
                print("Uncommon Powerup");
            }
            else if (_chance >= 86)
            {
                Instantiate(_rarePowerups[Random.Range(0, 3)], posToSpawn, Quaternion.identity);
                print("Rare Powerup");
            }
            yield return new WaitForSeconds(Random.Range(3.0f, 4.0f));
        }
       
    }
    public void OnPlayerDeath()
    {
        _canSpawn = false;
    }
}
