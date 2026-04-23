using UnityEngine;
using UnityEngine.UI;

public class CarMouseControl : MonoBehaviour
{
    [Header("Ajustes de Movimiento")]
    public float forceMultiplier = 15f;
    public float rotationSpeed = 5f;
    public float jumpForce = 5f;

    [Header("UI (Línea del Canvas)")]
    public RectTransform uiLine; // Arrastra aquí la imagen "DirectionLine"

    private Rigidbody rb;
    private Vector3 startMousePos;
    private Vector2 screenCenter;
    private bool isDragging = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // Congelamos la rotación en X y Z para que no vuelque (efecto coche)
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);

        if (uiLine != null)
        {
            uiLine.gameObject.SetActive(false);
            // Aseguramos que la línea esté físicamente en el centro
            uiLine.position = screenCenter;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //startMousePos = Input.mousePosition;
            isDragging = true;
            if (uiLine != null) uiLine.gameObject.SetActive(true);
        }

        if (Input.GetMouseButton(0) && isDragging)
        {
            Vector3 currentMousePos = Input.mousePosition;
            Vector3 dragVector = currentMousePos - (Vector3)screenCenter;

            // 1. UI: Dibujar la línea en el Canvas
            UpdateUILine(dragVector);

            // 2. Movimiento: Empujar hacia adelante (invertido al arrastre)
            // Si arrastras hacia abajo (y negativo), la fuerza es hacia adelante (Z positivo)
            float forceMagnitude = dragVector.magnitude * forceMultiplier * Time.deltaTime;
            rb.AddRelativeForce(Vector3.forward * forceMagnitude, ForceMode.Acceleration);

            // 3. Giro: Rotar el coche según la dirección del arrastre
            if (dragVector.sqrMagnitude > 100f) // Solo si hay arrastre significativo
            {
                // Invertimos el dragVector para que el coche mire hacia donde "apunta" el tiro
                float angle = Mathf.Atan2(-dragVector.x, -dragVector.y) * Mathf.Rad2Deg;
                Quaternion targetRot = Quaternion.Euler(0, angle, 0);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, Time.deltaTime * rotationSpeed);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            // --- LÓGICA DEL SALTO ---
            /*if (isDragging)
            {
                Jump();
            }*/

            isDragging = false;
            if (uiLine != null) uiLine.gameObject.SetActive(false);
        }
    }

    void Jump()
    {
        // Aplicamos un impulso hacia arriba
        // ForceMode.Impulse usa la masa del objeto, así que se siente muy físico
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    void UpdateUILine(Vector3 drag)
    {
        if (uiLine == null) return;

        uiLine.position = startMousePos;
        // Rotamos la línea para que mire al ratón
        float angle = Mathf.Atan2(drag.y, drag.x) * Mathf.Rad2Deg;
        uiLine.rotation = Quaternion.Euler(0, 0, angle - 90);

        // Escalamos la altura de la imagen según la distancia del arrastre
        uiLine.sizeDelta = new Vector2(uiLine.sizeDelta.x, drag.magnitude);
    }
}