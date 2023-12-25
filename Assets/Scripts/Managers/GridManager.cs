using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int gridWidth;
    [SerializeField] private int gridHeight;
    [SerializeField] private float sizeByPixel;
    [SerializeField] private GridTileController gridMapController;

    private Grid grid;
    private GridCell currentCell;

    private BaseActor currentActor;
    private BaseActor selectedActor;

    private FactoryManager factoryManager;
 
    public void InitGrid()
    {
        factoryManager = FactoryManager.Instance;
        grid = new Grid(gridWidth, gridHeight, PixelToWorldSize(sizeByPixel));
        gridMapController.SetGridMap(gridWidth, gridHeight, PixelToWorldSize(sizeByPixel));
        GetComponent<Pathfinding>().SetGrid(grid);
    }

    //public Grid GetGrid() { return grid; }

    /// <summary>
    /// Returns the cell which cursor on
    /// </summary>
    /// <param name="cell"></param>
    /// <param name="grid"></param>
    public GridCell GetCellAccordingTheMovingCursor(Vector2 worldPos)
    {
        GridCell cell = null;

        if (currentActor != null)
            cell = grid.GetGridCell(worldPos + currentActor.GetMouseOffset(), currentActor.GetActorWidth(), currentActor.GetActorHeight());
        else
            cell = grid.GetGridCell(worldPos, 1, 1);

        return cell;
    }

    public void OnClickOnCell(GridCell cell, Vector2 worldPos)
    {
        if (currentActor == null)
        {
            if (cell.IsCellOccupied() && !grid.IsOutOfBounds(worldPos))
            {
                if (selectedActor != null)
                    selectedActor.ActorDeselected(); 

                // This actor could be building or soldier
                cell.GetOccupantActor().OnActorClickedOnBoard();
            }
        }
        else
        {
            TryToPlaceActor(cell, currentActor);
        }
    }

    private void TryToPlaceActor(GridCell gridCell, BaseActor actor)
    {
        if (actor.CheckIfCanBePlaced(grid, gridCell))
        {
            actor.SetActorLocation(grid, gridCell);
            SetCurrentActor(null);
        }
        // buraya buraya insaa edemezsin gibi bir pop-up gelebilir
    }

    public void OnCurrentCellChange(GridCell cell)
    {
        if (currentCell == cell) return;
        currentCell = cell;

        if (currentActor != null)
        {
            currentActor.ActionWhileMoving(cell);
            currentActor.CheckIfCanBePlaced(grid, cell);
        }

        if (selectedActor != null)
        {
            selectedActor.ActionWhileSelected(cell);
        }
    }

    public void SetCurrentActor(BaseActor actor)
    {
        currentActor = actor;
    }

    /// <summary>
    /// Returns the world unit according to given pixel unit
    /// </summary>
    /// <param name="cell"></param>
    /// <param name="grid"></param>
    private float PixelToWorldSize(float pixel)
    {
        return pixel / 100;
    }

    public void CreateBuilding(BuildingData buildingData)
    {
        if (currentActor != null) return;

        if (selectedActor != null)
            selectedActor.ActorDeselected();

        BaseActor actor = factoryManager.BuildingFactory.GetActor(buildingData);
        BuildingBase building = actor as BuildingBase;
        building.Init(buildingData);
        SetCurrentActor(building);
    }

    public void CreateSoldier(SoldierData soldierData)
    {
        if (currentActor != null) return;

        if (selectedActor.IsDead) return;


        // First i tried to place the soldier to the closest cell to building
        List<GridCell> emptyCells = selectedActor.GetEmptyNeighbors(1);
        GridCell emptyCell = GetEmptyCellRecursive();

        //// If there are no empty cell of the neighbor of the building, I placed it right by the cell until it finds empty cell
        //if (emptyCells.Count < 1)
        //{
        //    GridCell selectedBuildingCell = selectedActor.GetFirstOccupiedCell();
        //    emptyCell = grid.GetGridCellUntilEmpty(selectedBuildingCell.GetCellXIndex(), selectedBuildingCell.GetCellYIndex());
        //}
        //else
        //{
        //    emptyCell = selectedActor.GetEmptyNeighbors(1)[0];
        //}

        if (emptyCell == null) return;

        BaseActor actor = factoryManager.SoldierFactory.GetActor(soldierData);
        SoldierBase soldier = actor as SoldierBase;
        TryToPlaceActor(emptyCell, soldier);
    }

    private GridCell GetEmptyCellRecursive()
    {
        int degree = 1;

        return GetCell(degree);

        GridCell GetCell(int dgr)
        {
            if (grid.IsGridFull()) return null;

            List<GridCell> emptyCells = selectedActor.GetEmptyNeighbors(dgr);
            GridCell emptyCell = null;

            // If there are no empty cell of the neighbor of the building, I placed it right by the cell until it finds empty cell
            if (emptyCells.Count < 1)
            {
                //GridCell selectedBuildingCell = selectedActor.GetFirstOccupiedCell();
                //emptyCell = grid.GetGridCellUntilEmpty(selectedBuildingCell.GetCellXIndex(), selectedBuildingCell.GetCellYIndex());
                degree++;
                return GetCell(degree);
            }
            else
            {
                emptyCell = selectedActor.GetEmptyNeighbors(dgr)[0];
                return emptyCell;
            }
        }

        
    }

    public void SetSelectedActor(BaseActor actor)
    {
        this.selectedActor = actor;
    }

    public BaseActor GetSelectedActor()
    {
        return selectedActor;
    }


    public void OnRightClicked()
    {
        if ((selectedActor != null) && currentCell != null)
        {
            selectedActor.OnRightClicked(currentCell, grid);
        }
    }

    public float GetCellSize()
    {
        return PixelToWorldSize(sizeByPixel);
    }

}
