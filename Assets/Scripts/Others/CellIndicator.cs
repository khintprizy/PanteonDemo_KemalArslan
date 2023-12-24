using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellIndicator : MonoBehaviour
{
    [SerializeField] private SpriteRenderer indicatorRenderer;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Color freeCellColor;
    [SerializeField] private Color attackCellColor;

    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite attackSprite;

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

    private void OnCellChange(GridCell currentCell, GridCell newCell)
    {
        if (newCell == null) return;

        indicatorRenderer.gameObject.SetActive(true);
        transform.position = newCell.GetCellPosition();

        if (newCell.IsCellOccupied())
        {
            indicatorRenderer.color = attackCellColor;
            lineRenderer.gameObject.SetActive(false);
            indicatorRenderer.sprite = attackSprite;
        }
        else
        {

            indicatorRenderer.color = freeCellColor;
            SetLineRenderer(currentCell, newCell);

            indicatorRenderer.sprite = normalSprite;
        }
    }

    private void SetLineRenderer(GridCell currentCell, GridCell newCell)
    {
        List<GridCell> cells = pathfinder.FindPath(currentCell, newCell);

        if (cells == null)
        {
            indicatorRenderer.color = Color.red;
            return;
        }

        lineRenderer.gameObject.SetActive(true);

        cells.Insert(0, currentCell);

        lineRenderer.positionCount = cells.Count;

        Vector3[] poses = new Vector3[cells.Count];

        for (int i = 0; i < poses.Length; i++)
        {
            poses[i] = cells[i].GetMiddlePointOfTheCell();
        }

        lineRenderer.SetPositions(poses);
    }

    private void OnSoldierStartToMove()
    {
        indicatorRenderer.gameObject.SetActive(false);
        lineRenderer.gameObject.SetActive(false);
    }

    private void OnBuildingSelected()
    {
        indicatorRenderer.gameObject.SetActive(false);
        lineRenderer.gameObject.SetActive(false);
    }
}
