using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranger : Charactor
{
    private void Awake()
    {
        attackRange = 10.0f;
        attackDamage = 2.0f;
        maxHp = 50.0f;
        hp = maxHp;
        speed = 15.0f;
        attackSpeed = 2.0f;
    }
}
