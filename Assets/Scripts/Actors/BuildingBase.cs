using UnityEngine;

public class BuildingBase : BaseActor
{
    private BuildingFactory factory;

    public override void Init(ActorData actorData)
    {
        base.Init(actorData);
        factory = FactoryManager.Instance.BuildingFactory;
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
        if (IsDead) return;
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

    protected override void Die()
    {
        base.Die();
        factory.SendObjectToPool(gameObject, actorData.poolType);
    }
}
