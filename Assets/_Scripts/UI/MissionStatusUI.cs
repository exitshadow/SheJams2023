using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Yarn.Unity;

public class MissionStatusUI : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool showMissionStatusAtStart;

    [Header("References")]
    [SerializeField] private TextMeshProUGUI missionPromptTMP;
    [SerializeField] private TextMeshProUGUI secondaryMissionTMP;

    [YarnCommand("change_mission_prompt")]
    public void ChangeMissionPrompt(string prompt)
    {
        missionPromptTMP.text = prompt;
    }

    [YarnCommand("change_secondary_mission_prompt")]
    public void ChangeSecondaryMissionPrompt(string prompt)
    {
        secondaryMissionTMP.text = prompt;
    }

    public string MissionPrompt { get { return missionPromptTMP.text; }}
    public string SecondaryMissionPrompt { get { return secondaryMissionTMP.text; }}
}
