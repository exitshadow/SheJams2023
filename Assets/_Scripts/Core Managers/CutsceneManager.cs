using System.Collections;
using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
    public PlayableDirector feedingCatTimeline;
    public PlayableDirector vetCatLeaveTimeline;

    [SerializeField] private PlayerController playerController;
    [SerializeField] private SphereCollider catCollider;

    public void PlayCatCutscene()
    {
        feedingCatTimeline.Play();
        playerController.SetWalkSpeedToZero();
        //once vet dialogue closed, stop? and play next cutscene (do through signals? first check code once vet nearby)
    }

    public void PlayVetLeave()
    {
        vetCatLeaveTimeline.Play();
        // once done, stop cutscene (through signals) >> check if camera back to normal or end last cutscene with camera switch
    }

    private void StopCutscene(PlayableDirector timeline)
    {
        timeline.Stop(); 
        playerController.SetWalkSpeedToNormal();
    }

    public void DisableCatCollider()
    {
        catCollider.isTrigger = false;
    }
}
