using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfSWController : MonoBehaviour
{
    private List<RectTransform> buttons = new List<RectTransform>();

    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private RectTransform panelTr;
    [SerializeField] private RectTransform productionMenuTr;
    [SerializeField] private VerticalLayoutGroup vertGroup;

    private Vector2 lastVelo;
    private bool changed;

    private void Start()
    {

    }

    private void Update()
    {
        float unitHeight = (buttons.Count / 3) * (buttons[0].rect.height + vertGroup.spacing);

        //Debug.Log(productionMenuTr.localPosition);

        // Here, I controlled the changed frame of velocity, and if changed set the velocity to prevent any jittered movement
        if (changed)
        {
            scrollRect.velocity = lastVelo;
            changed = false;
        }

        if (productionMenuTr.localPosition.y < 0)
        {
            changed = true;
            lastVelo = scrollRect.velocity;
            productionMenuTr.localPosition += new Vector3(0, unitHeight, 0);
        }

        if (productionMenuTr.localPosition.y > (unitHeight * 2))
        {
            changed = true;
            lastVelo = scrollRect.velocity;

            productionMenuTr.localPosition -= new Vector3(0, unitHeight, 0);
        }
    }

    public void AddToButtons(RectTransform button)
    {
        buttons.Add(button);
    }

    public void SetToMiddle()
    {
        float yPos = ((buttons[0].rect.height + vertGroup.spacing) * buttons.Count / 3);
        //float yPos = 0;
        productionMenuTr.localPosition = new Vector3(productionMenuTr.localPosition.x, 450, productionMenuTr.localPosition.z);
    }
}
