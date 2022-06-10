using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public InventoryObject playerInventory;
    public Transform helmetHolder, vestHolder, bagHolder, weaponHolder;
    private Collider other;
    [Header("Player Grounded")]
    [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
    public bool Grounded = false;
    [Tooltip("Useful for rough ground")]
    public float GroundedOffset = -0.14f;
    [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
    public float GroundedRadius = 0.28f;
    [Tooltip("What layers the character uses as ground")]
    public LayerMask GroundLayers;
    private void Start()
    {
        CharactherDesign characther = GetComponentInChildren<CharactherDesign>();
        helmetHolder = characther.GethelmerHolder();
        vestHolder = characther.GetArmorHolder();
        bagHolder = characther.GetBagHolder();
        weaponHolder = characther.GetweaponHolder();
    }
    private void GroundedItemCheck()
    {
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
        Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);
        if (Grounded)
        {
            var zone = Physics.OverlapSphere(transform.position, GroundedRadius);
            foreach (var item in zone)
            {
                if(item.transform.tag == "Item")
                {
                    Debug.Log("Press E to pick up the item");
                    other = item;
                    if (Input.GetKeyDown(KeyCode.E) && other != null)
                    {
                        PickUp(other);
                    }
                }
            }
        }
    }
    public void PickUp(Collider other)
    {
        ItemInfo item = other.GetComponent<ItemInfo>();
        ItemObject data = item.item[item.level];
        playerInventory.AddItem(data.data, 1, item.level);
        EquipItem(item);
        Destroy(other.gameObject);
        other = null;
        Grounded = false;
    }

    private void Update()
    {
        GroundedItemCheck();
    }

    public void EquipItem(ItemInfo itemInfo)
    {
        ItemObject item = itemInfo.item[itemInfo.level];
        GameObject display;
        switch (item.type)
        {
            case ItemType.Weapon:
                display = Instantiate(item.characterDisplay, transform.position, Quaternion.identity);
                display.transform.SetParent(weaponHolder);
                display.transform.position = Vector3.zero;
                display.transform.localScale = Vector3.one;
                break;
            case ItemType.Helmet:
                display = Instantiate(item.characterDisplay, transform.position, Quaternion.identity);
                display.transform.SetParent(helmetHolder);
                display.transform.position = Vector3.zero;
                display.transform.localScale = Vector3.one;
                break;
            case ItemType.Vest:
                display = vestHolder.GetChild(itemInfo.level).gameObject;
                display.SetActive(true);
                break;
            case ItemType.Backpack:
                display = Instantiate(item.characterDisplay, transform.position, Quaternion.identity);
                display.transform.SetParent(bagHolder);
                display.transform.position = Vector3.zero;
                display.transform.localScale = Vector3.one;
                break;
        }

    }
}
