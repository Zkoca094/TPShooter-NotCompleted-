using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
public class LobbyListManager : MonoBehaviour
{
    public static LobbyListManager instance;
    public GameObject lobbiesMenu;
    public GameObject lobbyDataItemPrefab;
    public GameObject lobbylistContent;
    public GameObject lobbiesButton, hostButton, settingButton, controlButton, exitButton;
    public List<GameObject> listOfLobbies = new List<GameObject>();
    private void Awake()
    {
        if (instance == null) { instance = this; }
        GameManager.instance.scene = Scene.MainMenu;
    }
    public void DestroyLobbies()
    {
        foreach (GameObject lobbyitem in listOfLobbies)
        {
            Destroy(lobbyitem);
        }
        listOfLobbies.Clear();
    }
    public void DisplayLobbies(List<CSteamID> lobbyIDs, LobbyDataUpdate_t result)
    {
        for (int i = 0; i < lobbyIDs.Count; i++)
        {
            if (lobbyIDs[i].m_SteamID==result.m_ulSteamIDLobby)
            {
                GameObject createdItem = Instantiate(lobbyDataItemPrefab);
                createdItem.GetComponent<LobbyEntryData>().lobbyID = (CSteamID)lobbyIDs[i].m_SteamID;
                createdItem.GetComponent<LobbyEntryData>().lobbyName = SteamMatchmaking.GetLobbyData((CSteamID)lobbyIDs[i].m_SteamID, "name");
                createdItem.GetComponent<LobbyEntryData>().SetLobbyData();
                createdItem.transform.SetParent(lobbylistContent.transform);
                createdItem.transform.localScale = Vector3.one;
                listOfLobbies.Add(createdItem);
            }
        }
    }
    public void GetListOfLobbies()
    {
        lobbiesButton.SetActive(false);
        hostButton.SetActive(false);
        settingButton.SetActive(false);
        controlButton.SetActive(false);
        exitButton.SetActive(false);
        lobbiesMenu.SetActive(true);
        SteamLobby.instance.GetLobbyList();
    }
    public void GoBackButton()
    {
        lobbiesButton.SetActive(true);
        hostButton.SetActive(true);
        settingButton.SetActive(true);
        controlButton.SetActive(true);
        exitButton.SetActive(true);
        lobbiesMenu.SetActive(false);
        DestroyLobbies();
    }
}
