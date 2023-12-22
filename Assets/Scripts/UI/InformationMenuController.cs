using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InformationMenuController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI buildingName;
    [SerializeField] private Image buildingImage;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private Button moveButton;
    [SerializeField] private SoldierProductionController soldierProductionController;
    [SerializeField] private GameObject panel;

    private BuildingBase currentBuilding;

    private void Start()
    {
        moveButton.onClick.AddListener(OnMoveButtonClicked);

        SetThePanel(false);
    }

    public void SetInfoMenu(BuildingBase buildingBase)
    {
        SetThePanel(true);

        BuildingData data = buildingBase.GetActorData() as BuildingData;
        currentBuilding = buildingBase;

        buildingName.text = data.actorName;
        buildingImage.sprite = data.actorSprite;
        description.text = data.actorDescription;

        soldierProductionController.gameObject.SetActive(data.producableSoldiers.Length > 0);

        soldierProductionController.Init(data.producableSoldiers);
    }

    public void SetThePanel(bool isActive)
    {
        panel.SetActive(isActive);
    }

    private void OnMoveButtonClicked()
    {
        if (currentBuilding != null)
        {
            currentBuilding.PickBuildingFromGrid();
            SetThePanel(false);
        }
    }
}
