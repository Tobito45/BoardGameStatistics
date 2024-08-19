using Data;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIStateControllers
{
    public class GamesCharacterInputUIStateController : UIStateControllerBase
    {
        private VisualTreeAsset _gamesCharacterListElement;
        public GamesCharacterInputUIStateController(UIController uIController) : base(uIController) { }
        public override void Installization(VisualElement visualElement)
        {
            _gamesCharacterListElement = Resources.Load<VisualTreeAsset>("Elements/GamesCharacterListElement");
            visualElement.Q<Button>("BackButton").clicked += () => StateMachine.SetGamesInfoInputState();
            ListView listView = visualElement.Q<ListView>("List");
            visualElement.Q<Button>("AddButton").clicked += () => SaveNewGame(visualElement, listView);
            listView.makeItem = () =>
            {
                VisualElement item = _gamesCharacterListElement.Instantiate();
                return item;
            };
            listView.bindItem = (element, index) =>
            {
                element.Q<Label>("Text").text = (listView.itemsSource[index] as Character).Name;
            };
        }
        public override void Clear(VisualElement visualElement) { }

        public override void Update(VisualElement visualElement)
        {
            visualElement.Q<IntegerField>("PointsInput").value = 0;
            ListView listView = visualElement.Q<ListView>("List");
            _uIController.SetInputFieldColor(listView, Color.white, 0);
            listView.Clear();
            listView.itemsSource = ActualData.GetCharacters.ToList();
            listView.Rebuild();
            listView.ClearSelection();
        }

        public bool Validate(ListView listView)
        {
            if (listView.selectedItem == null)
            {
                _uIController.SetInputFieldColor(listView, Color.red, 2);
                return false;
            }
            return true;
        }

        private void SaveNewGame(VisualElement visualElement, ListView listView)
        {
            if (!Validate(listView)) return;
            _uIController.ActualGame.AddNewCharacter(listView.selectedItem as Character, visualElement.Q<IntegerField>("PointsInput").value);
            StateMachine.SetGamesInfoInputState();
        }
    }
}
