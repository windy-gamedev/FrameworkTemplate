using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Wigi.Utilities;

namespace Wigi.Turorial
{
    public class TutorialManager : Singleton<TutorialManager>
    {
        public TutorialLayout tutorialLayout;
        public TutorialPlayer tutorialPlayer;

        private void Start()
        {
            tutorialLayout = GetComponent<TutorialLayout>();
            tutorialPlayer = GetComponent<TutorialPlayer>();
        }

        public void FocusButton(Image button)
        {
            button.GetSize();
            var rectTrans = button.GetComponent<RectTransform>();
            tutorialLayout.SetLayoutFocus(rectTrans.GetWorldRect());
        }
    }
}
