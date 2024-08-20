using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIStateControllers
{
    public class UrlInputUIStateController : UIStateControllerBase
    {
        VisualTreeAsset _prefabUrlElement, _prefabPlusElement;

        public UrlInputUIStateController(UIController uIController) : base(uIController) { }

        public override void Installization(VisualElement visualElement)
        {
            _prefabUrlElement = Resources.Load<VisualTreeAsset>("Elements/UrlElement");
            _prefabPlusElement = Resources.Load<VisualTreeAsset>("Elements/UrlPlusElement");
            visualElement.Q<Button>("BackButton").clicked += () => StateMachine.SetGameState();
            visualElement.Q<Button>("AddButton").clicked += () => SaveNewUrlList(visualElement.Q<ScrollView>("List"));
        }
        public override void Clear(VisualElement visualElement) { }

        public override void Update(VisualElement visualElement)
        {
            ScrollView list = visualElement.Q<ScrollView>("List");
            list.Clear();

            foreach (string str in ActualData.GetUrls)
            {
                if (str == GameDataFactory.URL_LOADING)
                    continue;

                CreateListItem(list, str);
            }
            VisualElement plus = _prefabPlusElement.Instantiate();
            list.Add(plus);
            plus.Q<Button>("Plus").clicked += () => {
                CreateListItem(list, string.Empty);
            };
        }

        private void CreateListItem(ScrollView list, string str)
        {
            VisualElement itemUi = _prefabUrlElement.Instantiate();
            TextField textField = itemUi.Q<TextField>("NameInput");
            textField.value = str;
            textField.RegisterCallback<ChangeEvent<string>>(evt => _uIController.LoadImage(itemUi.Q<VisualElement>("Image"), evt.newValue));
            _uIController.LoadImage(itemUi.Q<VisualElement>("Image"), str);

            itemUi.Q<Button>("DeleteButton").clicked += () =>
            {
                ActualData.RemoveUrl(str);
                list.Remove(itemUi);
            };
            if (str != string.Empty)
                list.Add(itemUi);
            else
            {
                list.Insert(list.childCount - 1, itemUi);
                _uIController.PickImage(itemUi.Q<VisualElement>("Image"), ref str, textField);
            }
        }

        
        private void SaveNewUrlList(ScrollView list)
        {
            List<string> newUrls = new List<string>();
            foreach (VisualElement visualElement in list.Query<VisualElement>("UrlElement").ToList())
            {
                string str = visualElement.Q<TextField>("NameInput").value;
                if (visualElement.Q<VisualElement>("Image").style.backgroundImage != null)
                    newUrls.Add(str);
            }
            newUrls.Add(GameDataFactory.URL_LOADING);
            ActualData.SetNewListUrl(newUrls);

            StateMachine.SetGameState();
        }
    }
}