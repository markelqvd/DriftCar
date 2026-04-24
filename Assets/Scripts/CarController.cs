using UnityEngine;

public class CochePro : MonoBehaviour
{
    [Header("Ajustes de Control")]
    public float fuerzaEmpuje = 25f;
    public float velocidadMax = 30f;
    public float sensibilidadGiro = 20f;

    [Header("Referencias")]
    public RectTransform lineaUI;
    private Rigidbody rb;
    private Camera cam;
    private bool arrastrando = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;

        // Configuración de Rigidbody para evitar el "temblor" desde el objeto
        rb.interpolation = RigidbodyInterpolation.Interpolate; 
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        rb.drag = 1.5f; // Freno natural para que no patine
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) arrastrando = true;
        if (Input.GetMouseButtonUp(0)) 
        {
            arrastrando = false;
            if (lineaUI != null) lineaUI.gameObject.SetActive(false);
        }

        if (arrastrando) ManejarInputVisual();
    }

    void FixedUpdate()
    {
        if (arrastrando) AplicarFisicas();
    }

    void ManejarInputVisual()
    {
        Vector3 cocheEnPantalla = cam.WorldToScreenPoint(transform.position);
        Vector3 vectorArrastre = Input.mousePosition - cocheEnPantalla;

        if (lineaUI != null)
        {
            lineaUI.gameObject.SetActive(true);
            lineaUI.position = cocheEnPantalla;
            float anguloUI = Mathf.Atan2(vectorArrastre.y, vectorArrastre.x) * Mathf.Rad2Deg;
            lineaUI.rotation = Quaternion.Euler(0, 0, anguloUI - 90);
            lineaUI.sizeDelta = new Vector2(lineaUI.sizeDelta.x, vectorArrastre.magnitude);
        }

        // Rotación inmediata pero fluida
        if (vectorArrastre.sqrMagnitude > 1f)
        {
            float anguloCoche = Mathf.Atan2(-vectorArrastre.x, -vectorArrastre.y) * Mathf.Rad2Deg;
            Quaternion rotacionDeseada = Quaternion.Euler(0, anguloCoche, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacionDeseada, Time.deltaTime * sensibilidadGiro);
        }
    }

    void AplicarFisicas()
    {
        Vector3 cocheEnPantalla = cam.WorldToScreenPoint(transform.position);
        Vector3 vectorArrastre = Input.mousePosition - cocheEnPantalla;

        // Empujamos en la dirección donde mira el coche
        rb.AddForce(transform.forward * (vectorArrastre.magnitude * fuerzaEmpuje * Time.fixedDeltaTime), ForceMode.VelocityChange);

        // Capar velocidad
        if (rb.velocity.magnitude > velocidadMax)
        {
            rb.velocity = rb.velocity.normalized * velocidadMax;
        }
    }
}