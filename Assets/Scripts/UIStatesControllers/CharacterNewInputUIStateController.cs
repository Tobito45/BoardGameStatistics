using Data;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIStateControllers
{
    public class CharacterNewInputUIStateController : UIStateControllerBase
    {
        public CharacterNewInputUIStateController(UIController uIController) : base(uIController) { }
        public override void Installization(VisualElement visualElement)
        {
            visualElement.Q<Button>("BackButton").clicked += () => StateMachine.SetCharactersState();
            visualElement.Q<Button>("AddButton").clicked += () => SaveNewCharacter(visualElement);
        }
        public override void Clear(VisualElement visualElement) { }

        public override void Update(VisualElement visualElement)
        {
            UnsignedIntegerField textFieldGames = visualElement.Q<UnsignedIntegerField>("GamesInput");
            UnsignedIntegerField textFieldWins = visualElement.Q<UnsignedIntegerField>("WinsInput");
            TextField textFieldName = visualElement.Q<TextField>("NameInput");
            if (_uIController.ActualCharater == null)
            {
                textFieldGames.value = 0;
                textFieldWins.value = 0;
                textFieldName.value = string.Empty;
                visualElement.Q<Label>("HeadText").text = "New character";
            }
            else
            {
                textFieldGames.value = (uint)_uIController.ActualCharater.Games;
                textFieldWins.value = (uint)_uIController.ActualCharater.Wins;
                textFieldName.value = _uIController.ActualCharater.Name;
                visualElement.Q<Label>("HeadText").text = "Edit character";
            }
            _uIController.SetInputFieldColor(textFieldGames, Color.white, 0);
            _uIController.SetInputFieldColor(textFieldWins, Color.white, 0);
            _uIController.SetInputFieldColor(textFieldName, Color.white, 0);
        }

        private bool Validate(UnsignedIntegerField textFieldGames, UnsignedIntegerField textFieldWins, TextField textFieldName)
        {
            bool result = true;
            if (textFieldGames.value == 0)
            {
                _uIController.SetInputFieldColor(textFieldGames, Color.red, 2);
               // result = false;
            }
            if (textFieldWins.value == 0)
            {
                _uIController.SetInputFieldColor(textFieldWins, Color.red, 2);
              //  result = false;
            }
            if (textFieldName.value == string.Empty)
            {
                _uIController.SetInputFieldColor(textFieldName, Color.red, 2);
                result = false;
            }
            return result;
        }

        private void SaveNewCharacter(VisualElement visualElement)
        {
            TextField textFieldName = visualElement.Q<TextField>("NameInput");
             UnsignedIntegerField textFieldGames = visualElement.Q<UnsignedIntegerField>("GamesInput");
            UnsignedIntegerField textFieldWins = visualElement.Q<UnsignedIntegerField>("WinsInput");

            if (!Validate(textFieldGames, textFieldWins, textFieldName))
                return;
           
            if(_uIController.ActualCharater == null)
                _uIController.GetActualData.AddCharacter(new Character(textFieldName.value));//, (int)textFieldGames.value, (int)textFieldWins.value));
            else
                _uIController.ActualCharater.Name = textFieldName.value;

            _uIController.ActualCharater = null;
            StateMachine.SetCharactersState();
        }
    }
}