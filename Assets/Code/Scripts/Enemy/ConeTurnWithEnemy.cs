using UnityEngine;


public class ConeTurnWithEnemy : MonoBehaviour
{
    public Transform detectionCone;

    private void FaceDirection(float direction)
    {
        if (direction > 0)
        {
            detectionCone.localScale = new Vector3(1, 1, 1);
        }
        else if (direction < 0)
        {
            detectionCone.localScale = new Vector3(-1, 1, 1);
        }
    }
}
