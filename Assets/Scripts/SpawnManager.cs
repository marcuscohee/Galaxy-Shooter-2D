using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject _enemyContainer;
    [SerializeField] private GameObject[] _commonEnemyPrefab;
    [SerializeField] private GameObject[] _uncommonEnemyPrefab;
    [SerializeField] private GameObject[] _rareEnemyPrefab;
    private bool _stopSpawning = false;
    [SerializeField] private GameObject[] _commonPowerups;
    [SerializeField] private GameObject[] _uncommonPowerups;
    [SerializeField] private GameObject[] _rarePowerups;
    [SerializeField] private int _chance;

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    private void Update()
    {
        _chance = Random.Range(1, 101);
    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(2.0f);
        while (_stopSpawning == false)
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
            else if (_chance >= 81)
            {
                Vector3 enemySpawn = new Vector3(Random.Range(-9.5f, 9.5f), 7.4f, 0);
                GameObject newEnemy = Instantiate(_rareEnemyPrefab[Random.Range(0, 1)], enemySpawn, Quaternion.identity);
                newEnemy.transform.parent = _enemyContainer.transform;
            }
            
            yield return new WaitForSeconds(Random.Range(3.0f, 5.0f));
        }

    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(2.0f);
        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-9.5f, 9.5f), 7.4f, 0);
            if (_chance <= 50)
            {
                Instantiate(_commonPowerups[Random.Range(0, 1)], posToSpawn, Quaternion.identity);
                print("Common Powerup");
            }
            else if (51 <= _chance && _chance <= 85)
            {
                Instantiate(_uncommonPowerups[Random.Range(0, 3)], posToSpawn, Quaternion.identity);
                print("Uncommon Powerup");
            }
            else if (_chance >= 86)
            {
                Instantiate(_rarePowerups[Random.Range(0, 3)], posToSpawn, Quaternion.identity);
                print("Rare Powerup");
            }
            yield return new WaitForSeconds(Random.Range(3.0f, 5.0f));
        }
       
    }
    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

}
