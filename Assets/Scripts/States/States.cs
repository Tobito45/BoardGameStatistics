using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Progress;

namespace States {
    public interface IState
    {
        public void Entry();
        public void Exit();
    }

    public abstract class BaseState : IState 
    { 
        public VisualElement VisualElement { get; private set; }
        public BaseState(VisualElement visualElement)
        {
            VisualElement = visualElement;
            Debug.Log(visualElement.name);
            visualElement.style.display = DisplayStyle.None;
        }
        public virtual void Entry() => VisualElement.style.display = DisplayStyle.Flex;
        public virtual void Exit() => VisualElement.style.display = DisplayStyle.None;

    }

    public class MainState : BaseState, IState
    {
        public MainState(VisualElement visualElement, GameDataFactory gameDataFactory, UIController uiController) : base(visualElement)
        {
            uiController.InstallizationMain(visualElement, gameDataFactory.GetData());
        }

    }

    public class GameState : BaseState, IState
    {
        private readonly VisualElement _visualElement;
        private readonly UIController _uiController;

        public GameState(VisualElement visualElement, UIController uiController) : base(visualElement) 
        {
            _visualElement = visualElement;
            _uiController = uiController;
            _uiController.InstallizationGame(visualElement);
        }

        public override void Entry()
        {
            _uiController.UpdateGame(_visualElement);
            base.Entry();
        }

    }
    public class ActionsState : BaseState, IState
    {
        private readonly VisualElement _visualElement;
        private readonly UIController _uiController;

        public ActionsState(VisualElement visualElement, UIController uiController) : base(visualElement) 
        {
            _visualElement = visualElement;
            _uiController = uiController;
            _uiController.InstallizationActions(_visualElement);
        }

        public override void Entry()
        {
            _uiController.UpdateActions(_visualElement);
            base.Entry();
        }

    }

    public class ReviewsState : BaseState, IState
    {
        private readonly VisualElement _visualElement;
        private readonly UIController _uiController;

        public ReviewsState(VisualElement visualElement, UIController uiController) : base(visualElement)
        {
            _visualElement = visualElement;
            _uiController = uiController;
            _uiController.InstallizationReviews(visualElement);
        }

        public override void Entry()
        {
            _uiController.UpdateReviews(_visualElement);
            base.Entry();
        }

    }
}

