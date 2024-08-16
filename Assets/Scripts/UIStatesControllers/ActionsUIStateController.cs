using UnityEngine.UIElements;

namespace UIStateControllers
{
    public class ActionsUIStateController : UIStateControllerBase
    {
        public ActionsUIStateController(UIController uIController) : base(uIController) { }

        public override void Installization(VisualElement visualElement)
        {
            visualElement.Q<Button>("BackButton").clicked += () => StateMachine.SetGameState();
            visualElement.Q<Button>("ButtonReview").clicked += () => StateMachine.SetReviewsState();
            visualElement.Q<Button>("ButtonGame").clicked += () => StateMachine.SetGamesInfoState();
            visualElement.Q<Button>("ButtonCharacters").clicked += () => StateMachine.SetCharactersState();
        }

        public override void Clear(VisualElement visualElement) { }

        public override void Update(VisualElement visualElement) => visualElement.Q<Label>("Name").text = ActualData.Name;
    }
}