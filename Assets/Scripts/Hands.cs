using UnityEngine;

public class Hands : MonoBehaviour
{
    public Collider leftHandCollider; // Collider for the left hand
    public Collider rightHandCollider; // Collider for the right hand

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider that was hit is a child of the enemy
        if (other.CompareTag("Enemy") || other.transform.parent.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponentInParent<Enemy>(); // Get the Enemy component from the parent
            if (enemy != null)
            {
                int damage = 0;
                HitType hitType = HitType.Body; // Default to body hit

                // Check if the left hand collider is enabled
                if (leftHandCollider.enabled)
                {
                    if (other.gameObject.name.Contains("Head"))
                    {
                        damage = enemy.headDamage;
                        hitType = HitType.Head;
                        Debug.Log("Hit Head!");
                    }
                    else if (other.gameObject.name.Contains("Body"))
                    {
                        damage = enemy.bodyDamage;
                        hitType = HitType.Body;
                        Debug.Log("Hit Body!");
                    }
                    else if (other.gameObject.name.Contains("Legs"))
                    {
                        damage = enemy.legDamage;
                        hitType = HitType.Legs;
                        Debug.Log("Hit Legs!");
                    }
                }

                // Check if the right hand collider is enabled
                if (rightHandCollider.enabled)
                {
                    if (other.gameObject.name.Contains("Head"))
                    {
                        damage = enemy.headDamage;
                        hitType = HitType.Head;
                        Debug.Log("Hit Head with Right Hand!");
                    }
                    else if (other.gameObject.name.Contains("Body"))
                    {
                        damage = enemy.bodyDamage;
                        hitType = HitType.Body;
                        Debug.Log("Hit Body with Right Hand!");
                    }
                    else if (other.gameObject.name.Contains("Legs"))
                    {
                        damage = enemy.legDamage;
                        hitType = HitType.Legs;
                        Debug.Log("Hit Legs with Right Hand!");
                    }
                }

                // If damage is greater than zero, apply it to the enemy
                if (damage > 0)
                {
                    enemy.TakeDamage(damage, hitType);
                    Debug.Log($"Dealt {damage} damage to {hitType}!");
                }
                else
                {
                    Debug.Log("No damage dealt.");
                }
            }
            else
            {
                Debug.Log("No Enemy component found on the collided object.");
            }
        }
    }
}