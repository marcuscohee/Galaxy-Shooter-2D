using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField] private float _speed = 3.0f;
    [SerializeField] private Transform _player;
    [SerializeField] private bool _isPlayerPullingPowerups = false;
    [SerializeField] private int _powerupID;
    //0 = Triple Shot, 1 = Speed Boost, 2 = Shields, 3 = Extra Life, 4 = Ammo Box, 5 = Homing Drone, 6 = Spray Shot, 7 = Laser Jammer

    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Transform>();
        if(_player == null)
        {
            Debug.LogError("The Player is NULL");
        }
        _isPlayerPullingPowerups = false;
    }

    void Update()
    {
        if (_isPlayerPullingPowerups == true && _player != null)
        {
            Vector3 direction = _player.position - transform.position;
            direction.Normalize();
            transform.Translate(direction * _speed * 1.5f * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime, Space.World);
        }

        if(_powerupID == 7)
        {
            transform.Rotate(Vector3.forward * 20 * Time.deltaTime);
        }
        
        if(transform.position.y < -5.8f)
        {
            Destroy(this.gameObject);
        }
    }

    public void PlayerIsPullingPowerup()
    {
        _isPlayerPullingPowerups = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if(player != null)
            {
                switch(_powerupID)
                {
                    case 0:
                        player.ActivateTripleShot();
                        break;
                    case 1:
                        player.ActivateSpeedBoostPowerup();
                        break;
                    case 2:
                        player.ActivateShieldPowerup();
                        break;
                    case 3:
                        player.ExtraLife();
                        break;
                    case 4:
                        player.AmmoPickup();
                        break;
                    case 5:
                        player.ActivateHomingDrone();
                        break;
                    case 6:
                        player.ActivateSprayShot();
                        break;
                    case 7:
                        player.JamLasers();
                        break;
                    default:
                        print("Default Value");
                        break;
                }
                
            }
            Destroy(this.gameObject);
        }
    }

   
}
