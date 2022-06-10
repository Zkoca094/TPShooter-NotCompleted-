using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactherDesign : MonoBehaviour
{
    public Mesh[] sharedMesh;
    public SkinnedMeshRenderer meshrenderer;
    public Transform hair, beard;
    public Transform helmerHolder, weaponHolder, ArmorHolder, BagHolder,aimTransform;
    
    public Transform GethelmerHolder()
    {
        return helmerHolder;
    }
    public Transform GetweaponHolder()
    {
        return weaponHolder;
    }
    public Transform GetArmorHolder()
    {
        return ArmorHolder;
    }
    public Transform GetBagHolder()
    {
        return BagHolder;
    }
    public Transform GetaimTransform()
    {
        return aimTransform;
    }
    public void Design(Gender gender, int charactherNumber, GameObject hairprefab, GameObject beardprefab, Material material)
    {
        meshrenderer.sharedMesh = sharedMesh[charactherNumber];
        meshrenderer.material = material;

        if (hair.childCount > 0)
        {
            for (int i = 0; i < hair.childCount; i++)
            {
                Destroy(hair.GetChild(i).gameObject);
            }
        }

        Instantiate(hairprefab, hair);

        if (beardprefab == null)
            return;

        if (beard.childCount > 0)
        {
            for (int i = 0; i < beard.childCount; i++)
            {
                Destroy(beard.GetChild(i).gameObject);
            }
        }
        Instantiate(beardprefab, beard);
    }
}
