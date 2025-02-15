using System.Collections.Generic;
using UnityEngine;

public enum GameState 
{
	Loading,
	Intro,
	Tracking,
	POIPopUp,
	UI,
	GameOver
}

[CreateAssetMenu(fileName = "New GameState", menuName = "Gameplay/GameState SO")]
public class GameStateSO : ScriptableObject
{
    [Header("Debug")]
    [SerializeField] private DebugUIEventChannelSO debugUIEventChannelSO;

    [Header("Game states")]
	[SerializeField] private GameState _currentGameState = default;
	[SerializeField] private GameState _previousGameState = default;

	public GameState CurrentGameState => _currentGameState;
    public GameState PreviousGameState => _previousGameState;

    public void UpdateGameState(GameState newGameState)
	{
		if (newGameState == CurrentGameState)
			return;

		_previousGameState = _currentGameState;
		_currentGameState = newGameState;

        debugUIEventChannelSO.RaiseDebugEventGameState(newGameState.ToString());
    }

	public void ResetToPreviousGameState()
	{
		if (_previousGameState == _currentGameState)
			return;

		GameState stateToReturnTo = _previousGameState;
		_previousGameState = _currentGameState;
		_currentGameState = stateToReturnTo;
	}

	public void ResetToState(GameState state)
	{
        _previousGameState = state;
        _currentGameState = state;
    }
}
