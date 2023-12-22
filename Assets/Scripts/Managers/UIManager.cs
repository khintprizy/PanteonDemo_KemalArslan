using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : SingletonMaker<UIManager>
{
    [SerializeField] private ProductionMenuController productionMenuController;
    [SerializeField] private InformationMenuController informationMenuController;

    public ProductionMenuController GetProductionMenuController() { return productionMenuController; }
    public InformationMenuController GetInformationMenuController() { return informationMenuController; }
}
