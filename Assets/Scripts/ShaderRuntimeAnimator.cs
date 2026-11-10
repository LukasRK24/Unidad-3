using UnityEngine;

// Anima en ejecucion el parametro de disolucion (_DissolveAmount) de SG_Dissolve
// para apreciar en vivo el recorte Voronoi y el borde emisivo HDR en el pedestal.
public class ShaderRuntimeAnimator : MonoBehaviour
{
    [SerializeField] private string propertyName = "_DissolveAmount";
    [SerializeField] private float cycleSpeed = 0.45f;
    [SerializeField] private float minValue = 0.12f;
    [SerializeField] private float maxValue = 0.78f;

    private Material runtimeMaterial;
    private int propertyId;

    private void Awake()
    {
        Renderer r = GetComponent<Renderer>();
        if (r != null)
        {
            runtimeMaterial = r.material;
        }
        propertyId = Shader.PropertyToID(propertyName);
    }

    private void Update()
    {
        if (runtimeMaterial == null) return;

        float t = Mathf.PingPong(Time.time * cycleSpeed, 1f);
        float val = Mathf.Lerp(minValue, maxValue, Mathf.SmoothStep(0f, 1f, t));
        runtimeMaterial.SetFloat(propertyId, val);
    }

    private void OnDestroy()
    {
        if (runtimeMaterial != null)
        {
            Destroy(runtimeMaterial);
        }
    }
}
