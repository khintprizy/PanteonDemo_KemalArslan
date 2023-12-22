using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoldierBase : BaseActor
{
    private Coroutine attackCr;
    SoldierData soldierData;

    private BaseActor targetActor;

    public override void Init(ActorData actorData, float cellSize)
    {
        base.Init(actorData, cellSize);

        soldierData = actorData as SoldierData;
    }

    public override void SetActorLocation(Grid grid, GridCell targetCell)
    {
        base.SetActorLocation(grid, targetCell);

        transform.position = targetCell.GetCellPosition();

        SetActorOnTheGrid(grid, targetCell.GetCellXIndex(), targetCell.GetCellYIndex());
    }

    private void StartTweenMovement(Grid grid, GridCell targetCell, BaseActor targetActor)
    {
        StartCoroutine(TweenMovementCr(grid, targetCell, targetActor));
    }

    IEnumerator TweenMovementCr(Grid grid, GridCell targetCell, BaseActor targetActor)
    {
        GridCell startCell = GetFirstOccupiedCell();

        // I got the path here
        List<GridCell> path = gameManagers.Pathfinder.FindPath(startCell, targetCell);

        gameManagers.EventManager.OnSoldierStartToMove?.Invoke();

        if (path == null)
        {
            ActorDeselected();
            yield break;
        }

        // I set current cell empty and targetcell occupied
        SetEmptyOccupiedCells();
        SetActorOnTheGrid(grid, targetCell.GetCellXIndex(), targetCell.GetCellYIndex());


        // With this loop, we move to cell after cell of the path
        // I used Coroutine for a smooth movement
        for (int i = 0; i < path.Count; i++)
        {
            Vector2 start = transform.position;
            Vector2 destination = path[i].GetCellPosition();

            float speed = (destination - start).magnitude;

            float t = 0;

            while (t < 1)
            {
                t = t + ((Time.deltaTime / speed) * soldierData.movementSpeed);
                transform.position = Vector2.Lerp(start, destination, t);
                yield return null;
            }
        }

        // I will start AttackCoroutine here
        // I will hold my coroutine in a variable so i can cancel it if attacking stops

        if (targetActor != null)
            StartAttacking(targetActor);
    }

    IEnumerator AttackCr(BaseActor targetActor)
    {
        if (targetActor == null) yield break;
        targetActor.TakeDamage(soldierData.attackPower);

        yield return new WaitForSeconds(soldierData.attackSpeed);

        if (targetActor == null) yield break;
        attackCr = StartCoroutine(AttackCr(targetActor));
    }

    private void StartAttacking(BaseActor targetActor)
    {
        this.targetActor = targetActor;
        targetActor.OnActorDie += StopAttacking;
        attackCr = StartCoroutine(AttackCr(targetActor));
    }

    private void StopAttacking()
    {
        //targetActor.OnActorDie -= StopAttacking;
        if (attackCr != null)
        {
            StopCoroutine(attackCr);
            attackCr = null;
        }
    }

    protected override void Die()
    {
        base.Die();

        if (targetActor != null)
        {
            targetActor.OnActorDie -= StopAttacking;
        }
    }

    public override void OnActorClickedOnBoard()
    {
        base.OnActorClickedOnBoard();

        UIManager.Instance.GetInformationMenuController().SetThePanel(false);
    }

    public override void OnRightClicked(GridCell cell, Grid grid)
    {
        base.OnRightClicked(cell, grid);

        // If soldier is attacking, he/she will cancel it
        StopAttacking();

        if (cell.IsCellOccupied())
        {
            // take the clicked actor, take the closest neighbor, move there and attack
            BaseActor targetActor = cell.GetOccupantActor();

            GridCell closestCell = targetActor.GetClosestEmptyCell(cell);

            if (closestCell == null) return;

            StartTweenMovement(grid, closestCell, targetActor);
            ActorDeselected();
        }
        else
        {
            StartTweenMovement(grid, cell, null);
            ActorDeselected();
        }
    }

    public override void ActionWhileSelected(GridCell cell)
    {
        base.ActionWhileSelected(cell);
        gameManagers.EventManager.OnCellOverWhileSoldierSelected?.Invoke(cell);
    }
}
