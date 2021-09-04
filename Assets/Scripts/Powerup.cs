using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField] private float _speed = 3.0f;
    [SerializeField] private int _powerupID;
    //0 = Triple Shot, 1 = Speed Boost, 2 = Shields, 3 = Extra Life, 4 = Ammo Box

    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if(transform.position.y < -5.8f)
        {
            Destroy(this.gameObject);
        }        
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
                    default:
                        print("Default Value");
                        break;

                }
                
            }
            Destroy(this.gameObject);
        }
    }

   
}
