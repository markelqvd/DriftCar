using UnityEngine;

[CreateAssetMenu(fileName = "NewLevel", menuName = "CocheArcade/LevelData")]
public class LevelData : ScriptableObject
{
    public string levelName;
    public int worldIndex;
    
    [Header("Fase 1: Básica")]
    public float completionTimeStar1; // Tiempo máximo sugerido

    [Header("Fase 2: Tiempo")]
    public float goldTimeStar2;

    [Header("Fase 3: Desafío")]
    public bool enableRamp;
    public bool enableObstacles;
    public string customChallengeDescription;
}