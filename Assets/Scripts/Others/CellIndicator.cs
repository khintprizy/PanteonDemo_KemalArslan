using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellIndicator : MonoBehaviour
{
    [SerializeField] private SpriteRenderer indicatorRenderer;
    [SerializeField] private Color freeCellColor;
    [SerializeField] private Color attackCellColor;

    private GameManagers gameManagers;
    private Pathfinding pathfinder;

    private void Start()
    {
        gameManagers = GameManagers.Instance;
        gameManagers.EventManager.OnCellOverWhileSoldierSelected += OnCellChange;
        gameManagers.EventManager.OnSoldierStartToMove += OnSoldierStartToMove;
        gameManagers.EventManager.OnBuildingSelected += OnBuildingSelected;

        pathfinder = gameManagers.Pathfinder;
    }

    private void OnCellChange(GridCell newCell)
    {
        if (newCell == null) return;

        indicatorRenderer.gameObject.SetActive(true);
        transform.position = newCell.GetCellPosition();

        if (newCell.IsCellOccupied())
        {
            indicatorRenderer.color = attackCellColor;
        }
        else
        {
            indicatorRenderer.color = freeCellColor;
        }
    }

    private void OnSoldierStartToMove()
    {
        indicatorRenderer.gameObject.SetActive(false);
    }

    private void OnBuildingSelected()
    {
        indicatorRenderer.gameObject.SetActive(false);
    }
}
