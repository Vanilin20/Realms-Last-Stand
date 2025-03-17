using UnityEngine;

public class SpawnZone : MonoBehaviour
{
    private GameObject unit;

    public bool IsOccupied()
    {
        return unit != null;
    }

    public void SetUnit(GameObject newUnit)
    {
        unit = newUnit;
    }

    public void ClearUnit()
    {
        unit = null;
    }

    public Vector3 GetCenter()
    {
        return transform.position;
    }
}
