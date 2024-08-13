using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace States
{
    public interface IState
    {
        public void Entry();
        public void Exit();
    }

    public abstract class BaseState : IState
    {
        private readonly UIController _uIController;

        public VisualElement VisualElement { get; private set; }
        public BaseState(VisualElement visualElement, UIController uIController)
        {
            VisualElement = visualElement;
            _uIController = uIController;
            _uIController.GetController(this).Installization(VisualElement);
            visualElement.style.display = DisplayStyle.None;
        }
        public virtual void Entry()
        {
            VisualElement.style.display = DisplayStyle.Flex;
            _uIController.GetController(this).Update(VisualElement);
        }
        public virtual void Exit()
        {
            VisualElement.style.display = DisplayStyle.None;
            _uIController.GetController(this).Clear(VisualElement);
        }
    }
}
