using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIStateControllers
{
    public class GameNewInputUIStateController : UIStateControllerBase
    {
        private readonly GameDataFactory _gameDataFactory;
        public GameNewInputUIStateController(UIController uIController, GameDataFactory gameDataFactory) : base(uIController)
        {
            _gameDataFactory = gameDataFactory;
        }

        public override void Installization(VisualElement visualElement)
        {
            visualElement.Q<Button>("BackButton").clicked += () =>
            {
                _uIController.SetActualData(null);
                StateMachine.SetMainState();
            };
            visualElement.Q<Button>("AddButton").clicked += () =>
            {
                SaveNewGame(visualElement);
                _uIController.SetActualData(null);
            };
        }
        public override void Clear(VisualElement visualElement) { }

        public override void Update(VisualElement visualElement)
        {
            TextField textFieldName = visualElement.Q<TextField>("NameInput");
            TextField textFieldUrl = visualElement.Q<TextField>("UrlInput");
            TextField textFieldText = visualElement.Q<TextField>("TextInput");
            if(ActualData == null)
            {
                textFieldName.value = string.Empty;
                textFieldUrl.value = string.Empty;
                textFieldText.value = string.Empty;
                visualElement.Q<Label>("HeadText").text = "New board game";
            } else
            {
                textFieldName.value = ActualData.Name;
                textFieldUrl.value = ActualData.Url;
                textFieldText.value = ActualData.Description;
                visualElement.Q<Label>("HeadText").text = "Edit board game";
            }
            _uIController.SetInputFieldColor(textFieldName, Color.white, 0);
            _uIController.SetInputFieldColor(textFieldUrl, Color.white, 0);
            _uIController.SetInputFieldColor(textFieldText, Color.white, 0);
        }

        private bool Validate(TextField textFieldName, TextField textFieldUrl, TextField textFieldText)
        {
            bool result = true;
            if (textFieldName.value == string.Empty)
            {
                _uIController.SetInputFieldColor(textFieldName, Color.red, 2);
                result = false;
            }
            if (textFieldUrl.value == string.Empty)
            {
                _uIController.SetInputFieldColor(textFieldUrl, Color.red, 2);
                result = false;
            }
            if (textFieldText.value == string.Empty)
            {
                _uIController.SetInputFieldColor(textFieldText, Color.red, 2);
                result = false;
            }
            return result;
        }

        private void SaveNewGame(VisualElement visualElement)
        {
            TextField textFieldName = visualElement.Q<TextField>("NameInput");
            TextField textFieldUrl = visualElement.Q<TextField>("UrlInput");
            TextField textFieldText = visualElement.Q<TextField>("TextInput");

            if (!Validate(textFieldName, textFieldUrl, textFieldName))
                return;


            if (ActualData == null)
            {
                GameData gameData = new GameData(textFieldName.value, textFieldUrl.value, textFieldText.value);
                gameData.AddUrl(GameDataFactory.URL_LOADING);
                _gameDataFactory.AddBoardGame(gameData);
            } else
            {
                ActualData.Name = textFieldName.value;
                ActualData.Url = textFieldUrl.value;
                ActualData.Description = textFieldText.value;
            }

            StateMachine.SetMainState();
        }
    }
}