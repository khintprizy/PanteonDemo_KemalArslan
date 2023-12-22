using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierProductionController : MonoBehaviour
{
    private PrefabManager prefabManager;
    [SerializeField] private List< SoldierButton> buttons = new List< SoldierButton>();

    public void Init(SoldierData[] soldierDatas)
    {
        if (buttons.Count > 0)
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                Destroy(buttons[i].gameObject);
            }

            buttons.Clear();
        }

        prefabManager = PrefabManager.Instance;

        for (int i = 0; i < soldierDatas.Length; i++)
        {
            SoldierButton soldierButton = prefabManager.GetSoldierButton(transform);
            soldierButton.Init(soldierDatas[i]);
            buttons.Add(soldierButton);
        }
    }
}
