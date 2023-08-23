using System.Collections;
using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
    public PlayableDirector feedingCatTimeline;
    public PlayableDirector vetCatLeaveTimeline;
    private bool hasPlayedCatCutscene = false;

    [SerializeField] protected UIManager uiManager;
    [SerializeField] protected NPC npc;
    [SerializeField] private SphereCollider catCollider;
    [SerializeField] private PlayerController pc;
    [SerializeField] private Vet vetNPC;

    public void PlayCatCutscene()
    {
        if (hasPlayedCatCutscene) return;
        // switch action map where only Interact works
        DisableCatCollider();
        MakeVetCurrentNPC();
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

    public void MakeVetCurrentNPC()
    {
        pc.currentInteractingNPC = vetNPC;
    }

    public void DisableCatCollider()
    {
        catCollider.enabled = false;
    }
}
