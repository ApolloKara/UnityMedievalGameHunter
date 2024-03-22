using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAddon : MonoBehaviour
{
    private Rigidbody rb;

    public int damage;

    private bool targetHit;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Stick to the first target
        if(targetHit)
            return;
        else
            targetHit = true;

        //Enemy Hit Check
        if(collision.gameObject.GetComponent<EnemyHealth>() != null)
        {
            EnemyHealth enemy = collision.gameObject.GetComponent<EnemyHealth>();

            enemy.TakeDamage(damage);

            Destroy(gameObject);
        }

        //Make projectile stick
        rb.isKinematic = true;

        //Projectile Moves With Target
        transform.SetParent(collision.transform);
    }
}
