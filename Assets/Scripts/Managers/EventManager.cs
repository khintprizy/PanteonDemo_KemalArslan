using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public Action<GridCell> OnCellOverWhileSoldierSelected { get; set; }
    public Action OnSoldierStartToMove { get; set; }
    public Action OnBuildingSelected { get; set; }
}
