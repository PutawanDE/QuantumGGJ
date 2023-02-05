using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soundplayer : MonoBehaviour
{

    public AudioSource audioSource;

    private void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable() {
        Character.OnHurt += PlaySound;
    }

    private void OnDisable() {
        Character.OnHurt -= PlaySound;
    }

    private void PlaySound() {

        audioSource.Play();
        
    }
}
