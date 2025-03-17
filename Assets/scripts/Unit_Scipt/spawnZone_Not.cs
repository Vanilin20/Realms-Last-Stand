using UnityEngine;

public class spawnZone_Not : MonoBehaviour
{
    private SpawnZone spawnZone;

    public void SetSpawnZone(SpawnZone zone)
    {
        spawnZone = zone;
    }

    private void OnDestroy()
    {
        if (spawnZone != null)
        {
            spawnZone.ClearUnit(); // Звільняємо клітинку, коли юніт зник
        }
    }
}
