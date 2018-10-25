using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryButtonObject : MonoBehaviour
{

    public InventoryScript IS;

    private void Update()
    {
        GetTouch();
    }

    public void GetTouch()
    {
        if (IS.inventoryPanel.activeSelf) { return; }

        Touch[] Touches = Input.touches;

        Vector2 touchPos = Vector2.zero;
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            touchPos = Input.mousePosition;
        }
        else
        if (Touches.Length > 0)
        {
            touchPos = Touches[0].position;
        }
        else
        {
            return;
        }

        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(touchPos), Vector2.zero);

        if (hit && hit.transform.tag == "Giant")
        {
            IS.inventoryPanel.SetActive(true);
            IS.HUDPanel.SetActive(false);
            IS.UpdateEggUI(IS.EggUI);
            IS.UpdateSeedUI(IS.SeedUI);
        }
    }
}
