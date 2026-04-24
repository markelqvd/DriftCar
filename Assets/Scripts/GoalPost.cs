using UnityEngine;

public class GoalPost : MonoBehaviour
{
    public LevelData currentLevelData; // Arrastra aquí el ScriptableObject del nivel
    private float timer;
    private bool levelFinished = false;

    void Update()
    {
        if (!levelFinished)
        {
            timer += Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !levelFinished)
        {
            levelFinished = true;
            CheckStars();
        }
    }

    void CheckStars()
    {
        Debug.Log("¡Meta alcanzada!");
        Debug.Log("Tiempo total: " + timer);

        // Lógica de estrellas
        bool star1 = true; // Por llegar
        bool star2 = timer <= currentLevelData.goldTimeStar2;
        bool star3 = false; 

        // La estrella 3 depende de la fase activa
        // Aquí podrías preguntar al LevelManager en qué fase estamos
        
        Debug.Log($"Estrellas: S1:{star1} | S2:{star2} | S3:{star3}");
    }
}