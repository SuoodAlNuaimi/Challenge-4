using UnityEngine;

public class SmashGemEffect : MonoBehaviour
{
    private float floatSpeed = 2f;
    private float floatHeight = 0.5f;
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        transform.Rotate(0, 60 * Time.deltaTime, 0);

        float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}