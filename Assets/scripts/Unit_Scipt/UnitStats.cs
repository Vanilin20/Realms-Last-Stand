using UnityEngine;

public class UnitStats : MonoBehaviour
{
    public int health;
    public int damage;

    public void Initialize(int hp, int dmg)
    {
        health = hp;
        damage = dmg;
    }
}
