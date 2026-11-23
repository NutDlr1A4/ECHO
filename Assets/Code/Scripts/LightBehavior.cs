using System;
using System.Resources;
using UnityEngine;

public class LightInteraction : MonoBehaviour, IHackable
{
    [SerializeField] private bool is_on = true;
    private EchoNode my_node;
    private GameObject my_light;

    private void SetState(bool state) {
        is_on = state;
        UpdateVisuals();
    }

    public void Toggle() {
        SetState(!is_on);
    }

    public void SwitchOn() {
        SetState(true);
    }

    public void SwitchOff() {
        SetState(false);
    }

    public bool IsOn() {
        return is_on;
    }

    private void UpdateVisuals() {
        my_light.SetActive(is_on);

        EchoNode node_component = my_node.gameObject.GetComponent<EchoNode>();
        if (is_on) {
            node_component.SetState(NodeState.NEUTRAL_ACTIVE);
        }
        else {
            node_component.SetState(NodeState.NEUTRAL_INACTIVE);
        }
    }

    public bool OnHack() {
        Toggle();
        
        return true;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        my_light = gameObject.transform.Find("Light_2D").gameObject;
        my_node = gameObject.transform.Find("Node").GetComponent<EchoNode>();

        SetState(is_on);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
