using System.Collections;
using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
    public PlayableDirector feedingCatTimeline;
    public PlayableDirector vetAppearsTimeline;
    public PlayableDirector vetCatLeaveTimeline;

    [SerializeField] private PlayerController playerController;

    // void Start()
    // {
    //     timeline = GetComponent<PlayableDirector>();
    // }

    public void PlayCatCutscene()
    {
        // start timeline
        feedingCatTimeline.Play();
        // set walkingspeed to 0
        playerController.SetWalkSpeedToZero();
        //once dialogue closed, stop and play next cutscene
    }

    private void PlayVetCutscene()
    {
        // start timeline
        vetAppearsTimeline.Play();
        // trigger dialogue with vet
        //once dialogue closed, stop and play next cutscene
    }

    private void PlayVetLeave()
    {
        // start timeline
        vetCatLeaveTimeline.Play();
        // once done, stop cutscene
    }

    private void StopCutscene()
    {
        // end timeline
        vetCatLeaveTimeline.Stop();
        // switch back to main camera
        // set walkingspeed back
        playerController.SetWalkSpeedToNormal();
    }
}
