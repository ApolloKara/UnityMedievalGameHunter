using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwing : MonoBehaviour
{
    [Header("Refrences")]
    public Transform cam;
    public Transform attackPoint;
    public GameObject objectToThrow;

    [Header("Settings")]
    public int totalThrows;
    public float throwCooldown;

    [Header("Throwing")]
    public KeyCode throwKey = KeyCode.Mouse0;
    public float throwForce;
    public float throwUpwardsForce;
    bool readyToThrow;


    // Start is called before the first frame update
    void Start()
    {
        readyToThrow = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(throwKey) && readyToThrow && totalThrows > 0)   
        {
            Throw();
        }
    }

    private void Throw()
    {
        readyToThrow = false;

        //Instantiate object to throw
        GameObject projectile = Instantiate(objectToThrow, attackPoint.position, cam.rotation);

        //Get Rigidbody
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

        //Calculate Direction
        Vector3 forceDirection = cam.transform.forward;

        RaycastHit hit;

        if(Physics.Raycast(cam.position, cam.forward, out hit, 500f))
        {
            forceDirection = (hit.point - attackPoint.position).normalized;
        }

        // Add Force
        Vector3 forceToAdd = forceDirection * throwForce + transform.up * throwUpwardsForce;

        projectileRb.AddForce(forceToAdd, ForceMode.Impulse);

        totalThrows --;

        //Implement Cooldown
        Invoke(nameof(ResetThrow), throwCooldown);

       
    }
    private void ResetThrow()
        {
            readyToThrow = true;
        }

}
