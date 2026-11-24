using UnityEngine;
using TMPro;

public class ObjectiveUI : MonoBehaviour
{
    public VictoryZone victoryZone;

    public ModeManager modeManager;

    private GameObject Objective;
    private GameObject Leave;
    private GameObject EchoInfo;

    private TMP_Text ObjectiveTexst;
    private TMP_Text LeaveTexst;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Objective = transform.Find("Objective").gameObject;
        Objective.SetActive(true);

        Leave = transform.Find("LeaveTheArea").gameObject;
        Leave.SetActive(false);

        EchoInfo = transform.Find("EchoInfo").gameObject;
        EchoInfo.SetActive(false);

        ObjectiveTexst = GetComponent<TMP_Text>();
        LeaveTexst = GetComponent<TMP_Text>();

    }

    // Update is called once per frame
    void Update()
    {
        if (victoryZone.objectiveCompleted)
        {
            Objective.GetComponent<TMP_Text>().fontStyle = FontStyles.Strikethrough;
            Objective.GetComponent<TMP_Text>().color = Color.deepSkyBlue;

            Leave.SetActive(true);

        }

        if (modeManager.IsInECHOMode())
        {

            EchoInfo.SetActive(true);

        }
        else 
        {
            EchoInfo.SetActive(false);
        }

    }
}
