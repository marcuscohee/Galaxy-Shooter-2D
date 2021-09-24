using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] private int _shieldHealth = 3;
   
    public void ActivateShield()
    {
        gameObject.SetActive(true);
        _shieldHealth = 3;
        ShieldHealth();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Enemy_Laser")
        {
            Destroy(other.gameObject);
            DamageShield();
        }
        if (other.tag == "Enemy_Shield")
        {
            Destroy(other.gameObject);
            DamageShield();
        }
    }

    public void DamageShield()
    {
        _shieldHealth--;
        ShieldHealth();
    }

    void ShieldHealth()
    {
       switch (_shieldHealth)
        {
            case 0:
                gameObject.SetActive(false);
                DeactivateShield();
                break;
            case 1:
                GetComponent<SpriteRenderer>().color = Color.red;
                break;
            case 2:
                GetComponent<SpriteRenderer>().color = Color.yellow;
                break;
            case 3:
                GetComponent<SpriteRenderer>().color = Color.white;
                break;
        }

    }

    public void DeactivateShield()
    {
        GetComponentInParent<Player>().OnShieldBreak();
    }

}
