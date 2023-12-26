using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoldierBase : BaseActor
{
    private Coroutine attackCr;
    SoldierData soldierData;

    private BaseActor targetActor;

    private SoldierFactory factory;

    public override void Init(ActorData actorData)
    {
        base.Init(actorData);

        soldierData = actorData as SoldierData;
        factory = FactoryManager.Instance.SoldierFactory;
    }

    public override void SetActorLocation(Grid grid, GridCell targetCell)
    {
        base.SetActorLocation(grid, targetCell);

        transform.position = targetCell.GetCellPosition();

        SetActorOnTheGrid(grid, targetCell);
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

        if (path == null)
        {
            ActorDeselected();
            yield break;
        }

        List<GridCell> tempCells = new List<GridCell>();

        for (int i = 0; i < path.Count; i++)
        {
            tempCells.Add(path[i]);
        }

        gameManagers.EventManager.OnSoldierStartToMove?.Invoke();

        

        // I set current cell empty and targetcell occupied
        SetEmptyOccupiedCells();

        //SetActorOnTheGrid(grid, targetCell.GetCellXIndex(), targetCell.GetCellYIndex());


        // With this loop, we move to cell after cell of the path
        // I used Coroutine for a smooth movement
        for (int i = 0; i < path.Count; i++)
        {
            Vector2 start = transform.position;
            Vector2 destination = path[i].GetCellPosition();

            float speed = (destination - start).magnitude;

            float t = 0;

            path[i].SetCellOccupation(this);

            while (t < 1)
            {
                t = t + ((Time.deltaTime / speed) * soldierData.movementSpeed);
                transform.position = Vector2.Lerp(start, destination, t);
                yield return null;
            }

            path[i].SetCellOccupation(null);

            tempCells.Remove(path[i]);

            if (CheckIfPathIsOccupied(tempCells))
            {
                tempCells.Clear();

                if (targetCell.IsCellOccupied())
                {

                    SetActorOnTheGrid(grid, path[i]);

                    BaseActor actor = targetCell.GetOccupantActor();
                    GridCell neighborCell = actor.GetClosestEmptyCell(path[i]);

                    StartTweenMovement(grid, neighborCell, null);

                    yield break;
                }

                SetActorOnTheGrid(grid, path[i]);
                StartTweenMovement(grid, targetCell, targetActor);

                yield break;
            }
        }

        SetActorOnTheGrid(grid, targetCell);

        // I will start AttackCoroutine here
        // I will hold my coroutine in a variable so i can cancel it if attacking stops

        if (targetActor != null)
            StartAttacking(targetActor);
    }

    private bool CheckIfPathIsOccupied(List<GridCell> path)
    {
        for (int i = 0; i < path.Count; i++)
        {
            if (path[i].IsCellOccupied()) return true;
        }

        return false;
    }

    IEnumerator AttackCr(BaseActor targetActor)
    {
        if (targetActor.IsDead) yield break;
        targetActor.TakeDamage(soldierData.attackPower);

        yield return new WaitForSeconds(soldierData.attackSpeed);

        if (targetActor.IsDead) yield break;
        attackCr = StartCoroutine(AttackCr(targetActor));
    }

    private void StartAttacking(BaseActor targetActor)
    {
        this.targetActor = targetActor;

        // I am adding StopAttacking function to target's Die event so soldier will stop attacking when target dies
        targetActor.OnActorDie += StopAttacking;

        attackCr = StartCoroutine(AttackCr(targetActor));
    }

    private void StopAttacking()
    {
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

        factory.SendObjectToPool(gameObject, actorData.poolType);
    }

    public override void OnActorClickedOnBoard()
    {
        base.OnActorClickedOnBoard();

        // I am closing the info panel when soldier is selected
        UIManager.Instance.GetInformationMenuController().SetThePanel(false);
    }


    public override void OnRightClicked(GridCell cell, Grid grid)
    {
        if (cell == GetFirstOccupiedCell())
        {
            Debug.Log("You cannot attack yourself!");
            return;
        }

        base.OnRightClicked(cell, grid);

        // If soldier is attacking, he/she will cancel it
        StopAttacking();

        if (cell.IsCellOccupied())
        {
            // take the clicked actor, take the closest neighbor, move there and attack
            BaseActor targetActor = cell.GetOccupantActor();

            GridCell closestCell = targetActor.GetClosestEmptyCell(cell, GetFirstOccupiedCell());

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

        // Triggers the event for indicator to show cells status
        gameManagers.EventManager.OnCellOverWhileSoldierSelected?.Invoke(GetFirstOccupiedCell(), cell);
    }
}
