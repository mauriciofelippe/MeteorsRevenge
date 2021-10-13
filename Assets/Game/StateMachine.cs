using UnityEngine;

namespace Game
{
    public class StateMachine
    {
        private CommandState _currentCommandState;
        private CommandState _lastCommandState;

        private float lastTime;

        public void BackState()
        {
            if (_currentCommandState == null || _lastCommandState == null) return;
        
            _currentCommandState?.Exit();
            _currentCommandState = _lastCommandState;
            lastTime = Time.realtimeSinceStartup;
            _currentCommandState.Enter();
        }
    
        public void ChangeState(CommandState newCommandState)
        {
            _currentCommandState?.Exit();
            if(_currentCommandState != newCommandState)
                _lastCommandState = _currentCommandState;
            _currentCommandState = newCommandState;
            lastTime = Time.realtimeSinceStartup;
        
            _currentCommandState.Enter();
        }

        public void Update()
        {
            if(Time.realtimeSinceStartup > lastTime+0.2f)
                _currentCommandState?.Execute();  
        }

        public void DirectUpdate()
        {
            _currentCommandState?.Execute();
        }

        public CommandState GetState() => _currentCommandState;

    }

    public abstract class CommandState
    {
        public abstract void Enter();
        public abstract void Execute();
        public abstract void Exit();
    }
}