using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum Scene
{
	MainMenu,
	Lobby,
	Game
}
public class GameManager : MonoBehaviour
{
	public static GameManager instance;
	public delegate void OnPlayerKilledCallback(string player, string source);
	public OnPlayerKilledCallback onPlayerKilledCallback;
	public Scene scene;
	#region Singleton
	void Awake()
	{
		if (instance != null)
		{
			Debug.LogError("More than one GameManager in scene.");
		}
		else
		{
			instance = this;
		}
		DontDestroyOnLoad(this);
	}
	#endregion

	#region Player tracking

	private const string PLAYER_ID_PREFIX = "Player";

	private Dictionary<string, PlayerInfo> players = new Dictionary<string, PlayerInfo>();

	public void RegisterPlayer(string _netID, PlayerInfo _player)
	{
		string _playerID = PLAYER_ID_PREFIX + _netID;
		players.Add(_playerID, _player);
		_player.transform.name = _playerID;
	}

	public void UnRegisterPlayer(string _playerID)
	{
		players.Remove(_playerID);
	}

	public PlayerInfo GetPlayer(string _playerID)
	{
		return players[_playerID];
	}
	public PlayerInfo[] GetAllPlayers()
	{
		return players.Values.ToArray();
	}
	#endregion

}
