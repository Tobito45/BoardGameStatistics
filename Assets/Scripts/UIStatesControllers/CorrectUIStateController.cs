using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIStateControllers
{
    public class CorrectUIStateController : UIStateControllerBase
    {
        public CorrectUIStateController(UIController uIController) : base(uIController) { }

        public override void Installization(VisualElement visualElement)
        {
            visualElement.Q<Button>("NoButton").clicked += () => StateMachine.TransitionTo(_uIController.CorrectInfo.previousState);
            visualElement.Q<Button>("YesButton").clicked += () =>
            {
                _uIController.CorrectInfo.yesFunction();
                StateMachine.TransitionTo(_uIController.CorrectInfo.previousState);
            };
        }

        public override void Clear(VisualElement visualElement) { }

        public override void Update(VisualElement visualElement)
        {
            _uIController.CorrectInfo.previousState.Entry();
            visualElement.Q<Label>("Text").text = _uIController.CorrectInfo.message;
        }
    }
}