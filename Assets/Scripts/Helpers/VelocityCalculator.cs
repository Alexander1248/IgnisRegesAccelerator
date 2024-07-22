using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityCalculator : MonoBehaviour
{
    /*
    скрипт считает велосити объекта (корабля) 
    чтобы игрок учитывал его при своем движении (двигался вместе с кораблем)
    */

    private Vector3 _previousPosition;
    private Vector3 _velocity;

    private void Start()
    {
        _previousPosition = transform.position;
    }

    private void Update()
    {
        _velocity = (transform.position - _previousPosition) / Time.deltaTime;
        Debug.Log(transform.position + " " + _previousPosition + " " + _velocity);
        _previousPosition = transform.position;
    }

    public Vector3 GetVelocity()
    {
        return _velocity;
    }
}
