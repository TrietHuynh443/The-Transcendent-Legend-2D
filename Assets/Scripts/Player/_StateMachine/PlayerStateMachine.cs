
namespace Player.PlayerStates.PlayerStateMachine
{
    public class PlayerStateMachine
    {
        public PlayerState CurrentState { get; private set; }

        public void Initialize(PlayerState startingState)
        {
            CurrentState = startingState;
            CurrentState.Enter();
        }

        public void ChangeState(PlayerState newState)
        {
            if(CurrentState == newState)
                return;
            CurrentState.Exit();
            CurrentState = newState;
            CurrentState.Enter();
        }
    }
}