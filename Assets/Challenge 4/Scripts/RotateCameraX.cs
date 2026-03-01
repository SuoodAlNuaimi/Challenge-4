using UnityEngine;

public class RotateCameraX : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 200f;
    [SerializeField] private Transform player;

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up, horizontal * rotationSpeed * Time.deltaTime);
        transform.position = player.position;
    }
}