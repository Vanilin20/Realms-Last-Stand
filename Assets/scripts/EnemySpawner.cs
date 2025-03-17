using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Префаб ворога
    public Vector2[] spawnPositions; // Масив координат (X, Y)

    void Start()
{
    InvokeRepeating(nameof(SpawnEnemy), 1f, 5f); // Спавн кожні 3 секунди
}

    public void SpawnEnemy()
    {
        if (spawnPositions.Length == 0) return; // Перевірка, чи є позиції

        int randomIndex = Random.Range(0, spawnPositions.Length); // Випадковий індекс
        Vector2 spawnPos2D = spawnPositions[randomIndex]; // Випадкова позиція (X, Y)

        Vector3 spawnPos3D = new Vector3(spawnPos2D.x, spawnPos2D.y, 0f); // Конвертація в 3D

        Instantiate(enemyPrefab, spawnPos3D, Quaternion.identity); // Спавн ворога
    }
}
