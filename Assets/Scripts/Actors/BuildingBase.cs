using UnityEngine;

public class BuildingBase : BaseActor
{
    public override void Init(ActorData actorData)
    {
        base.Init(actorData);
    }

    public override void SetActorLocation(Grid grid, GridCell targetCell)
    {
        base.SetActorLocation(grid, targetCell);

        transform.position = targetCell.GetCellPosition();

        SetActorOnTheGrid(grid, targetCell.GetCellXIndex(), targetCell.GetCellYIndex());
        ActorDeselected();
    }

    public override void OnActorClickedOnBoard()
    {
        base.OnActorClickedOnBoard();
        UIManager.Instance.GetInformationMenuController().SetInfoMenu(this);
        
        gameManagers.EventManager.OnBuildingSelected?.Invoke();
    }

    public void PickBuildingFromGrid()
    {
        gameManagers.GridManager.SetCurrentActor(this);
        SetEmptyOccupiedCells();
    }

    public override void ActionWhileMoving(GridCell cell)
    {
        base.ActionWhileMoving(cell);

        transform.position = cell.GetCellPosition();
    }

    protected override void SetIndicator(Color color)
    {
        base.SetIndicator(color);
        SetActorColor(color);
    }

    public override void OnRightClicked(GridCell cell, Grid grid)
    {
        base.OnRightClicked(cell, grid);
        Debug.Log("Buildings can not attack!");
    }
}
