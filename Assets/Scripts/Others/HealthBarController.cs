using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthBarController : MonoBehaviour
{
    [SerializeField] private Transform bar;
    [SerializeField] private TextMeshPro ghosText;

    private Coroutine ghostCoroutine;

    public void SetHeathBar(float startHealth, float currentHealth, float damageAmount, bool setGhostText = false)
    {
        bar.localScale = new Vector3 (currentHealth/startHealth, bar.localScale.y, bar.localScale.z);

        if (setGhostText )
        {
            SetGhostText(damageAmount);
        }
    }

    private void SetGhostText(float damageAmount)
    {
        if (ghostCoroutine != null)
        {
            StopCoroutine(ghostCoroutine);
            ghostCoroutine = null;
        }

        ghostCoroutine = StartCoroutine(SetGhostCr(damageAmount));
    }

    IEnumerator SetGhostCr(float damageAmount)
    {
        float activeTime = .25f;
        ghosText.gameObject.SetActive(true);
        ghosText.text = "-" + damageAmount;
        yield return new WaitForSeconds(activeTime);
        ghosText.gameObject.SetActive(false);
        ghostCoroutine = null;
    }
}
