using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Steamworks;
public class PlayerObjectController : NetworkBehaviour
{
    [SyncVar] public int ConnectionID;
    [SyncVar] public int playerIDNumber;
    [SyncVar] public ulong PlayerSteamID;
    [SyncVar(hook = nameof(PlayerNameUpdate))] public string playerName;
    [SyncVar(hook = nameof(PlayerReadyUpdate))] public bool Ready;
    [SerializeField] private Behaviour[] componentsToDisable;
    [SerializeField] private GameObject[] objectToDisable;
    private CustomNetworkManager manager;
    private CustomNetworkManager Manager
    {
        get {
            if (manager != null)
                return manager;
            return manager = CustomNetworkManager.singleton as CustomNetworkManager;
        }

    }
    void ActiveselfComponents(bool state)
    {
        for (int i = 0; i < componentsToDisable.Length; i++)
        {
            componentsToDisable[i].enabled = state;
        }
    }
    void ActiveselfObject(bool state)
    {
        for (int i = 0; i < objectToDisable.Length; i++)
        {
            objectToDisable[i].SetActive(state);
        }
    }
    public override void OnStartAuthority()
    {
        CmdSetPlayerName(SteamFriends.GetPersonaName().ToString());
        gameObject.name = "LocalGamePlayer";
        LobbyController.instance.FindLocalPlayer();
        LobbyController.instance.UpdateLobbyName();
        ActiveselfComponents(false);
        ActiveselfObject(false);
    }
    public override void OnStartClient()
    {
        Manager.GamePlayers.Add(this);
        LobbyController.instance.UpdateLobbyName();
        LobbyController.instance.UpdatePlayerList();
    }
    public override void OnStopClient()
    {
        Manager.GamePlayers.Remove(this);
        LobbyController.instance.UpdatePlayerList();
    }
    [Command]
    public void CmdSetPlayerName(string playerName)
    {
        this.PlayerNameUpdate(this.playerName, playerName);
    }
    public void PlayerNameUpdate(string OldValue, string NewValue)
    {
        if (isServer)
        {
            this.playerName = NewValue;
        }
        if (isClient)
        {
            LobbyController.instance.UpdatePlayerList();
        }
    }
    public void PlayerReadyUpdate(bool OldValue, bool NewValue)
    {
        if (isServer)
        {
            this.Ready = NewValue;
        }
        if (isClient)
        {
            LobbyController.instance.UpdatePlayerList();
        }
    }
    [Command]
    private void CmdSetPlayerReady()
    {
        this.PlayerReadyUpdate(this.Ready, !this.Ready);
    }
    public void ChangeReady()
    {
        if (hasAuthority)
        {
            CmdSetPlayerReady();
        }
    }
    public void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void CanStartGame(string sceneName)
    {
        if (hasAuthority)
            CmdCanStartGame(sceneName);
    }
    [Command]
    public void CmdCanStartGame(string sceneName)
    {
        manager.StartGame(sceneName);        
        ActiveselfObject(true);
        ActiveselfComponents(true);
    }
}
