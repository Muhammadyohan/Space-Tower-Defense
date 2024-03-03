using Unity.VisualScripting;
using UnityEngine;

public class Recoil : MonoBehaviour
{
    //Rotations
    private Vector3 currentRotation;
    private Vector3 targetRotation;
    private PlayerCam playerCam;

    //Hipfire Recoil
    [Header("Hipfire Recoil")]
    [SerializeField] private float recoilX;
    [SerializeField] private float recoilY;
    [SerializeField] private float recoilZ;

    //Settings
    [Header("Settings")]
    [SerializeField] private float snappiness;
    [SerializeField] private float returnSpeed;

    void Start()
    {
        playerCam = GetComponentInParent<PlayerCam>();
    }

    void Update()
    {
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, returnSpeed * Time.deltaTime);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, snappiness * Time.fixedDeltaTime);
        transform.localRotation = Quaternion.Euler(currentRotation);
    }

    public void RecoilFire()
    {
        targetRotation += new Vector3(recoilX, Random.Range(-recoilY, recoilY), Random.Range(-recoilZ, recoilZ));
    }
}
