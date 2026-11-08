using UnityEngine;
using Unity.Cinemachine;

/// <summary>
/// Cambia la prioridad de una cámara virtual de Cinemachine al entrar o salir de una zona con Trigger.
/// </summary>
public class CameraZoneTrigger : MonoBehaviour
{
    [SerializeField] private CinemachineCamera targetCamera;
    [SerializeField] private int activePriority = 20;
    [SerializeField] private int idlePriority = 5;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && targetCamera != null)
        {
            targetCamera.Priority = activePriority;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && targetCamera != null)
        {
            targetCamera.Priority = idlePriority;
        }
    }
}
