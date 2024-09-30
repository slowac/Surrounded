using System.Collections;
using UnityEngine;

public abstract class Gun : MonoBehaviour
{
    public string gunName; // Weapon name, for players to identify and select in the game
    public GameObject gunPickup;

    [Header("Stats")]
    public AmmunitionTypes ammunitionType;
    public int minmumDamage; // Minimum damage value of the weapon, used to calculate the actual damage within the random damage range
    public int maxmumDamage; // Maximum damage value of the weapon, used to calculate the actual damage within the random damage range
    public float maxmumRange; // Maximum range of the weapon, determines the farthest distance the player can shoot
    public float recoilAmount = 1.0f; // Weapon recoil
    public float recoilRecoveryFactor = 1.0f; // Recoil recovery factor, used to adjust the ratio of recovery speed to recoil
    public AudioClip shootAudioClip;
    public float fireRate;

    [Header("Bullet")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] public Transform firePoint;
    public float bulletSpeed = 20f;

    protected float timeOfLastShot;

    private Transform cameraTransform;
    private PlayerCameraController playerCameraController;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
        playerCameraController = cameraTransform.GetComponent<PlayerCameraController>();
    }

    private Vector3 GetAimDirection()
    {
        RaycastHit hit;
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, Mathf.Infinity))
        {
            return (hit.point - firePoint.position).normalized;
        }
        else
        {
            return cameraTransform.forward;
        }
    }

    protected void Fire()
    {
        if (AmmunitionManager.instance.ConsumeAmmunition(ammunitionType))// If there is enough ammunition
        {
            // Play shooting sound effect
            AudioSource.PlayClipAtPoint(shootAudioClip, transform.position);
            Vector3 aimDirection = GetAimDirection();
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.LookRotation(aimDirection));

            BulletController bulletController = bullet.GetComponent<BulletController>();
            bulletController.minimumDamage = minmumDamage;
            bulletController.maximumDamage = maxmumDamage;
            bulletController.maximumRange = maxmumRange;
            bulletController.speed = bulletSpeed;
            bulletController.gun = this; // Set the gun script reference of the bullet

            Destroy(bullet, 10f); // Destroy the bullet to prevent too many bullets from affecting performance. You can adjust this value as needed.
        }

        // Calculate vertical and horizontal recoil
        float verticalRecoil = Random.Range(recoilAmount * 0.02f, recoilAmount * 0.03f);
        float horizontalRecoil = Random.Range(-recoilAmount * 0.0125f, recoilAmount * 0.0125f);

        // Record the initial position of the camera before recoil
        Vector2 initialLookingPos = playerCameraController.GetCurrentLookingPos();

        // Add recoil offset
        Vector2 recoil = new Vector2(horizontalRecoil, verticalRecoil);
        playerCameraController.ApplyRecoilWithRecovery(recoil, recoil.magnitude * recoilRecoveryFactor, initialLookingPos); // Recoil will be applied in 0.3 seconds
    }
}