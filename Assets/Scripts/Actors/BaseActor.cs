using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

// THIS CLASS IS THE PARENT CLASS OF THE BUILDINGS AND SOLDIERS

public class BaseActor : MonoBehaviour, IPooledObject
{
    protected ActorData actorData;
    private float currentHealth;
    private float cellSize;
    private SpriteRenderer actorSprite;
    [SerializeField] private Transform actorGraphics;
    [SerializeField] private TextMeshPro actorNameText;
    [SerializeField] private GameObject fakeOutline;
    [SerializeField] private HealthBarController healthBarController;
    private List<GridCell> occupiedCells = new List<GridCell>();
    protected GameManagers gameManagers;

    private bool isDead;

    public Action OnActorDie { get; set; }
    public bool IsDead { get => isDead; }

    /// <summary>
    /// Initialises the actor acccording to given data
    /// </summary>
    /// <param name="cell"></param>
    /// <param name="grid"></param>
    public virtual void Init(ActorData actorData)
    {
        gameManagers = GameManagers.Instance;
        this.actorData = actorData;
        this.cellSize = gameManagers.GridManager.GetCellSize();
        this.currentHealth = actorData.actorHealth;
        actorGraphics.localScale = new Vector3(GetActorWidth(), GetActorHeight(), 1f);
        actorSprite = GetComponentInChildren<SpriteRenderer>();
        actorSprite.sprite = actorData.actorSprite;

        SetActorColor(actorData.actorColor);
        actorNameText.text = actorData.actorName;
        actorNameText.transform.localPosition = (new Vector2(actorData.actorWidth, actorData.actorHeight)) * cellSize / 2;

        healthBarController.SetHeathBar(actorData.actorHealth, currentHealth, 0);
    }

    public virtual void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        healthBarController.SetHeathBar(actorData.actorHealth, currentHealth, damageAmount, true);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        isDead = true;

        // I subscribed all the attacking soldiers to this event, so all of them will stop attacking
        OnActorDie?.Invoke();

        healthBarController.StopGhostText();

        SetEmptyOccupiedCells();
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
        gameManagers.GridManager.SetSelectedActor(this);
        fakeOutline.SetActive(true);
    }

    public virtual void ActorDeselected()
    {
        gameManagers.GridManager.SetSelectedActor(null);
        fakeOutline.SetActive(false);
    }

    public virtual void ActionWhileMoving(GridCell cell)
    {

    }

    public virtual void ActionWhileSelected(GridCell cell)
    {

    }

    /// <summary>
    /// Returns true if the given cell and according to size, all of the current cells are empty
    /// </summary>
    /// <param name="cell"></param>
    /// <param name="grid"></param>
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

    /// <summary>
    /// This function occupies the grid for this actor
    /// </summary>
    /// <param name="cell"></param>
    /// <param name="grid"></param>
    public void SetActorOnTheGrid(Grid grid, GridCell targetCell)
    {
        int x = targetCell.GetCellXIndex();
        int y = targetCell.GetCellYIndex();

        SetActorColor(actorData.actorColor);

        for (int i = 0; i < GetActorWidth(); i++)
        {
            for (int j = 0; j < GetActorHeight(); j++)
            {
                GridCell cell = grid.GetGridCell(x + i, y + j);
                cell.SetCellOccupation(this);
                occupiedCells.Add(cell);
            }
        }
    }

    /// <summary>
    /// When actor is removed from its place, this function clears the grid
    /// </summary>
    /// <param name="cell"></param>
    /// <param name="grid"></param>
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

    /// <summary>
    /// Returns the list of non occupied cells of the actor
    /// </summary>
    /// <param name="cell"></param>
    /// <param name="grid"></param>
    public List<GridCell> GetEmptyNeighbors(int degree, GridCell additionalCell = null)
    {
        List<GridCell> emptyNeigbrs = new List<GridCell>();

        for (int i = 0; i < occupiedCells.Count; i++)
        {
            List<GridCell> neiCells = occupiedCells[i].GetNeighborsDynamic(degree);
            for (int j = 0; j < neiCells.Count; j++)
            {
                GridCell cell = neiCells[j];
                if (!cell.IsCellOccupied() && !emptyNeigbrs.Contains(cell))
                {
                    emptyNeigbrs.Add(cell);
                }

                if (additionalCell != null)
                {
                    if (additionalCell == cell)
                        emptyNeigbrs.Add(cell);
                }
            }
        }

        return emptyNeigbrs;
    }

    /// <summary>
    /// Returns the closest empty cell closest to clicked cell
    /// </summary>
    /// <param name="cell"></param>
    /// <param name="grid"></param>
    public GridCell GetClosestEmptyCell(GridCell cell, GridCell soldierCell = null)
    {
        List<GridCell> neighbors = GetEmptyNeighbors(1, soldierCell);

        Vector2 pos = cell.GetCellPosition();
        if (neighbors.Count < 1) return null;

        float closestDist = 1000f;
        GridCell closestCell = null;
        for (int i = 0; i < neighbors.Count; i++)
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

    /// <summary>
    /// After selecting actor, this function cover what to do when right click on a cell
    /// </summary>
    /// <param name="cell"></param>
    /// <param name="grid"></param>
    public virtual void OnRightClicked(GridCell cell, Grid grid)
    {
        
    }

    //Following interface functions can be filled for specific things when pooling

    public virtual void OnObjectGetFromPool()
    {
        // When it goes to pool it should be dead because i will use this actor again
        isDead = false;
    }

    public virtual void OnObjectSendToPool()
    {
        
    }

    public virtual void OnObjectInstantiate()
    {
        
    }
}
