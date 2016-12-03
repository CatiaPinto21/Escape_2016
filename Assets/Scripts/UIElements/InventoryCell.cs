﻿using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using PixelCrushers.DialogueSystem;

public class InventoryCell : MonoBehaviour
{
    private Inventory m_inventory;

    void Start()
    {
        m_inventory = transform.parent.parent.GetComponent<Inventory>();
        Assert.IsNotNull(m_inventory);
    }

    public void ToggleClick()
    {
        switch (m_inventory.m_eMode)
        {
            case Inventory.EInventoryMode.e_Crafting:
                SetItemToCrafting();
                break;

            case Inventory.EInventoryMode.e_Solo:
                UseItem();
                break;

            default:
                Debug.LogError("Undefined inventory mode");
                return;
        }
    }

    private void SetItemToCrafting()
    {
        bool isOn = transform.GetComponent<UnityEngine.UI.Toggle>().isOn;
        if (transform.childCount > 0)
        {
            Assert.AreEqual(transform.childCount, 1);
            transform.GetComponentInChildren<InventoryItem>().related3DObject.SetToCrafting(isOn);
        }
    }

    private void UseItem()
    {
        if (!transform.GetComponent<UnityEngine.UI.Toggle>().isOn || transform.childCount == 0)
            return;

        Assert.AreEqual(transform.childCount, 1);
        if (m_inventory.m_unLockAction.unlockItem.Equals(GetComponentInChildren<InventoryItem>()))
        {
            //Unlock
            m_inventory.m_unLockAction.Unlock();
            m_inventory.m_unLockAction = null;
            //Remove item from inventory
            transform.GetComponentInChildren<InventoryItem>().SetInInventory(false);
            // Hide inventory
            m_inventory.Hide();
            // Broadcast
            DialogueManager.Instance.gameObject.BroadcastMessage("OnInventoryHide", gameObject, SendMessageOptions.DontRequireReceiver);
        }
        else
        {
            DialogueManager.ShowAlert("Cet objet ne peut pas être utilisé ici.");
        }
    }
}