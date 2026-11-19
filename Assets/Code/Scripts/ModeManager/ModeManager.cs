using UnityEditor.Rendering.Universal;
using UnityEngine;

public class ModeManager : MonoBehaviour
{
    [SerializeField] private GameObject smooth_camera;
    [SerializeField] private GameObject Ghost;
    [SerializeField] private GameObject ECHO;

    private bool in_echo_mode = false;

    private EchoNode[] FindAllNodesComponents() {
        return FindObjectsByType<EchoNode>(
            FindObjectsInactive.Include,
            FindObjectsSortMode.None
        );
    }

    public void EnterECHOMode(GameObject start_node) {
        Debug.Log("Entering ECHO mode");

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

    void Start() {
        ExitECHOMode();
    }

    void Update()
    {
        
    }
}
