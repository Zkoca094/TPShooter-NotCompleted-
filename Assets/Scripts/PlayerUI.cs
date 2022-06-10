using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
	private GameObject player;

	[SerializeField]
	private RectTransform healthBarFill;
	[SerializeField]
	private RectTransform energyBarFill;
	[SerializeField]
	private Text playerNameText;

	void Start()
	{
		playerNameText.text = player.GetComponent<PlayerObjectController>().playerName;
	}

	public void SetPlayer(GameObject _player)
	{
		player = _player;
	}

	void FixedUpdate()
	{
		SetHealthAmount(player.GetComponent<PlayerInfo>().GetHealthPct());
	}
	void SetHealthAmount(float _amount)
	{
		healthBarFill.localScale = new Vector3(_amount, 1f, 1f);
	}
}
