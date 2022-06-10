using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;
public class PlayerSetup : NetworkBehaviour
{
    public GameObject PlayerModel;
    [SerializeField]
    string remoteLayerName = "RemotePlayer";
    [SerializeField]
    string localLayerName = "LocalPlayer";
    [SerializeField]
    GameObject playerUIPrefab;
    [HideInInspector]
    public GameObject playerUIInstance;

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Game")
        {
            GameManager.instance.scene = Scene.Game;
            if (hasAuthority)
            {
                gameObject.GetComponent<PlayerInfo>().SetupPlayer();
                AssignRemoteLayer(localLayerName);
                if (transform.tag != "LocalPlayer")
                {
                    PlayerModel.SetActive(true);
                    SetTagRecursively(gameObject);
                }

                if (playerUIInstance == null)
                {
                    playerUIInstance = Instantiate(playerUIPrefab);
                    playerUIInstance.name = playerUIPrefab.name;
                    PlayerUI ui = playerUIInstance.GetComponent<PlayerUI>();
                    if (ui == null)
                        Debug.LogError("No PlayerUI component on PlayerUI prefab.");
                    ui.SetPlayer(gameObject);
                }
            }
        }
        else 
        {
            AssignRemoteLayer(remoteLayerName);
        }
    }
	void SetTagRecursively(GameObject obj)
	{
        obj.tag = "LocalPlayer";

		foreach (Transform child in obj.transform)
		{
			SetTagRecursively(child.gameObject);
		}
	}
    void SetLayerRecursively(GameObject obj,string layerName)
    {
        obj.layer = LayerMask.NameToLayer(layerName);

        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, layerName);
        }
    }
    void AssignRemoteLayer(string LayerName)
	{
        SetLayerRecursively(gameObject, LayerName);
	}
}
