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
        private readonly ReviewsState _reviewState;

        public ReviewInputState(VisualElement visualElement, ReviewsState reviewState, UIController uIController) : base(visualElement, uIController)
        {
            _reviewState = reviewState;
        }

        public override void Entry()
        {
            _reviewState.VisualElement.style.display = DisplayStyle.Flex;
            base.Entry();
        }
    }
    public class GamesInfoState : BaseState, IState
    {
        public GamesInfoState(VisualElement visualElement, UIController uIController) : base(visualElement, uIController)
        {
        }
    }
    public class GamesInfoInputState : BaseState, IState
    {
        private readonly GamesInfoState _gamesInfoState;

        public GamesInfoInputState(VisualElement visualElement, GamesInfoState gamesInfoState, UIController uIController) : base(visualElement, uIController)
        {
            _gamesInfoState = gamesInfoState;
        }
        public override void Entry()
        {
            _gamesInfoState.VisualElement.style.display = DisplayStyle.Flex;
            base.Entry();
        }
    }
    public class CharactersState : BaseState, IState
    {
        public CharactersState(VisualElement visualElement, UIController uIController) : base(visualElement, uIController)
        {
        }
    }

    public class CharacterNewInputState : BaseState, IState
    {
        private readonly CharactersState _characterState;

        public CharacterNewInputState(VisualElement visualElement, CharactersState characterState, UIController uIController) : base(visualElement, uIController)
        {
            _characterState = characterState;
        }
        public override void Entry()
        {
            _characterState.VisualElement.style.display = DisplayStyle.Flex;
            base.Entry();
        }
    }

    public class CharacterChangeInputState : BaseState, IState
    {
        private readonly CharactersState _characterState;

        public CharacterChangeInputState(VisualElement visualElement, CharactersState characterState, UIController uIController) : base(visualElement, uIController)
        {
            _characterState = characterState;
        }
        public override void Entry()
        {
            _characterState.VisualElement.style.display = DisplayStyle.Flex;
            base.Entry();
        }
    }

    public class GameNewInputState : BaseState, IState
    {
        private readonly MainState _mainState;

        public GameNewInputState(VisualElement visualElement, MainState mainState, UIController uIController) : base(visualElement, uIController)
        {
            _mainState = mainState;
        }
        public override void Entry()
        {
            _mainState.VisualElement.style.display = DisplayStyle.Flex;
            base.Entry();
        }
    }
}

