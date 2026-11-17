using System;
using Unity.Mathematics;
using UnityEngine;

public class GhostInteractBehaviour : MonoBehaviour
{
    private InputSystem_Actions controls;
    [SerializeField] private float max_interact_distance = 2.0f;
    [SerializeField] private GameObject current_interact_target = null;

    private float DistBetweenGameObjectsSqr(GameObject obj1, GameObject obj2) {
        return (obj2.transform.position - obj1.transform.position).sqrMagnitude;
    }

    private void FindNewInteractTarget() {
        GameObject[] interactables = GameObject.FindGameObjectsWithTag("Interactable");
        GameObject new_interact_target = null;

        if (interactables.Length > 0) {
            float current_min_distance_sqr = max_interact_distance * max_interact_distance;

            foreach (GameObject obj in interactables) {
                float dist_sqr = DistBetweenGameObjectsSqr(gameObject, obj);

                if (dist_sqr < current_min_distance_sqr) {
                    current_min_distance_sqr = dist_sqr;

                    if (dist_sqr <= max_interact_distance * max_interact_distance) {
                        new_interact_target = obj;
                    }
                }
            }
        }

        if (new_interact_target != current_interact_target) {
            if (current_interact_target != null) {
                current_interact_target.GetComponent<IInteractable>()?.OnInteractTargetExit();
            }
            if (new_interact_target != null) {
                new_interact_target.GetComponent<IInteractable>()?.OnInteractTargetEnter();
            }
            
            current_interact_target = new_interact_target;
        }
    }

    void Start()
    {
        controls = new InputSystem_Actions();
        controls.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        FindNewInteractTarget();

        if (controls.Player.Interact.WasPressedThisFrame()) {
            if (current_interact_target != null) {
                current_interact_target.GetComponent<IInteractable>()?.OnInteract();
            }
        }
    }
}
