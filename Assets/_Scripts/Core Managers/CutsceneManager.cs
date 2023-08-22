using System.Collections;
using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
    public PlayableDirector feedingCatTimeline;
    public PlayableDirector vetCatLeaveTimeline;
    private bool hasPlayedCatCutscene = false;

    [SerializeField] private SphereCollider catCollider;

    public void PlayCatCutscene()
    {
        if (hasPlayedCatCutscene) return;
        // switch action map to cutscene where only Interact works >> add new marker and method?
        feedingCatTimeline.Play();
        hasPlayedCatCutscene = true;
    }

    public void PlayVetLeave()
    {
        // use bool
        // vetCatLeaveTimeline.Play();
    }

    public void StopCatCutscene()
    {
        feedingCatTimeline.Stop();
        // switch back action map
    }

    public void DisableCatCollider()
    {
        // catCollider.isTrigger = false;
        // or disable collider, but then cat might walk through Iman
    }
}
