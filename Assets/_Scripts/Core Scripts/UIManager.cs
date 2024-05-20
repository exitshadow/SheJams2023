using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Yarn.Unity;
using UnityEngine.UI.ProceduralImage;


/// <summary>
/// Dispatches all the texts in the game into their right places in the UI and manages opening and closing UI groups.
/// </summary>
///
public class UIManager : MonoBehaviour
{   
    #region member variables
    [Header("Settings")]
    [SerializeField] private bool showMissionStatusAtStart;
    [SerializeField] private bool showInteractionPromptAtStart;
    [SerializeField] private bool showControlsPromptAtStart;
    [SerializeField] private bool showPhoneAtStart;
    [SerializeField] private bool showDialogueBoxAtStart;
    [SerializeField] private bool debug = false;


    #region exposed references
    [Header("General References")]
    [SerializeField] private Canvas uiCanvas;

    [Header("Controls prompt references")]
    [SerializeField] private RectTransform showControlsPromptGroup;
    [SerializeField] private RectTransform controlsHelpGroup;
    [HideInInspector] public bool isControlHelpDisplayed;

    [Header("Mission Indicator Reference")]
    [SerializeField] private TextMeshProUGUI missionPromptTMP;
    [SerializeField] private TextMeshProUGUI secondaryMissionTMP;


    #endregion
    #endregion


    #region show controls prompt
    public void OpenControlsPrompt()
    {
        showControlsPromptGroup.gameObject.SetActive(true);
        isControlHelpDisplayed = true;
    }

    public void CloseControlsPromt()
    {
        showControlsPromptGroup.gameObject.SetActive(false);
        isControlHelpDisplayed = false;
    }

    public void OpenControlsHelp()
    {
        controlsHelpGroup.gameObject.SetActive(true);
        isControlHelpDisplayed = true;
    }

    public void CloseControlsHelp()
    {
        controlsHelpGroup.gameObject.SetActive(false);
        isControlHelpDisplayed = false;
    }
    #endregion

    #region mission prompt
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

    #endregion


    void Start()
    {

        //ClosePhoneUI();
        CloseControlsHelp();
        if (showControlsPromptAtStart) OpenControlsPrompt();
    }

    #region UI utils
    public static Vector2 ScaleToScreen(    Vector2 coordinate,
                                            float boundRight,
                                            float boundTop,
                                            float marginX,
                                            float marginY)
    {
        float x = coordinate.x / Camera.main.pixelWidth / (boundRight + marginX);
        float y = coordinate.y / Camera.main.pixelHeight / (boundTop + marginY);

        return new Vector2(x, y);
    }

    public static Vector2 WorldToCanvasPoint(   Vector3 position,
                                                float boundRight,
                                                float boundTop,
                                                float marginX,
                                                float marginY)
    {
        Vector2 coordinate = Camera.main.WorldToScreenPoint(position);

        float x = coordinate.x / Camera.main.pixelWidth / (boundRight + marginX);
        float y = coordinate.y / Camera.main.pixelHeight / (boundTop + marginY);

        return new Vector2(x, y);
    }
    #endregion
}
