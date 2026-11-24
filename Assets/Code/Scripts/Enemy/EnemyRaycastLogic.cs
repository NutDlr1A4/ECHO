using UnityEngine;

public class EnemyRaycastLogic : MonoBehaviour
{
    [SerializeField] private LayerMask detectionLayers;

    public bool CanDetect(
        Vector2 startPosition,
        Collider2D target)
    {
        if (target == null)
        {
            return false;
        }

        RaycastHit2D hit = Physics2D.Linecast(
            startPosition,
            target.bounds.center,
            detectionLayers
        );

        if (hit.collider == null)
        {
            return false;
        }

        return hit.collider == target ||
               hit.collider.GetComponentInParent<GhostMovement>() != null;
    }
}
