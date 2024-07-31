using System.Collections;
using System.Collections.Generic;
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
        public VisualElement VisualElement { get; private set; }
        public BaseState(VisualElement visualElement)
        {
            VisualElement = visualElement;
            visualElement.style.display = DisplayStyle.None;
        }
        public virtual void Entry() => VisualElement.style.display = DisplayStyle.Flex;
        public virtual void Exit() => VisualElement.style.display = DisplayStyle.None;

    }

    public class MainState : BaseState, IState
    {
        public MainState(VisualElement visualElement, GameDataFactory gameDataFactory, UIController gameDataController) : base(visualElement) 
        {
            VisualTreeAsset prefabElement = Resources.Load<VisualTreeAsset>("ElementListView");

            ScrollView listView = visualElement.Q<ScrollView>("List");

            foreach (GameData item in gameDataFactory.GetData())
            {
                VisualElement itemUi = prefabElement.Instantiate();
                listView.Add(itemUi);
                gameDataController.InstallizationMain(itemUi, item);
            }
        }

        public override void Entry()
        {
            base.Entry();
        }

        public override void Exit()
        {
            base.Exit();
        }
    }

    public class GameState : BaseState, IState
    {
        private readonly VisualElement visualElement;
        private readonly UIController gameDataController;

        public GameState(VisualElement visualElement, UIController gameDataController) : base(visualElement) 
        {
            this.visualElement = visualElement;
            this.gameDataController = gameDataController;
        }

        public override void Entry()
        {
            gameDataController.InstallizationGame(visualElement);
            base.Entry();
        }

        public override void Exit()
        {
            gameDataController.ClearGame(visualElement);
            base.Exit();
        }
    }
    public class ActionsState : BaseState, IState
    {
        private readonly VisualElement visualElement;
        private readonly UIController gameDataController;

        public ActionsState(VisualElement visualElement, UIController gameDataController) : base(visualElement) 
        {
            this.visualElement = visualElement;
            this.gameDataController = gameDataController;
        }

        public override void Entry()
        {
            gameDataController.InstallizationActions(visualElement);
            base.Entry();
        }

        public override void Exit()
        {
            gameDataController.ClearActions(visualElement);
            base.Exit();
        }
    }
}

