using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Steamworks;
using Mirror;
public enum Gender
{
    Woman,
    Man
}

public class CharacterCustomization : NetworkBehaviour
{
    private Transform currentCharacter, CharacterContain;     
    private SkinnedMeshRenderer Meshrenderer;
    private string Character_Name;
    private int hairChildCount = 0;
    private CustomNetworkManager manager;
    [SerializeField] private Transform FCharacterContain, MCharacterContain, HairContain, BeardContain;
    [SerializeField] private Material[] materialsLightSkin;
    [SerializeField] private Material[] materialsMediumSkin;
    [SerializeField] private Material[] materialsDarkSkin;
    [SerializeField] private Text CharacterNameText;    
    [SerializeField] private Gender gender;
    [SerializeField] public GameObject[] FemaleHairPrefab, MaleHairPrefab, MaleBeardPrefab;
    [SerializeField] private int charactherNumber, skinNumber, clothesNumber,hairNumber, beardNumber;
    [SerializeField] private GameObject beardNextbutton, beardPrevButton;
    private CustomNetworkManager Manager
    {
        get
        {
            if (manager != null)
                return manager;
            return manager = CustomNetworkManager.singleton as CustomNetworkManager;
        }

    }
    public string GetName
    {
        get { return Character_Name; }
        set { Character_Name = value; }
    }
    public void SetDefaults()
    {
        if (gender == Gender.Man)
        {
            CharacterContain = MCharacterContain;
            FCharacterContain.gameObject.SetActive(false);
            MCharacterContain.gameObject.SetActive(true);
            BeardContain.gameObject.SetActive(true);
            beardNextbutton.SetActive(true);
            beardPrevButton.SetActive(true);
            hairChildCount = FemaleHairPrefab.Length - 1;
            SetBeard(0);
        }
        else
        {
            beardNextbutton.SetActive(false);
            beardPrevButton.SetActive(false);
            CharacterContain = FCharacterContain;
            MCharacterContain.gameObject.SetActive(false);
            BeardContain.gameObject.SetActive(false);
            FCharacterContain.gameObject.SetActive(true);
            hairChildCount = MaleHairPrefab.Length - 1;
        }
        SetCharacter(2, 0, 0);
        SetHair(0);
    }
    void Start()
    {
        SetDefaults();
    }
    void SetCharacter(int a, int b, int c)
    {
        for (int i = 2; i < CharacterContain.childCount; i++)
        {
            CharacterContain.GetChild(i).gameObject.SetActive(false);
        }
        charactherNumber = a;
        skinNumber = b;
        clothesNumber = c;
        currentCharacter = CharacterContain.GetChild(charactherNumber);
        currentCharacter.gameObject.SetActive(true);
        Meshrenderer = currentCharacter.GetComponent<SkinnedMeshRenderer>();
        CharacterNameText.text = currentCharacter.name;
        switch (skinNumber)
        {
            case 0:
                Meshrenderer.material = materialsLightSkin[clothesNumber];
                break;
            case 1:
                Meshrenderer.material = materialsMediumSkin[clothesNumber];
                break;
            case 2:
                Meshrenderer.material = materialsDarkSkin[clothesNumber];
                break;
        }
    }
    public void NextBtn()
    {
        if (charactherNumber < CharacterContain.childCount - 1)
            charactherNumber++;

        SetCharacter(charactherNumber, skinNumber, clothesNumber);
    }
    public void PrevBtn()
    {
        if (charactherNumber > 2)
            charactherNumber--;

        SetCharacter(charactherNumber, skinNumber, clothesNumber);
    }
    public void ClothesBtn(int value)
    {
        clothesNumber = value;
        SetCharacter(charactherNumber, skinNumber, clothesNumber);
    }
    public void SkinBtn(int value)
    {
        skinNumber = value;
        SetCharacter(charactherNumber, skinNumber, clothesNumber);
    }
    public void ChangeName()
    {
        currentCharacter.name = Character_Name;
        CharacterNameText.text = currentCharacter.name;
    }
    public void HairNextBtn()
    {
        if (hairNumber < hairChildCount - 1)
        {
            hairNumber++;
        }
        SetHair(hairNumber);
    }    
    public void HairPrevBtn()
    {
        if (hairNumber > 0)
        {
            hairNumber--;
        }
        SetHair(hairNumber);
    }
    public void SetHair(int number)
    {
        for (int i = 0; i < HairContain.childCount; i++)
        {
            Destroy(HairContain.GetChild(i).gameObject);
        }
        if (gender == Gender.Woman)
        {
            Instantiate(FemaleHairPrefab[number], HairContain);
        }
        else
        {
            Instantiate(MaleHairPrefab[number], HairContain);
        }
    }
    public void BeardNextBtn()
    {
        if (beardNumber < MaleBeardPrefab.Length - 1)
        {
            beardNumber++;
        }
        SetBeard(beardNumber);
    }
    public void BeardPrevBtn()
    {
        if (beardNumber > 0)
        {
            beardNumber--;
        }
        SetBeard(beardNumber);
    }
    public void SetBeard(int number)
    {
        for (int i = 0; i < BeardContain.childCount; i++)
        {
            Destroy(BeardContain.GetChild(i).gameObject);
        }
        Instantiate(MaleBeardPrefab[number], BeardContain);
    }
    public void GenderWomanBtn()
    {
        SetGender(Gender.Woman);
    }
    public void GenderManBtn()
    {
        SetGender(Gender.Man);
    }
    public void SetGender(Gender newgender)
    {
        gender = newgender;
        SetDefaults();
    }    
    public void ConfirmBtn()
    {
        GameObject prefab, player = null;
        PlayerObjectController playerObjectController = GameObject.Find("LocalGamePlayer").transform.GetComponent<PlayerObjectController>();
        if (playerObjectController.Ready)
        {
            if (GameObject.Find("LocalGamePlayer").transform.GetChild(0).childCount > 0)
                Destroy(GameObject.Find("LocalGamePlayer").transform.GetChild(0).GetChild(0).gameObject);
            if (gender == Gender.Woman)
                prefab = (GameObject)Resources.Load("Character_Female");
            else
                prefab = (GameObject)Resources.Load("Character_Male");
            if (prefab != null)
                player = Instantiate(prefab);

            player.transform.SetParent(GameObject.Find("LocalGamePlayer").transform.GetChild(0));
            player.transform.position = Vector3.zero;
            player.transform.rotation = Quaternion.Euler(Vector3.zero);
            player.transform.localScale = Vector3.one;
            if (gender == Gender.Man)
                player.GetComponent<CharactherDesign>().Design(gender, charactherNumber, MaleHairPrefab[hairNumber], MaleBeardPrefab[beardNumber], Meshrenderer.material);
            else
                player.GetComponent<CharactherDesign>().Design(gender, charactherNumber, FemaleHairPrefab[hairNumber], null, Meshrenderer.material);
        }
    }
}
