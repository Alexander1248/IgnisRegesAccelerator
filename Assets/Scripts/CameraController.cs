using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform fixationPoint;
    public Vector2 minAngles = new(-120, -60);
    public Vector2 maxAngles = new(120, 60);
    public float distance;
    public bool lockRotation;


    private void Start()
    {
        
    }

    void Update()
    {
        if (lockRotation) return;
    }

    public void LockMouseMovement()
    {
        
    }
}
