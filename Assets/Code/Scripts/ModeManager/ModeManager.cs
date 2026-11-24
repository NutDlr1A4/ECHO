using System.Runtime.InteropServices;
using UnityEditor.Rendering.Universal;
using UnityEngine;

public class ModeManager : MonoBehaviour
{
    [SerializeField] private GameObject smooth_camera;
    [SerializeField] private GameObject Ghost;
    [SerializeField] private GameObject ECHO;

    [Header("Audio")]
    [SerializeField] private AudioSource echo_move_audio_source;
    [SerializeField] private AudioSource echo_hack_audio_source;

    [SerializeField] private AudioClip echo_move_sound;
    [SerializeField, Range(0f, 1f)] private float echo_move_volume = 0.5f;

    [SerializeField] private AudioClip echo_hack_success_sound;
    [SerializeField, Range(0f, 1f)] private float echo_success_volume = 0.5f;
    [SerializeField] private AudioClip echo_hack_fail_sound;
    [SerializeField, Range(0f, 1f)] private float echo_fail_volume = 0.5f;

    private bool in_echo_mode = false;
    private GameObject used_terminal_node;

    [SerializeField] private bool is_enabled = true;

    // Plays sounds here so they don't get interupted if exiting ECHO
    // THis should maybe be moved into its own "sound manager", but I'm lazy
    public void PlaySound(string sound_name) {
        if (sound_name == "echo_move") {
            echo_move_audio_source.pitch = Random.Range(0.95f, 1.05f);
            echo_move_audio_source.PlayOneShot(echo_move_sound, echo_move_volume);
        }
        else if (sound_name == "echo_hack_success") {
            echo_hack_audio_source.PlayOneShot(echo_hack_success_sound, echo_success_volume);
        }
        else if (sound_name == "echo_hack_fail") {
            echo_hack_audio_source.PlayOneShot(echo_hack_fail_sound, echo_fail_volume);
        }
    }

    private EchoNode[] FindAllNodesComponents() {
        return FindObjectsByType<EchoNode>(
            FindObjectsInactive.Include,
            FindObjectsSortMode.None
        );
    }

    public void EnterECHOMode(GameObject start_node) {
        if (!is_enabled) return;

        Debug.Log("Entering ECHO mode");

        used_terminal_node = start_node;
        EchoNode used_terminal_node_component = used_terminal_node.GetComponent<EchoNode>();
        used_terminal_node_component.SetState(NodeState.CONTROLED);
        used_terminal_node_component.SpawnEffectOnNode();
        
        gameObject.transform.Find("Overlay").gameObject.SetActive(true);
        gameObject.transform.Find("EchoNodeManager").gameObject.SetActive(true);

        EchoNode[] all_nodes = FindAllNodesComponents();
        foreach(EchoNode node in all_nodes) {
            node.gameObject.SetActive(true);
        }

        Ghost.GetComponent<GhostMovement>().EnterEchoMode();
        
        ECHO.GetComponent<ECHOBehaviour>().SetNewNode(start_node);
        ECHO.SetActive(true);

        smooth_camera.transform.position = Ghost.transform.position;
        smooth_camera.GetComponent<Camera_Movement>().SetToFollow(ECHO);

        in_echo_mode = true;
    }

    public void ExitECHOMode() {
        Debug.Log("Exiting ECHO Mode...");

        if (used_terminal_node != null) used_terminal_node.GetComponent<EchoNode>().SetState(NodeState.NEUTRAL_ACTIVE);

        gameObject.transform.Find("Overlay").gameObject.SetActive(false);
        gameObject.transform.Find("EchoNodeManager").gameObject.SetActive(false);

        EchoNode[] all_nodes = FindAllNodesComponents();
        foreach(EchoNode node in all_nodes) {
            node.gameObject.SetActive(false);
        }

        Ghost.GetComponent<GhostMovement>().ExitEchoMode();

        ECHO.SetActive(false);

        smooth_camera.GetComponent<Camera_Movement>().SetToFollow(Ghost);

        in_echo_mode = false;
    }

    public bool IsInECHOMode() {
        return in_echo_mode;
    }

    public void Disable() {
        is_enabled = false;
    }

    void Start() {
        ExitECHOMode();
    }

    void Update()
    {
        
    }
}
