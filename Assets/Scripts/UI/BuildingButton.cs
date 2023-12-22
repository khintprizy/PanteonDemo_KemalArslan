using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingButton : MonoBehaviour
{
    private BuildingData buildingData;
    private GridManager gridManager;

    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI textMeshProUGUI;

    public void Init(BuildingData buildingData)
    {
        gridManager = GameManagers.Instance.GridManager;
        this.buildingData = buildingData;

        image.color = buildingData.actorColor;
        textMeshProUGUI.text = buildingData.actorName;

        GetComponentInChildren<Button>().onClick.AddListener(CreateBuilding);
    }

    private void CreateBuilding()
    {
        gridManager.CreateBuilding(buildingData);
    }
}
