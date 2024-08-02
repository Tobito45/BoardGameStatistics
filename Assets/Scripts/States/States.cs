using UnityEngine;
using UnityEngine.UIElements;

namespace States {
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

    public class MainState : BaseState, IState
    {
        public MainState(VisualElement visualElement, UIController uIController) : base(visualElement, uIController)
        {
        }
    }

    public class GameState : BaseState, IState
    {
        public GameState(VisualElement visualElement, UIController uIController) : base(visualElement, uIController)
        {
        }
    }
    public class ActionsState : BaseState, IState
    {
        public ActionsState(VisualElement visualElement, UIController uIController) : base(visualElement, uIController)
        {
        }
    }

    public class ReviewsState : BaseState, IState
    {
        public ReviewsState(VisualElement visualElement, UIController uIController) : base(visualElement, uIController)
        {
        }
    }
    public class ReviewInputState : BaseState, IState
    {
        private readonly ReviewsState reviewState;

        public ReviewInputState(VisualElement visualElement, ReviewsState reviewState, UIController uIController) : base(visualElement, uIController)
        {
            this.reviewState = reviewState;
        }

        public override void Entry()
        {
            reviewState.VisualElement.style.display = DisplayStyle.Flex;
            base.Entry();
        }
    }
}

