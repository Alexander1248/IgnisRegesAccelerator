using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveShips : MonoBehaviour
{
    [SerializeField] private float speedClouds;
    [SerializeField] private Transform[] clouds;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform respawnPoint;

    [SerializeField] private Transform cloudsParent;
    
    void FixedUpdate()
    {
        cloudsParent.position = Vector3.MoveTowards(
            cloudsParent.position, cloudsParent.position + new Vector3(0, 0, -1), speedClouds * Time.fixedDeltaTime);

        for(int i = 0; i < clouds.Length; i++){
            if (clouds[i].position.z <= respawnPoint.position.z){
                clouds[i].position = new Vector3(clouds[i].position.x, clouds[i].position.y, spawnPoint.position.z);
            }
        }
    }
}
