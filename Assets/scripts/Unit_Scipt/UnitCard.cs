using UnityEngine;

[CreateAssetMenu(fileName = "NewUnit", menuName = "Unit Card")]
public class UnitCard : ScriptableObject
{
    public string unitName;
    public GameObject unitPrefab; // Префаб юніта
    public int cost;
    public float speed;
    public int health;
    public int damage;
}
