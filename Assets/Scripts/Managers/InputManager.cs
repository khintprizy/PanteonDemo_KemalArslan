using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    [SerializeField] private Camera mainCam;
    private GridManager gridManager;
    private Vector2 mouseWorldPosition;

    [SerializeField] private SoldierData testSoldier;

    private void Start()
    {
        gridManager = GetComponent<GridManager>();
    }

    private void Update()
    {
        mouseWorldPosition = mainCam.ScreenToWorldPoint(Input.mousePosition);
        GridCell cell = gridManager.GetCellAccordingTheMovingCursor(mouseWorldPosition);

        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;
            gridManager.OnClickOnCell(cell, mouseWorldPosition);
        }

        gridManager.OnCurrentCellChange(cell);

        if (Input.GetMouseButtonDown(1))
        {
            gridManager.OnRightClicked();
        }
    }
}
