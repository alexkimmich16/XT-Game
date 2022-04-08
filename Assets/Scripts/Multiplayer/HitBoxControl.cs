using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum AttackType
{
    Low = 0, 
    High = 1
}
public class HitBoxControl : MonoBehaviour
{
    public AttackType type;
    public int Damage;
}
