using UnityEngine;

// Controlador en tercera persona (Semana 9 - Actividades 3 y 4).
// Usa CharacterController con gravedad ajustada (-20) y rotacion de camara con el mouse.
[RequireComponent(typeof(CharacterController))]
public class PlayerController3D : MonoBehaviour
{
    [Header("Movimiento y Salto")]
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private float gravity = -20f;

    [Header("Camara con Mouse")]
    [SerializeField] private Transform cameraPivot;
    [SerializeField] private float mouseSensitivity = 2.2f;
    [SerializeField] private float minPitch = -22f;
    [SerializeField] private float maxPitch = 35f;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private float pitch;
    private Vector3 spawnPosition;
    private Quaternion spawnRotation;

    public bool IsGrounded => isGrounded;
    public Vector3 CurrentVelocity => velocity;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        spawnPosition = transform.position;
        spawnRotation = transform.rotation;

        if (cameraPivot != null)
        {
            pitch = cameraPivot.localEulerAngles.x;
            if (pitch > 180f) pitch -= 360f;
        }

#if !UNITY_WEBGL && !UNITY_EDITOR
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
#endif
    }

    private void Update()
    {
        // Bloquear el cursor al hacer clic en la pantalla (necesario en WebGL para capturar el mouse)
        if ((Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) && Cursor.lockState != CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        if (Input.GetKeyDown(KeyCode.Escape) && Cursor.lockState == CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        // Reinicio rapido al punto inicial del sendero con la tecla R
        if (Input.GetKeyDown(KeyCode.R))
        {
            controller.enabled = false;
            transform.SetPositionAndRotation(spawnPosition, spawnRotation);
            velocity = Vector3.zero;
            controller.enabled = true;
            return;
        }

        // 1. Verificar contacto con el suelo
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0f)
        {
            velocity.y = -2f; // Mantiene al personaje pegado al terreno en pendientes
        }

        // 2. Rotacion de camara y personaje desplazando el mouse
        if (Cursor.lockState == CursorLockMode.Locked || Input.GetMouseButton(0) || Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

            if (Mathf.Abs(mouseX) > 0.0001f)
            {
                transform.Rotate(Vector3.up * mouseX);
            }

            if (cameraPivot != null && Mathf.Abs(mouseY) > 0.0001f)
            {
                pitch = Mathf.Clamp(pitch - mouseY, minPitch, maxPitch);
                cameraPivot.localRotation = Quaternion.Euler(pitch, 0f, 0f);
            }
        }

        // 3. Movimiento en coordenadas locales (WASD / Flechas) independiente de los FPS
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        if (move.sqrMagnitude > 1f)
        {
            move.Normalize();
        }
        controller.Move(move * moveSpeed * Time.deltaTime);

        // 4. Salto con v0 = sqrt(h * -2 * g) y acumulacion de gravedad
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
