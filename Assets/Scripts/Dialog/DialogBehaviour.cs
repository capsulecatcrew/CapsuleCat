using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEditor;

namespace Dialog.Scripts
{
    /// <summary>
    /// Handles the NPC interaction with a player.
    /// Takes in a text file with a conversation to be displayed.
    /// Passes the file to the DialogManger to parse.
    /// </summary>
    public class DialogBehaviour : MonoBehaviour, ISpeakable
    {
        [Header("DialogBehaviour Base Vars")]
        [SerializeField] protected bool _letPlayerControlDialog = true;
        [Space]
        [SerializeField] protected float textSpeedNorm = 0.022f;
        [SerializeField] protected float textSpeedSlow = 0.06f;
        [SerializeField] protected float textSpeedFast = 0.01f;
        [Space]
        [SerializeField] protected TextMeshProUGUI textDisplay;
        [SerializeField] private TextAsset convoTextFile;
        // [SerializeField] private Animator nextPageIcon; // stretch goal: bouncing continue animator
        
        protected Queue<(TextSpeed speed, string speech)> _convoQueue;
        protected (TextSpeed speed, string speech) _currSpeech;
        protected bool _doneTalking = true; // done showing all the text in the current speech.
        protected bool _isConvoOngoing; // player is reading through the queue of speeches
        // ^ can also be speeches.Count == 0;
        protected bool _dialogBtnIsPressed = false;
        // protected InputManager _inputManager;
        protected Coroutine _currTalkingCoroutine;

        protected virtual void Start() // virtual means able to be overridden
        {
            // _inputManager = InputManager.GetInstance();

            // if you wish to start the dialog when the scene loads, 
            // drag a StartDialogInScene.cs script into the scene instead of doing it here
        }

        protected virtual void Update()
        {
            if (!_letPlayerControlDialog) return;

            // !A &  B
            //  A & !B
            //  TODO: B is left as placeholder for the interact function
            if (!_dialogBtnIsPressed /* && player pressed Interact */)
            {
                FinishCurrSentence();
                _dialogBtnIsPressed = true;
                return;
            }
            
            if (_dialogBtnIsPressed /* && !player pressed Interact */)
            {
                _dialogBtnIsPressed = false;
            }
        }

        /// <summary>
        /// Let the player's controller button trigger the next dialog
        /// </summary>
        public void EnablePlayerControl()
        {
            _letPlayerControlDialog = true;
        }
        
        /// <summary>
        /// Called by text asset events to disable player continuation
        /// </summary>
        public void DisablePlayerControl()
        {
            _letPlayerControlDialog = false;
        }

        /// <summary>
        /// Passes the dialog to display and the text panel to the
        /// DialogManager to parse and display.
        /// Triggered by interacting with NPCs (Unity Event).
        /// </summary>
        public virtual void StartDialog()
        {
            _isConvoOngoing = true;
            
            _convoQueue = DialogParser.ParseTextFileAsQueue(convoTextFile);
            _currSpeech = _convoQueue.Dequeue();
            StartCoroutine(TypeCurrSpeech(_currSpeech.speech, _currSpeech.speed));
        }
        
        
        /// <summary>
        /// Closes the text panel and stops talking.
        ///
        /// Triggered by Cancel input action.
        /// </summary>
        public virtual void EndDialog()
        {
            StopCoroutine(_currTalkingCoroutine);
            _isConvoOngoing = false;
        }

        protected void StartTypeCurrSpeech(string thisSentence, TextSpeed speed)
        {
            _currTalkingCoroutine = StartCoroutine(TypeCurrSpeech(thisSentence, speed));
        }
        
        /// <summary>
        /// Adds the speech letter by letter to the display box.
        /// </summary>
        protected IEnumerator TypeCurrSpeech(string thisSentence, TextSpeed speed)
        {
            textDisplay.text = "";
            _doneTalking = false; // flag, telling the show-remaining-speech line to show all if still talking
            // nextPageIcon.SetBool("doneTalking", false); // stop the Continue arrow from bouncing

            // set the talking speed
            var currTextSpeed = speed == TextSpeed.Normal
                ? textSpeedNorm
                : speed == TextSpeed.Slow
                ? textSpeedSlow
                : textSpeedFast;
            
            // add each letter to the display
            foreach (char letter in thisSentence)
            {
                textDisplay.text += letter;
                yield return new WaitForSeconds(currTextSpeed);
            }

            _doneTalking = true;
            // nextPageIcon.SetBool("doneTalking", true); // make the Continue arrow bounce
        }
        
        /// <summary>
        /// When player wants to see the next page,
        /// if it is the end of the dialog, exit it.
        /// if the NPC has not printed out the full speech, print it out.
        /// if the NPC has printed out the full speech, move on to the next one.
        ///
        /// Triggered by Continue input action.
        /// </summary>
        public virtual void FinishCurrSentence()
        {
            if (!_isConvoOngoing) return;
            if (_currTalkingCoroutine != null) StopCoroutine(_currTalkingCoroutine);
            if (!_doneTalking) // show all remaining text in speech, stop typing
            {
                textDisplay.text = _currSpeech.speech;
                _doneTalking = true;
                // make the continue arrow bounce
                // nextPageIcon.SetBool("doneTalking", true);
            }
            else // start typing the next speech
            {
                if (_convoQueue.Count == 0)
                {
                    EndDialog();
                    return;
                }
                _currSpeech = _convoQueue.Dequeue();
                StartTypeCurrSpeech(_currSpeech.speech, _currSpeech.speed);
            }
            
        }

    }
}

#if UNITY_EDITOR
namespace Dialog.Scripts
{
    [CustomEditor(typeof(DialogBehaviour))]
    public class DialogBehaviourEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            DialogBehaviour thisScript = (DialogBehaviour) target;

            EditorGUILayout.LabelField("Controls");
            if (GUILayout.Button("Start Dialog"))
            {
                thisScript.StartDialog();
            }

            if (GUILayout.Button("Next"))
            {
                thisScript.FinishCurrSentence();
            }

            if (GUILayout.Button("End Dialog"))
            {
                thisScript.EndDialog();
            }
        }
    }
}
#endif

