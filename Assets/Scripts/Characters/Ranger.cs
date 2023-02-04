using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranger : Character
{
    private void Awake()
    {
        attackRange = 10.0f;
        attackDamage = 2.0f;
        maxHp = 50.0f;
        hp = maxHp;
        walkSpeed = 15.0f;
        attackRatePerSec = 2.0f;
    }
}
