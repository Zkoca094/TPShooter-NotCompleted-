using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Steamworks;
public class PlayerInfo : NetworkBehaviour
{		
	[SerializeField]
	private bool _isDead = false;
	public bool isDead
	{
		get { return _isDead; }
		protected set { _isDead = value; }
	}

	[SerializeField]
	public float maxHealth = 100f;

	public float currentHealth;
	public float GetHealthPct()
	{
		return (float)currentHealth / maxHealth;
	}
	public int itemSpace = 10;

	public void SetupPlayer()
	{
		CmdBroadCastNewPlayerSetup();
	}
	[Command]
	private void CmdBroadCastNewPlayerSetup()
	{
		RpcSetupPlayerOnAllClients();
	}
	[ClientRpc]
	private void RpcSetupPlayerOnAllClients()
	{
		SetDefaults();
	}
	public void SetDefaults()
	{
		isDead = false;

		currentHealth = maxHealth;
	}
	[ClientRpc]
	public void RpcTakeDamage(int _amount)
	{
		if (isDead)
			return;

		currentHealth -= _amount;
		Debug.Log(transform.name + " now has " + currentHealth + " health.");

		if (currentHealth <= 0)
		{
			Die();
		}
	}
	[ClientRpc]
	public void EnemyDamage(int _amount)
	{
		if (isDead)
			return;

		currentHealth -= _amount;
		Debug.Log(transform.name + " now has " + currentHealth + " health.");

		if (currentHealth <= 0)
		{
			Die();
		}
	}
	private void Die()
	{
		isDead = true;
		Debug.Log(transform.name + " is DEAD!");
	}
}
