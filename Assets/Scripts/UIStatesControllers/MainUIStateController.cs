using Data;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIStateControllers
{
    public class MainUIStateController : UIStateControllerBase
    {
        private readonly GameDataFactory _gameDataFactory;
        private VisualTreeAsset _prefabMainElement, _prefabPlusElement, _prefabImportExport;

        public MainUIStateController(UIController uIController, GameDataFactory gameDataFactory) : base(uIController)
        {
            _gameDataFactory = gameDataFactory;
            // GettingFile();

        }

        public override void Installization(VisualElement visualElement)
        {
            _prefabMainElement = Resources.Load<VisualTreeAsset>("Elements/MainElement");
            _prefabPlusElement = Resources.Load<VisualTreeAsset>("Elements/PlusElement");
            _prefabImportExport = Resources.Load<VisualTreeAsset>("Elements/ImportExportElement");
        }

        public override void Clear(VisualElement visualElement) { }

        public override void Update(VisualElement visualElement)
        {
            ScrollView listView = visualElement.Q<ScrollView>("List");
            listView.Clear();
            foreach (GameData data in _gameDataFactory.GetData())
            {

                VisualElement itemUi = _prefabMainElement.Instantiate();
                listView.Add(itemUi);
                itemUi.Q<Label>("Name").text = data.Name;
                itemUi.Q<Label>("MarkText").text = data.Mark == -1 ? "-" : data.Mark.ToString("F1"); ;
                itemUi.Q<Label>("GamesText").text = data.Games.ToString();
                itemUi.Q<Button>("MoreButton").clicked += () =>
                {
                    _uIController.SetActualData(data);
                    StateMachine.SetGameState();
                };
                _uIController.LoadImageAsync(itemUi.Q<VisualElement>("Image"), data.Url);
            }
            VisualElement plus = _prefabPlusElement.Instantiate();
            listView.Add(plus);
            plus.Q<Button>("Add").clicked += () =>
            {
                StateMachine.SetGameNewInputState();
                //Testic();
                //ShareGeneratedFile();
                // GettingFile();
            };
            VisualElement impExp = _prefabImportExport.Instantiate();
            listView.Add(impExp);
            impExp.Q<Button>("Export").clicked += () => _uIController.Export();
            impExp.Q<Button>("Import").clicked += () => _uIController.Import();
        }
    }
}