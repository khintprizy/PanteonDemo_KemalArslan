using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell
{
    private Grid grid;
    private Vector2 cellPosition;
    private BaseActor occupantActor;
    private int cellXIndex;
    private int cellYIndex;

    private int gCost;
    private int hCost;

    private GridCell parentCell;

    List<GridCell> neighbors = new List<GridCell>();

    public int FCost { get { return GCost + HCost; } }

    public int GCost { get => gCost; set => gCost = value; }
    public int HCost { get => hCost; set => hCost = value; }
    public GridCell ParentCell { get => parentCell; set => parentCell = value; }

    public GridCell(Vector2 cellPosition, int cellXIndex, int cellYIndex, Grid grid)
    {
        // constructor of the grid cell

        this.cellPosition = cellPosition;
        this.cellXIndex = cellXIndex;
        this.cellYIndex = cellYIndex;
        this.grid = grid;
    }

    public Vector2 GetCellPosition()
    {
        return cellPosition;
    }

    public Vector2 GetMiddlePointOfTheCell()
    {
        return GetCellPosition() + new Vector2(.16f, .16f);
    }

    public BaseActor GetOccupantActor()
    {
        return occupantActor;
    }

    public void SetNeighbours()
    {
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0) continue;

                int checkX = GetCellXIndex() + i;
                int checkY = GetCellYIndex() + j;

                if (checkX >= 0 && checkX < grid.GetGridWidth() && checkY >= 0 && checkY < grid.GetGridHeight())
                    neighbors.Add(grid.GetGridCell(checkX, checkY));
            }
        }
    }

    public List<GridCell> GetNeighbours()
    {
        return neighbors;
    }

    public bool IsCellOccupied() { return occupantActor != null; }
    public void SetCellOccupation(BaseActor occupant) { occupantActor = occupant; }

    public int GetCellXIndex() {  return cellXIndex; }
    public int GetCellYIndex() {  return cellYIndex; }
}
