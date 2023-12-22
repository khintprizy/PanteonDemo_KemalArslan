using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

// THIS CLASS IS THE PARENT CLASS OF THE BUILDINGS AND SOLDIERS

public class BaseActor : MonoBehaviour
{
    protected ActorData actorData;
    private float currentHealth;
    private float cellSize;
    private SpriteRenderer actorSprite;
    [SerializeField] private Transform actorGraphics;
    [SerializeField] private TextMeshPro actorNameText;
    private List<GridCell> occupiedCells = new List<GridCell>();
    protected GameManagers gameManagers;

    public Action OnActorDie { get; set; }

    public virtual void Init(ActorData actorData, float cellSize)
    {
        this.actorData = actorData;
        this.cellSize = cellSize;
        this.currentHealth = actorData.actorHealth;
        actorGraphics.localScale = new Vector3(GetActorWidth(), GetActorHeight(), 1f);
        actorSprite = GetComponentInChildren<SpriteRenderer>();

        SetActorColor(actorData.actorColor);
        actorNameText.text = actorData.actorName;
        actorNameText.transform.localPosition = (new Vector2(actorData.actorWidth, actorData.actorHeight)) * cellSize / 2;
        gameManagers = GameManagers.Instance;
    }

    public virtual void TakeDamage(float damageAmount, Action callback)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0 )
        {
            callback?.Invoke();
            Die();
        }
    }

    protected virtual void Die()
    {
        //OnActorDie?.Invoke();

        SetEmptyOccupiedCells();
        Destroy(gameObject);
    }

    public virtual ActorData GetActorData()
    {
        return actorData;
    }

    public int GetActorWidth()
    {
        return actorData.actorWidth;
    }
    public int GetActorHeight()
    {
        return actorData.actorHeight;
    }

    public Vector2 GetMouseOffset()
    {
        // this offset used while moving building on the grid to center to the mouse cursor

        float x = ((float)GetActorWidth() / 2) - .5f;
        float y = ((float)GetActorHeight() / 2) - .5f;
        return -cellSize * (new Vector2(x, y));
    }

    public virtual void OnActorClickedOnBoard()
    {

    }

    public virtual void ActionWhileMoving(GridCell cell)
    {

    }

    public virtual void ActionWhileSelected(GridCell cell)
    {

    }

    public bool CheckIfCanBePlaced(Grid grid, GridCell targetCell)
    {
        bool canBePlaced = true;

        int currentXIndex = targetCell.GetCellXIndex();
        int currentYIndex = targetCell.GetCellYIndex();

        for (int i = 0; i < GetActorWidth(); i++)
        {
            for (int j = 0; j < GetActorHeight(); j++)
            {
                GridCell cell = grid.GetGridCell(currentXIndex + i, currentYIndex + j);
                canBePlaced = !cell.IsCellOccupied();

                if (!canBePlaced)
                {
                    SetIndicator(Color.red);
                    return canBePlaced;
                }
            }
        }

        SetIndicator(Color.green);
        return canBePlaced;
    }

    public virtual void SetActorLocation(Grid grid, GridCell targetCell)
    {
        
    }

    public void SetActorOnTheGrid(Grid grid, int currentXIndex, int currentYIndex)
    {
        SetActorColor(actorData.actorColor);

        for (int i = 0; i < GetActorWidth(); i++)
        {
            for (int j = 0; j < GetActorHeight(); j++)
            {
                GridCell cell = grid.GetGridCell(currentXIndex + i, currentYIndex + j);
                cell.SetCellOccupation(this);
                occupiedCells.Add(cell);
            }
        }
    }

    protected void SetEmptyOccupiedCells()
    {
        for (int i = 0; i < occupiedCells.Count; i++)
        {
            occupiedCells[i].SetCellOccupation(null);
        }

        occupiedCells.Clear();
    }

    protected virtual void SetIndicator(Color color)
    {

    }

    protected void SetActorColor(Color color)
    {
        actorSprite.color = color;
    }

    public GridCell GetFirstOccupiedCell()
    {
        return occupiedCells[0];
    }

    public List<GridCell> GetEmptyNeighbors()
    {
        List<GridCell> emptyNeigbrs = new List<GridCell>();

        for (int i = 0; i < occupiedCells.Count; i++)
        {
            List<GridCell> neiCells = occupiedCells[i].GetNeighbours();
            for (int j = 0; j < neiCells.Count; j++)
            {
                GridCell cell = neiCells[j];
                if (!cell.IsCellOccupied() && !emptyNeigbrs.Contains(cell))
                {
                    emptyNeigbrs.Add(cell);
                }
            }
        }

        return emptyNeigbrs;
    }

    public GridCell GetClosestEmptyCell(GridCell cell)
    {
        List<GridCell> neighbors = GetEmptyNeighbors();

        Vector2 pos = cell.GetCellPosition();
        if (neighbors.Count < 1) return null;

        float closestDist = 1000f;
        GridCell closestCell = null;
        for (int i = 0;i < neighbors.Count;i++)
        {
            float dist = Vector2.Distance(neighbors[i].GetCellPosition(), pos);
            if (dist < closestDist)
            {
                closestDist = dist;
                closestCell = neighbors[i];
            }
        }
        return closestCell;
    }

    public virtual void OnRightClicked(GridCell cell, Grid grid)
    {

    }
}
