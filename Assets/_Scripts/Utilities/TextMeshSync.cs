using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(TextMeshProUGUI))]
public class TextMeshSync : MonoBehaviour
{
    public TextMeshProUGUI textToCopy;
    private TextMeshProUGUI currentTMP;

    void Awake()
    {
        currentTMP = GetComponent<TextMeshProUGUI>();
    }

    void OnEnable()
    {
        TMPro_EventManager.TEXT_CHANGED_EVENT.Add(ON_TEXT_CHANGED);
    }

    void OnDisable()
    {
        TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(ON_TEXT_CHANGED);
    }

    void ON_TEXT_CHANGED(Object objectChanged)
    {
        if (objectChanged == textToCopy)
        {
            currentTMP.SetText(textToCopy.text);
            currentTMP.ForceMeshUpdate(true);
            //Debug.Log("updating text");
        }
    }
}
