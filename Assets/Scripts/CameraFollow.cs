using UnityEngine;

public class CámaraArcade : MonoBehaviour
{
    public Transform objetivo; // Tu coche
    public Vector3 distancia = new Vector3(0, 20f, -10f);
    
    [Range(0, 1)] public float suavizado = 0.125f; // Para un toque sutil de fluidez

    void FixedUpdate()
    {
        if (objetivo == null) return;

        // Calculamos la posición deseada de forma rígida
        Vector3 posicionDeseada = objetivo.position + distancia;
        
        // Usamos MoveTowards o Lerp en FixedUpdate para sincronizar frames de física
        transform.position = Vector3.Lerp(transform.position, posicionDeseada, suavizado);
    }

    void LateUpdate()
    {
        if (objetivo == null) return;
        // La rotación siempre mira al coche en el frame de renderizado
        transform.LookAt(objetivo.position);
    }
}