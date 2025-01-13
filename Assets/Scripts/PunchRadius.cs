using UnityEngine;

public class PunchRadius : MonoBehaviour
{
    public float radius = 1.5f; // Radius around the player for punching
    public LayerMask enemyLayer; // Layer on which enemies are present

    private void OnDrawGizmos()
    {
        // Visualize the punch radius in the editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    public bool IsEnemyInRange(Vector3 enemyPosition)
    {
        return Vector3.Distance(transform.position, enemyPosition) <= radius;
    }
}