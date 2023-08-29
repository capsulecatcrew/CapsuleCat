using Dialog.Scripts;
using UnityEngine;

/// <summary>
/// Starts the conversation with the
/// shop owner NPC upon entering a scene
/// </summary>
public class StartDialogInScene : MonoBehaviour
{
    public DialogBehaviour dialogBehaviour;

    void Start()
    {
        dialogBehaviour.StartDialog(); 
    }
}
