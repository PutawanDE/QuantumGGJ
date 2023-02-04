using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    public static int deathCount = 0;

    private void Awake()
    {
        walkSpeed = 10.0f * (1 + deathCount * 0.1f);
        attackRatePerSec = 1.0f * (1 + deathCount * 0.1f);
        attackDamage = 1.0f * (1 + deathCount * 0.1f);
        hp = 60.0f * (1 + deathCount * 0.1f);
        maxHp = 60.0f * (1 + deathCount * 0.1f);

    }

    public void attacked(float damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            die();
        }
    }


    public float attack()
    {
        return attackDamage;
    }
    public void die()
    {
        deathCount++;
        Destroy(gameObject);
    }

        // Start is called before the first frame update
    void Start()
    {

    }

    private void Update() {
        
    }

    private void FixedUpdate() {
        
    }
}
