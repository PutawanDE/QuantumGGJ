using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranger : Character
{
    [SerializeField] private GameObject arrow;
    [SerializeField] private float arrowSpeed;

    private void Awake()
    {
        attackRange = 10.0f;
        attackDamage = 2.0f;
        maxHp = 50.0f;
        hp = maxHp;
        walkSpeed = 15.0f;
        attackRatePerSec = 2.0f;
    }

    protected override void Attack()
    {
        Arrow spawnedArrow = Instantiate(arrow, transform.position, Quaternion.identity).GetComponent<Arrow>();
        Vector2 direction = facingLeft ? Vector2.left : Vector2.right;
        spawnedArrow.Initialize(this, attackDamage, arrowSpeed, direction);
    }
}
