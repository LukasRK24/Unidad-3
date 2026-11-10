using UnityEngine;

// Panel ligero en pantalla para ver los FPS, controles basicos y avisos de las pistas.
// Se puede ocultar o mostrar en cualquier momento con la tecla H.
public class NarrativeAtmosphereHUD : MonoBehaviour
{
    [SerializeField] private bool showHUD = true;

    private static string lastNarrativeBeat = "Sigue el camino de tierra iluminado hacia el Santuario del Monolito.";
    private static float beatTimer = 9f;

    private float deltaTime = 0.016f;

    public static void NotifyNarrativeBeat(string message)
    {
        lastNarrativeBeat = message;
        beatTimer = 8.5f;
    }

    private void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;

        if (Input.GetKeyDown(KeyCode.H))
        {
            showHUD = !showHUD;
        }

        if (beatTimer > 0f)
        {
            beatTimer -= Time.unscaledDeltaTime;
        }
    }

    private void OnGUI()
    {
        if (!showHUD) return;

        float fps = Mathf.Clamp(1.0f / Mathf.Max(0.0001f, deltaTime), 1f, 240f);

        GUI.color = new Color(0.05f, 0.09f, 0.15f, 0.82f);
        GUI.Box(new Rect(16, 16, 470, 88), "");
        GUI.color = Color.white;

        GUIStyle titleStyle = new GUIStyle(GUI.skin.label)
        {
            fontSize = 14,
            fontStyle = FontStyle.Bold
        };
        titleStyle.normal.textColor = new Color(0.25f, 0.92f, 1.0f);

        GUIStyle bodyStyle = new GUIStyle(GUI.skin.label)
        {
            fontSize = 12
        };
        bodyStyle.normal.textColor = new Color(0.90f, 0.94f, 0.98f);

        GUI.Label(new Rect(28, 22, 450, 22), "HITO 3: LA ATMOSFERA NARRATIVA (URP 3D)", titleStyle);
        GUI.Label(new Rect(28, 44, 450, 20), $"Rendimiento: {fps:0.0} FPS | Static Batching y LOD activos", bodyStyle);
        GUI.Label(new Rect(28, 64, 450, 20), "Controles: [WASD] Mover | [Mouse] Camara | [Espacio] Saltar | [R] Inicio | [H] HUD", bodyStyle);

        if (Cursor.lockState != CursorLockMode.Locked)
        {
            GUI.color = new Color(0.06f, 0.11f, 0.18f, 0.88f);
            GUI.Box(new Rect(Screen.width * 0.5f - 235, 16, 470, 32), "");
            GUI.color = Color.white;

            GUIStyle hintStyle = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 12,
                fontStyle = FontStyle.Bold
            };
            hintStyle.normal.textColor = new Color(0.45f, 0.88f, 1.0f);
            GUI.Label(new Rect(Screen.width * 0.5f - 230, 18, 460, 28), "Haz clic en el juego para mover la camara con el mouse (Esc para soltar)", hintStyle);
        }

        if (beatTimer > 0f && !string.IsNullOrEmpty(lastNarrativeBeat))
        {
            float alpha = Mathf.Clamp01(beatTimer / 1.2f);
            GUI.color = new Color(0.06f, 0.11f, 0.18f, 0.85f * alpha);
            GUI.Box(new Rect(Screen.width * 0.5f - 320, Screen.height - 68, 640, 44), "");
            GUI.color = Color.white;

            GUIStyle subStyle = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 13,
                fontStyle = FontStyle.Italic
            };
            subStyle.normal.textColor = new Color(1.0f, 0.88f, 0.55f, alpha);
            GUI.Label(new Rect(Screen.width * 0.5f - 310, Screen.height - 64, 620, 36), lastNarrativeBeat, subStyle);
        }
    }
}
