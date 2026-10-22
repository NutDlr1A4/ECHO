using Unity.Mathematics;
using UnityEngine;

public class Camera_Movement : MonoBehaviour
{
    [SerializeField] private GameObject object_followed;
    [SerializeField] private float speed = 0.97f;
    [SerializeField] private float z_distance = 10.0f;

    public void SetFollow(GameObject new_object) {
        object_followed = new_object;
    }

    private void Set2DPos(Vector3 pos) {
        gameObject.transform.position = new Vector3(
            pos.x,
            pos.y,
            -z_distance
        );
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        Set2DPos(object_followed.transform.position);
    }

    // Update is called once per frame
    void Update() {
        
    }

    // Uses LateUpdate() to avoid jittering
    // TODO: It's still kinda jittery? This might just be regular lag, but should look into it.
    void LateUpdate() {
        Vector3 delta = object_followed.transform.position - gameObject.transform.position;

        // OKAY OKAY I know this looks janky, but when multiplying each frame you can't just multiply delta time.
        // You gotta multiply it by some fuckass exponent instead
        Vector3 new_pos = gameObject.transform.position + delta * math.pow(speed, 1.0f / Time.deltaTime);

        Set2DPos(new_pos);
    }
}
