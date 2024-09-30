using UnityEngine;

public class DestroyInOneHit : MonoBehaviour, IDamageable
{
    public void DealDamage(int damage,Vector3 v3)
    {
        Destroy(gameObject); // Destroy the object (one-shot kill)
    }
}