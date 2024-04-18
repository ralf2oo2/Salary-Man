using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartrateMonitor : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip beep;
    [SerializeField] AudioClip flatline;
    [SerializeField] GameObject gate;

    private bool flatlining = false;
    private float delay = 1f;
    private float currentTime = 0f;
    private bool isDead = false;

    public void Flatline()
    {
        flatlining = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentTime > delay && delay != 0)
        {
            audioSource.PlayOneShot(beep);
            currentTime = 0f;
            if (flatlining)
            {
                gate.SetActive(true);
                delay -= 0.1f;
            }
        }
        if(delay <= 0 && !isDead)
        {
            audioSource.loop = true;
            audioSource.clip = flatline;
            audioSource.Play();
            isDead = true;
            StateManager.states.Add("escape", true);
            ObjectiveManager.SetObjective("Escape");
        }
        currentTime += Time.deltaTime;
    }
}
