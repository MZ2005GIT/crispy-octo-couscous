using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    [Header("Auto Setup - NO INSPECTOR NEEDED!")]
    [SerializeField] private float smoothSpeed = 0.125f;

    private Transform player;

    void Start()
    {
        // AUTO-FIND PLAYER (works instantly!)
        player = GameObject.FindWithTag("Player").transform;
        Debug.Log("Camera locked onto: " + player.name);
    }

    void LateUpdate()
    {
        if (player == null) return;

        // Follow player smoothly (2D style)
        Vector3 desiredPos = player.position;
        desiredPos.z = transform.position.z; // Keep camera distance

        transform.position = Vector3.Lerp(transform.position, desiredPos, smoothSpeed);
    }
}