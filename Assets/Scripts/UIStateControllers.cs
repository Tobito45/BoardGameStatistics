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
                itemUi.Q<Label>("MarkText").text = data.Mark == -1 ? "-" : data.Mark.ToString("F1");;
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
            visualElement.Q<Label>("GameLength").text = $"{(ActualData.Time == -1 ? '-' : ActualData.Time.ToString("F1"))} min";
            visualElement.Q<Label>("Players").text = ActualData.Players == -1 ? "-" : ActualData.Players.ToString("F1");
            visualElement.Q<Label>("CountGames").text = ActualData.Games.ToString();
            visualElement.Q<Label>("GameMark").text = ActualData.Mark == -1 ? "-" : ActualData.Mark.ToString("F1");
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
            visualElement.Q<Button>("ButtonGame").clicked += () => StateMachine.SetGamesInfoState();
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
         //   if (visualElement.Q<Label>("Name").text != ActualData.Name 
          //      || ActualData.GetReviews.Count() != visualElement.Q<ScrollView>("List").childCount + 1)
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
            _uIController.SetInputFieldColor(textFieldText, Color.white,0);
            _uIController.SetInputFieldColor(textFieldName, Color.white,0);
            visualElement.Q<FloatField>("MarkInput").value = 0.0f;
        }

        private void ValidateFloatMark(FloatField floatField)
        {
            if (floatField.value > 10)
                floatField.value = 10;
            if (floatField.value < 0)
                floatField.value = 0;
        }

        private bool Validate(TextField textFieldName, TextField textFieldText)
        {
            bool result = true;
            if (textFieldName.value == string.Empty)
            {
                _uIController.SetInputFieldColor(textFieldName, Color.red, 2);
                result = false;
            }
            if (textFieldText.value != string.Empty)
            {
                _uIController.SetInputFieldColor(textFieldText, Color.red, 2);
                result = false;
            }
            return result;
        }

        private void SaveNewReview(VisualElement visualElement)
        {
            TextField textFieldName = visualElement.Q<TextField>("NameInput");
            TextField textFieldText = visualElement.Q<TextField>("TextInput");
            float mark = visualElement.Q<FloatField>("MarkInput").value;

            if (!Validate(textFieldName, textFieldText))
            {
                _uIController.SetInputFieldColor(textFieldText, Color.red,2);
                _uIController.SetInputFieldColor(textFieldName, Color.red,2);
                return;
            }
            _uIController.GetActualData.AddReview(new Review(textFieldName.value, mark, textFieldText.value));
            StateMachine.SetReviewsState();
        }
    }

    public class GamesInfoUIStateController : UIStateControllerBase
    {
        VisualTreeAsset prefabGamesElement, prefabPlusElement;

        public GamesInfoUIStateController(UIController uIController) : base(uIController) { }

        public override void Installization(VisualElement visualElement)
        {
            visualElement.Q<Button>("BackButton").clicked += () => StateMachine.SetActionsState();
            prefabGamesElement = Resources.Load<VisualTreeAsset>("GamesInfoElement");
            prefabPlusElement = Resources.Load<VisualTreeAsset>("PlusElement");
        }
        public override void Clear(VisualElement visualElement) { }

        public override void Update(VisualElement visualElement)
        {
         //   if (int.Parse(visualElement.Q<Label>("Players").text) != ActualData.Players
         //       || ActualData.GetGames.Count() != visualElement.Q<ScrollView>("List").childCount + 1)
            {
                visualElement.Q<ScrollView>("List").Clear();

                visualElement.Q<Label>("Name").text = ActualData.Name;
                ScrollView listView = visualElement.Q<ScrollView>("List");
                List<Game> games = ActualData.GetGames.ToList();
                for(int i = 0; i < games.Count(); i++)
                {
                    VisualElement itemUi = prefabGamesElement.Instantiate();
                    itemUi.Q<Label>("Number").text = i.ToString();
                    itemUi.Q<Label>("Players").text = games[i].Players.ToString();
                    itemUi.Q<Label>("Time").text = games[i].Time.ToString("F1") + " min";
                    itemUi.Q<Label>("Text").text = games[i].Comment;
                    listView.Add(itemUi);
                }
                VisualElement plus = prefabPlusElement.Instantiate();
                listView.Add(plus);
                plus.Q<Button>("Add").clicked += () => StateMachine.SetGamesInfoInputState();
            }
        }
    }

    public class GamesInfoInputUIStateController : UIStateControllerBase
    {
        public GamesInfoInputUIStateController(UIController uIController) : base(uIController) { }

        public override void Installization(VisualElement visualElement)
        {
            visualElement.Q<Button>("BackButton").clicked += () => StateMachine.SetGamesInfoState();
            visualElement.Q<Button>("AddButton").clicked += () => SaveNewGame(visualElement);
        }
        public override void Clear(VisualElement visualElement) { }

        public override void Update(VisualElement visualElement)
        {
            UnsignedIntegerField textFieldPlayers = visualElement.Q<UnsignedIntegerField>("PlayersInput");
            UnsignedIntegerField textFieldTime = visualElement.Q<UnsignedIntegerField>("TimeInput");
            textFieldPlayers.value = 0;
            textFieldTime.value = 0;
            _uIController.SetInputFieldColor(textFieldPlayers, Color.white, 0);
            _uIController.SetInputFieldColor(textFieldTime, Color.white, 0);
            visualElement.Q<TextField>("TextInput").value = string.Empty;
        }

        private bool Validate(UnsignedIntegerField textFieldPlayers, UnsignedIntegerField textFieldTime)
        {
            bool result = true;
            if(textFieldPlayers.value == 0)
            {
                _uIController.SetInputFieldColor(textFieldPlayers, Color.red, 2);
                result = false;
            }
            if (textFieldTime.value == 0)
            {
                _uIController.SetInputFieldColor(textFieldTime, Color.red, 2);
                result = false;
            }
            return result;
        }

        private void SaveNewGame(VisualElement visualElement)
        {
            UnsignedIntegerField textFieldPlayers = visualElement.Q<UnsignedIntegerField>("PlayersInput");
            UnsignedIntegerField textFieldTime = visualElement.Q<UnsignedIntegerField>("TimeInput");
            string comment = visualElement.Q<TextField>("TextInput").value;

            if (!Validate(textFieldPlayers, textFieldTime))
                return;
            
            _uIController.GetActualData.AddGame(new Game((int)textFieldPlayers.value, (int)textFieldTime.value, comment));
            StateMachine.SetGamesInfoState();
        }
    }
}
