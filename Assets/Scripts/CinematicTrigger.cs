using System.Collections;
using UnityEngine;
using Unity.Cinemachine;

/// <summary>
/// Controla la activación de la cámara de revelación (VCam_NarrativeReveal) cuando el jugador
/// entra al trigger del santuario, mostrando la pista en pantalla y regresando luego a la cámara de exploración.
/// </summary>
public class CinematicTrigger : MonoBehaviour
{
    [Header("Cámara Virtual Cinemachine")]
    [SerializeField] private CinemachineCamera vcam;
    [SerializeField] private int activePriority = 20;
    [SerializeField] private int idlePriority = 5;

    [Header("Configuración del Evento Narrativo")]
    [SerializeField] private bool triggerOnce = false;
    [SerializeField] private float holdDuration = 5.5f;
    [SerializeField] private AudioSource revealAudioStinger;
    [SerializeField] private string narrativeDiscoveryMessage =
        "Pista descubierta: La expedición activó el Monolito Rúnico antes de evacuar el campamento.";

    private bool played;
    private Coroutine returnRoutine;

    public bool HasPlayed => played;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (triggerOnce && played) return;

        played = true;

        if (vcam != null)
        {
            vcam.Priority = activePriority;
        }

        if (revealAudioStinger != null && !revealAudioStinger.isPlaying)
        {
            revealAudioStinger.Play();
        }

        NarrativeAtmosphereHUD.NotifyNarrativeBeat(narrativeDiscoveryMessage);

        if (holdDuration > 0f && gameObject.activeInHierarchy)
        {
            if (returnRoutine != null) StopCoroutine(returnRoutine);
            returnRoutine = StartCoroutine(ReturnToExploreAfterDelay(holdDuration));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (vcam != null)
        {
            vcam.Priority = idlePriority;
        }
    }

    private IEnumerator ReturnToExploreAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (vcam != null)
        {
            vcam.Priority = idlePriority;
        }
    }
}
