using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class autoDoor : MonoBehaviour
{
    public LayerMask whatIsPlayerOrEnemy;
    public Animator doorAnim;
    public Vector3 checkRadius;

    private Collider[] hitColliders;

    void Update()
    {
        hitColliders = Physics.OverlapBox(transform.position, checkRadius, Quaternion.identity, whatIsPlayerOrEnemy);
        if (hitColliders.Length > 0)
        {
            doorAnim.SetBool("character_nearby", true);
        }
        else
        {
            doorAnim.SetBool("character_nearby", false);
        }
    }
    // void OnTriggerEnter(Collider other) 
    // {
    //     if(other.CompareTag("Player") | other.CompareTag("Enemy"))
    //     {
    //         doorAnim.SetBool("character_nearby", true);
    //     }
    // }

    // void OnTriggerExit(Collider other) 
    // {
    //     if(other.CompareTag("Player") | other.CompareTag("Enemy"))
    //     {
    //         doorAnim.SetBool("character_nearby", false);

    //     }
    // }
}
