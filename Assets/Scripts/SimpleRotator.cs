using UnityEngine;

/// <summary>
/// Rotación continua simple para los modelos de prueba en la galería de shaders y el monolito central.
/// </summary>
public class SimpleRotator : MonoBehaviour
{
    [SerializeField] private Vector3 axis = Vector3.up;
    [SerializeField] private float speed = 30f;

    private void Update()
    {
        transform.Rotate(axis, speed * Time.deltaTime);
    }
}
