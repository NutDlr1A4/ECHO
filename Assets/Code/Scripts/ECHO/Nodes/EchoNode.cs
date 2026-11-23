using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public enum NodeState {
    CONTROLED,
    NEUTRAL_ACTIVE,
    NEUTRAL_INACTIVE,
    HOSTILE
}

public class EchoNode : MonoBehaviour
{
    public Dictionary<ConnectionPoint, GameObject> node_dict = new();
    public GameObject connection;

    [SerializeField] private GameObject hack_effect_prefab;

    [SerializeField] private NodeState state = NodeState.NEUTRAL_INACTIVE;

    private struct VisualsInfo {
        public Color node_color;
        public Color light_color;
        public float light_intensity;
    };

    // This could be a serialized field and be configurable from there but I'm lazy lol sue me
    private static readonly Dictionary<NodeState, VisualsInfo> visuals_info_dict = new(){
        { NodeState.CONTROLED, new VisualsInfo{ 
            node_color = new Color(0f, 1f, 1f), 
            light_color = new Color(0f, 0f, 1f), 
            light_intensity = 15.0f } 
        },
        { NodeState.NEUTRAL_ACTIVE, new VisualsInfo{ 
            node_color = new Color(1f, 1f, 1f), 
            light_color = new Color(1f, 1f, 1f), 
            light_intensity = 1.0f } 
        },
        { NodeState.NEUTRAL_INACTIVE, new VisualsInfo{ 
            node_color = new Color(120f/255f, 120f/255f, 120f/255f), 
            light_color = new Color(1f, 1f, 1f), 
            light_intensity = 0.05f } 
        },
        { NodeState.HOSTILE, new VisualsInfo{ 
            node_color = new Color(1f, 0f, 0f), 
            light_color = new Color(1f, 0f, 0f), 
            light_intensity = 10.0f }
        }
    };

    public void SpawnEffectOnNode(Color? effect_color = null) {
        GameObject effect = Instantiate(hack_effect_prefab);

        if (effect_color != null) effect.transform.Find("Light_2D").GetComponent<Light2D>().color = effect_color.Value;
        effect.transform.position = gameObject.transform.position;
    }

    public bool DoHack() {
        var hackable = GetComponentInParent<IHackable>();

        if (hackable != null) {
            if (hackable.OnHack()) {
                SpawnEffectOnNode();
                return true;
            }
        }

        return false;
    }

    public void SetState(NodeState new_state) {
        state = new_state;
        UpdateStateVisuals();
    }

    private void UpdateStateVisuals() {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        Light2D node_light = gameObject.transform.Find("Light_2D").GetComponent<Light2D>();

        VisualsInfo vinfo = visuals_info_dict[state];

        renderer.color = vinfo.node_color;
        node_light.color = vinfo.light_color;
        node_light.intensity = vinfo.light_intensity;
    }

    private void OnEnable()
    {
        UpdateStateVisuals();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }
}