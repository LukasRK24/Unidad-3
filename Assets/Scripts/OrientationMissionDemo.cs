using UnityEngine;

/// <summary>
/// Script de prueba para la Actividad 1 de la Semana 9:
/// permite comparar el movimiento en espacio local (Space.Self) frente al espacio global (Space.World).
/// </summary>
public class OrientationMissionDemo : MonoBehaviour
{
    [SerializeField] private float localSpeed = 2.0f;
    [SerializeField] private bool useLocalSpace = true;

    private void Update()
    {
        if (useLocalSpace)
        {
            // Se desplaza según la rotación propia del objeto (eje Z local)
            transform.Translate(Vector3.forward * localSpeed * Time.deltaTime, Space.Self);
        }
        else
        {
            // Se desplaza hacia el eje Z global del mundo sin importar hacia dónde gire el objeto
            transform.Translate(Vector3.forward * localSpeed * Time.deltaTime, Space.World);
        }
    }
}
