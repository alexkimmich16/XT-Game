using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Custom.Cus;
public enum AttackType
{
    Low = 0, 
    High = 1
}
public class HitBoxControl : MonoBehaviour
{
    public AttackType type;
    public int Damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("TriggerHitbox1");
        if (collision.transform.tag == "Player")
        {
            Debug.Log("TriggerHitbox2");
            DamageInfo DamageStat = new DamageInfo();
            DamageStat.HitType = type;
            DamageStat.Damage = Damage;
            collision.transform.GetComponent<CharacterController>().TakeDamage(DamageStat);
        }
    }
}
