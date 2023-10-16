using UnityEngine;

namespace Scriptable_Objects
{
    [CreateAssetMenu(fileName = "TutorialScriptableObject", menuName = "Tutorial")]
    public class TutorialScriptableObject : ScriptableObject
    {
        [System.Serializable]
        public class Page
        {
            public Sprite image;
            [TextArea(3, 10)]
            public string text;
        }

        public string title;
        public Page[] pages;
    }
}
