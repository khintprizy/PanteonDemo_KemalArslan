using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagers : SingletonMaker<GameManagers>
{
    [SerializeField] private Pathfinding pathfinder;
    [SerializeField] private GridManager gridManager;
    [SerializeField] private EventManager eventManager;

    public Pathfinding Pathfinder { get => pathfinder; }
    public GridManager GridManager { get => gridManager; }
    public EventManager EventManager { get => eventManager; }
}
