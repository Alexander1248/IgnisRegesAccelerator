using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSound : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] clips;
    [SerializeField] private Transform player;

    [SerializeField] private float[] timeRange;
    [SerializeField] private float[] radiusRange;

    void Start(){
        Invoke("PlaySound", Random.Range(timeRange[0], timeRange[1]));
    }

    void PlaySound(){
        Vector3 dir = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        audioSource.transform.position = player.position + dir.normalized * Random.Range(radiusRange[0], radiusRange[1]);
        audioSource.clip = clips[Random.Range(0, clips.Length)];
        audioSource.Play();

        Invoke("PlaySound", Random.Range(timeRange[0], timeRange[1]));
    }
}
