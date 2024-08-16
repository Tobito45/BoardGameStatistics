using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIStateControllers
{
    public class GameUIStateController : UIStateControllerBase
    {
        public GameUIStateController(UIController uIController) : base(uIController) { }

        public override void Installization(VisualElement visualElement)
        {
            VisualElement image = visualElement.Q<VisualElement>("ImagesList");

            visualElement.Q<Button>("BackButton").clicked += () => StateMachine.SetMainState();
            visualElement.Q<Button>("ButtonActions").clicked += () => StateMachine.SetActionsState();
            image.RegisterCallback<PointerDownEvent>(evt =>
            {
                if (ActualData.Index == ActualData.GetUrls.Count() - 1)
                    _uIController.StateMachine.SetUrlInputState();
            });
            visualElement.Q<Button>("RightButton").clicked += () =>
            {
                _uIController.LoadImageAsync(image, ActualData.MoveNextPicture());
                visualElement.Q<Label>("CountPictures").text = $"{ActualData.Index + 1}/{ActualData.GetUrls.Count()}";
            };
            visualElement.Q<Button>("LeftButton").clicked += () =>
            {
                _uIController.LoadImageAsync(image, ActualData.MovePreviusPicture());
                visualElement.Q<Label>("CountPictures").text = $"{ActualData.Index + 1}/{ActualData.GetUrls.Count()}";
            };
        }

        public override void Clear(VisualElement visualElement) { }

        public override void Update(VisualElement visualElement)
        {
            _uIController.LoadImageAsync(visualElement.Q<VisualElement>("ImagesList"), ActualData.GetCurrent());
            visualElement.Q<Label>("CountPictures").text = $"{ActualData.Index + 1}/{ActualData.GetUrls.Count()}";
            visualElement.Q<Label>("Name").text = ActualData.Name;
            visualElement.Q<Label>("GameLength").text = $"{(ActualData.Time == -1 ? '-' : ActualData.Time.ToString("F1"))} min";
            visualElement.Q<Label>("Players").text = ActualData.Players == -1 ? "-" : ActualData.Players.ToString("F1");
            visualElement.Q<Label>("CountGames").text = ActualData.Games.ToString();
            visualElement.Q<Label>("GameMark").text = ActualData.Mark == -1 ? "-" : ActualData.Mark.ToString("F1");
            visualElement.Q<Label>("Description").text = ActualData.Description;
            _uIController.LoadImageAsync(visualElement.Q<VisualElement>("Image"), ActualData.Url);
        }
    }
}