using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordHit : MonoBehaviour
{
    public WeaponController wp;

    public int damage;

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy" && wp.isAttacking)
        {
            EnemyHealth enemy = other.gameObject.GetComponent<EnemyHealth>();

            enemy.TakeDamage(damage);

            Debug.Log(other.name + "Hit");
        }
    }


  
}
