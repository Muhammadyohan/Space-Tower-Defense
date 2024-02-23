using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class autoDoor : MonoBehaviour
{
    public Animator doorAnim;
    void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Player") | other.CompareTag("Enemy"))
        {
            doorAnim.SetBool("character_nearby", true);
        }
    }

    void OnTriggerExit(Collider other) 
    {
        if(other.CompareTag("Player") | other.CompareTag("Enemy"))
        {
            doorAnim.SetBool("character_nearby", false);

        }
    }
}
