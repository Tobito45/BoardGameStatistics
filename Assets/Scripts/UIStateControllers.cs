using States;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIStateControllers
{
    public interface IUIState
    {
        public void Installization(VisualElement visualElement);
        public void Update(VisualElement visualElement);
        public void Clear(VisualElement visualElement);
        
    }
    public abstract class UIStateControllerBase : IUIState
    {
        protected UIController _uIController;
        protected StateMachine StateMachine => _uIController.StateMachine;
        protected GameData ActualData => _uIController.GetActualData;
        public UIStateControllerBase(UIController uIController)
        {
            _uIController = uIController;
        }

        public abstract void Clear(VisualElement visualElement);
        public abstract void Installization(VisualElement visualElement);
        public abstract void Update(VisualElement visualElement);
    }


    public class MainUIStateController : UIStateControllerBase
    {
        private readonly GameDataFactory _gameDataFactory;

        public MainUIStateController(UIController uIController, GameDataFactory gameDataFactory) : base(uIController)
        {
            _gameDataFactory = gameDataFactory;
        }

        public override void Installization(VisualElement visualElement)
        {
            var prefabMainElement = Resources.Load<VisualTreeAsset>("MainElement");
            ScrollView listView = visualElement.Q<ScrollView>("List");
            foreach (GameData data in _gameDataFactory.GetData())
            {

                VisualElement itemUi = prefabMainElement.Instantiate();
                listView.Add(itemUi);
                itemUi.Q<Label>("Name").text = data.Name;
                itemUi.Q<Label>("MarkText").text = data.Mark.ToString("F1");
                itemUi.Q<Label>("GamesText").text = data.Games.ToString();
                itemUi.Q<Button>("MoreButton").clicked += () =>
                {
                    _uIController.SetActualData(data);
                    StateMachine.SetGameState();
                };
                _uIController.LoadImageAsync(itemUi.Q<VisualElement>("Image"), data.Url);
            }
        }

        public override void Clear(VisualElement visualElement) { }

        public override void Update(VisualElement visualElement) { }
    }
    public class GameUIStateController : UIStateControllerBase
    {
        public GameUIStateController(UIController uIController) : base(uIController) { }

        public override void Installization(VisualElement visualElement)
        {
            visualElement.Q<Button>("BackButton").clicked += () => StateMachine.SetMainState();
            visualElement.Q<Button>("ButtonActions").clicked += () => StateMachine.SetActionsState();
        }

        public override void Clear(VisualElement visualElement) { }

        public override void Update(VisualElement visualElement)
        {
            visualElement.Q<Label>("Name").text = ActualData.Name;
            visualElement.Q<Label>("GameLength").text = ActualData.Time.ToString("F1") + " min";
            visualElement.Q<Label>("Players").text = ActualData.Players.ToString();
            visualElement.Q<Label>("CountGames").text = ActualData.Games.ToString();
            visualElement.Q<Label>("GameMark").text = ActualData.Mark.ToString("F1");
            visualElement.Q<Label>("Description").text = ActualData.Description;
            _uIController.LoadImageAsync(visualElement.Q<VisualElement>("Image"), ActualData.Url);
        }
    }
    public class ActionsUIStateController : UIStateControllerBase
    {
        public ActionsUIStateController(UIController uIController) : base(uIController) { }

        public override void Installization(VisualElement visualElement)
        {
            visualElement.Q<Button>("BackButton").clicked += () => StateMachine.SetGameState();
            visualElement.Q<Button>("ButtonReview").clicked += () => StateMachine.SetReviewsState();
        }

        public override void Clear(VisualElement visualElement) { }

        public override void Update(VisualElement visualElement) => visualElement.Q<Label>("Name").text = ActualData.Name;
    }
    public class ReviewsUIStateController : UIStateControllerBase
    {
        VisualTreeAsset prefabReviewElement, prefabPlusElement;

        public ReviewsUIStateController(UIController uIController) : base(uIController) { }

        public override void Installization(VisualElement visualElement)
        {
            visualElement.Q<Button>("BackButton").clicked += () => StateMachine.SetActionsState();
            prefabReviewElement = Resources.Load<VisualTreeAsset>("ReviewElement");
            prefabPlusElement = Resources.Load<VisualTreeAsset>("PlusElement");
        }
        public override void Clear(VisualElement visualElement) { }

        public override void Update(VisualElement visualElement)
        {
            if (visualElement.Q<Label>("Name").text != ActualData.Name 
                || ActualData.GetReviews.Count() != visualElement.Q<ScrollView>("List").childCount + 1)
            {
                visualElement.Q<ScrollView>("List").Clear();

                visualElement.Q<Label>("Name").text = ActualData.Name;
                ScrollView listView = visualElement.Q<ScrollView>("List");
                foreach (Review review in ActualData.GetReviews)
                {
                    VisualElement itemUi = prefabReviewElement.Instantiate();
                    itemUi.Q<Label>("Name").text = review.Name;
                    itemUi.Q<Label>("Text").text = review.Text;
                    itemUi.Q<Label>("Mark").text = review.Mark.ToString("F1");
                    listView.Add(itemUi);
                }
                VisualElement plus = prefabPlusElement.Instantiate();
                listView.Add(plus);
                plus.Q<Button>("Add").clicked += () => StateMachine.SetReviewsInputState();
            }
        }
    }

    public class ReviewsInputUIStateController : UIStateControllerBase
    {
        public ReviewsInputUIStateController(UIController uIController) : base(uIController) { }

        public override void Installization(VisualElement visualElement)
        {
            visualElement.Q<Button>("BackButton").clicked += () => StateMachine.SetReviewsState();
            visualElement.Q<Button>("AddButton").clicked += () => SaveNewReview(visualElement);

            FloatField markInput = visualElement.Q<FloatField>("MarkInput");
            visualElement.RegisterCallback<ChangeEvent<float>>(evt => ValidateFloatMark(markInput));

        }
        public override void Clear(VisualElement visualElement) { }

        public override void Update(VisualElement visualElement)
        {
            TextField textFieldName = visualElement.Q<TextField>("NameInput");
            TextField textFieldText = visualElement.Q<TextField>("TextInput");
            textFieldName.value = string.Empty;
            textFieldText.value = string.Empty;
            SetInputFieldColor(textFieldText, Color.white,0);
            SetInputFieldColor(textFieldName, Color.white,0);
            visualElement.Q<FloatField>("MarkInput").value = 0.0f;
        }

        private void ValidateFloatMark(FloatField floatField)
        {
            if (floatField.value > 10)
                floatField.value = 10;
            if (floatField.value < 0)
                floatField.value = 0;
        }

        private bool Validate(string name, string text)
        {
            return name != string.Empty  && text != string.Empty;
        }

        private void SaveNewReview(VisualElement visualElement)
        {
            TextField textFieldName = visualElement.Q<TextField>("NameInput");
            TextField textFieldText = visualElement.Q<TextField>("TextInput");
            string name = textFieldName.value;
            string text = textFieldText.value;
            float mark = visualElement.Q<FloatField>("MarkInput").value;

            if (!Validate(name, text))
            {
                SetInputFieldColor(textFieldText, Color.red,2);
                SetInputFieldColor(textFieldName, Color.red,2);
                return;
            }
            _uIController.GetActualData.AddReview(new Review(name, mark, text));
            StateMachine.SetReviewsState();
        }

        private void SetInputFieldColor(TextField textField, Color color, int boardSize)
        {
            textField.style.borderBottomColor = color;
            textField.style.borderTopColor = color;
            textField.style.borderLeftColor = color;
            textField.style.borderRightColor = color;

            textField.style.borderBottomWidth = boardSize;
            textField.style.borderTopWidth = boardSize;
            textField.style.borderLeftWidth = boardSize;
            textField.style.borderRightWidth = boardSize;
        }
    }
}
