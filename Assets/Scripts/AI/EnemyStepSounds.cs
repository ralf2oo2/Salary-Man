using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStepSounds : MonoBehaviour
{
    [SerializeField] AudioSource footStepAudioSource;
    [SerializeField] AudioClip[] footStepAudioClips;
    void PlayFootstepSound()
    {
        footStepAudioSource.PlayOneShot(footStepAudioClips[Random.Range(0, footStepAudioClips.Length - 1)]);
    }
}
