using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHealth : MonoBehaviour
{
    private int currentHealth;
    private bool isDead;

    [SerializeField]
    private int fallingItemNumber;

    public Transform[] itemSpawner;
    public Transform itemPrefab;

    [SerializeField]
    private GameObject hiteffectPrefab;
    private void Start()
    {
        fallingItemNumber = Random.Range(1, 5);
        isDead = false;
    }
    private void FixedUpdate()
    {
        if (isDead)
        {
            ItemSpawner();
        }
    }
    public void TakeDamage(int _amount, Vector3 _pos, Vector3 _normal)
    {
        if (isDead)
            return;

        currentHealth -= _amount;
        
        if (currentHealth <= 0)
            isDead = true;

        GameObject _hitEffect = Instantiate(hiteffectPrefab, _pos, Quaternion.LookRotation(_normal));
        Destroy(_hitEffect.gameObject, 2f);
    }

    void ItemSpawner()
    {
        for (int i = 0; i < fallingItemNumber; i++)
        {
            Transform _wood= Instantiate(itemPrefab, itemSpawner[i]);
            _wood.GetComponent<Collider>().isTrigger = false;
            _wood.SetParent(null);
        }
        isDead = false;
        Destroy(gameObject, 0.5f);
    }
}
