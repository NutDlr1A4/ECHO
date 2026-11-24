using System;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class GhostKAttackBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject mode_man;

    private InputSystem_Actions controls;
    [SerializeField] private float max_kill_distance = 2.0f;
    [SerializeField] private GameObject current_kill_target = null;

    [SerializeField] private bool is_enabled = true;

    private float DistBetweenGameObjectsSqr(GameObject obj1, GameObject obj2) {
        return (obj2.transform.position - obj1.transform.position).sqrMagnitude;
    }

    private void FindNewKillTarget() {
        GameObject[] killables = GameObject.FindGameObjectsWithTag("Killable");
        GameObject new_kill_target = null;

        if (killables.Length > 0) {
            float current_min_distance_sqr = max_kill_distance * max_kill_distance;

            foreach (GameObject obj in killables) {
                float dist_sqr = DistBetweenGameObjectsSqr(gameObject, obj);

                if (dist_sqr < current_min_distance_sqr) {
                    current_min_distance_sqr = dist_sqr;

                    if (dist_sqr <= max_kill_distance * max_kill_distance) {
                        new_kill_target = obj;
                    }
                }
            }
        }

        if (new_kill_target != current_kill_target) {
            current_kill_target = new_kill_target;
        }
    }

    public void Disable() {
        is_enabled = false;
    }

    void Start()
    {
        controls = new InputSystem_Actions();
        controls.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if (!is_enabled) return;
        
        if (mode_man.GetComponent<ModeManager>().IsInECHOMode()) {
            current_kill_target = null;
            return;
        }

        FindNewKillTarget();

        if (controls.Player.Attack.WasPressedThisFrame()) {
            if (current_kill_target != null) {
                current_kill_target.GetComponent<IKillable>()?.OnKill();
            }
        }
    }

    private void OnDestroy()
    {
        controls.Player.Disable();
        controls.UI.Disable();
    }
}
