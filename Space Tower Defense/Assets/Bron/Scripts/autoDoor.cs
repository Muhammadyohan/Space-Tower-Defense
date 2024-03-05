using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class autoDoor : MonoBehaviour
{
    public LayerMask whatIsPlayerOrEnemy;
    public Animator doorAnim;
    public Vector3 checkRadius;

    [SerializeField] private AudioClip doorOpenSoundClip;
    [SerializeField] private AudioClip doorCloseSoundClip;

    private Collider[] hitColliders;

    void Update()
    {
        hitColliders = Physics.OverlapBox(transform.position, checkRadius, Quaternion.identity, whatIsPlayerOrEnemy);
        if (hitColliders.Length > 0)
        {
            if (!doorAnim.GetBool("character_nearby"))
                doorAnim.SetBool("character_nearby", true);
        }
        else
        {
            if (doorAnim.GetBool("character_nearby"))
                doorAnim.SetBool("character_nearby", false);
        }
    }
    
    public void PlayerDoorOpenSound()
    {
        SoundFXManager.instance.PlayerSoundFXClip(doorOpenSoundClip, transform, 0.025f);
    }

    public void PlayerDoorCloseSound()
    {
        SoundFXManager.instance.PlayerSoundFXClip(doorCloseSoundClip, transform, 0.025f);
    }
}
