using States;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;


namespace UIStateControllers
{
    public class ErrorUIStateController : UIStateControllerBase
    {

        public ErrorUIStateController(UIController uIController) : base(uIController) { }
        public override void Installization(VisualElement visualElement)
        {
            visualElement.Q<Button>("BackButton").clicked += () => StateMachine.TransitionTo(_uIController.ErrorInfo.previousState);
        }

        public override void Clear(VisualElement visualElement) { }

        public override void Update(VisualElement visualElement)
        {
            _uIController.ErrorInfo.previousState.Entry();
            visualElement.Q<Label>("Text").text = _uIController.ErrorInfo.message;
        }

    }
}
