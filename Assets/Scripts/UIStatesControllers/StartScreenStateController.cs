using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIStateControllers
{
    public class StartScreenStateController : UIStateControllerBase
    {
        public StartScreenStateController(UIController uIController) : base(uIController) { }

        public override void Installization(VisualElement visualElement)
        {
            visualElement.Q<Label>("Version").text = "version: " + Application.unityVersion;
        }

        public override void Clear(VisualElement visualElement) { }

        public override void Update(VisualElement visualElement) { }
    }
}