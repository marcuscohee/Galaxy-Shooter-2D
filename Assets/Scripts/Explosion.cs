using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Explosion : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    public void Start()
    {
        _audioSource.Play();
        GetComponent<Animator>().SetTrigger("Explosion");
        Destroy(this.gameObject, 2.0f);
    }

}
