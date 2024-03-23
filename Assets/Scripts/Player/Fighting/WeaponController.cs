using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("Keybinds")]
    public KeyCode attackKey = KeyCode.Mouse0;

    [Header("Settings")]
    public GameObject sword; //You Can Add other objects like guns
    bool canAttack;
    public float AttackCooldown;
    public bool isAttacking;
 


    private void Start()
    {
        canAttack = true;
        isAttacking = false;
    }

    private void Update()
    {
        AttackCheck();
    }

    void AttackCheck()
    {
        if(Input.GetKeyDown(attackKey) && canAttack)
        {
            Attack();
        }
    }

    void Attack()
    {
        isAttacking = true;
        canAttack = false;
        Animator anim = sword.GetComponent<Animator>();
        anim.SetTrigger("Attack");

        Invoke(nameof(RestAttack), AttackCooldown);
    }

    void RestAttack()
    {
        canAttack = true;
        isAttacking = false;
    }

}
