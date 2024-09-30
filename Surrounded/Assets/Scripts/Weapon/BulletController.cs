using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed;
    public int damage; // Set damage as a public variable
    public Gun gun; // Reference to the gun script
    public Vector3 originPosition; // The point where the bullet is fired

    public int minimumDamage; // Minimum damage value passed from the Gun script
    public int maximumDamage; // Maximum damage value passed from the Gun script
    public float maximumRange; // Maximum range passed from the Gun script

    private Rigidbody rb;
    private float flightTime = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;
        originPosition = gun.firePoint.position; // Record the firing point
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        flightTime += Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null)
        {
            float distanceTraveled = flightTime * speed;
            float normalizedDistance = Mathf.Clamp01(distanceTraveled / maximumRange);
            damage = Mathf.RoundToInt(Mathf.Lerp(maximumDamage, minimumDamage, normalizedDistance));
            // Pass collision information to the DealDamage method
            damageable.DealDamage(damage, originPosition);
            Destroy(gameObject);
        }
        Destroy(gameObject);
    }
}