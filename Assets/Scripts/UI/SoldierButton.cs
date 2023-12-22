using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SoldierButton : MonoBehaviour
{
    private SoldierData soldierData;
    private GridManager gridManager;

    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI soldierName;

    public void Init(SoldierData soldierData)
    {
        gridManager = GameManagers.Instance.GridManager;
        this.soldierData = soldierData;

        image.color = soldierData.actorColor;
        soldierName.text = soldierData.actorName;

        GetComponentInChildren<Button>().onClick.AddListener(CreateSoldier);
    }

    private void CreateSoldier()
    {
        gridManager.CreateSoldier(soldierData);
    }
}
