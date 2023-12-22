using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTileController : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;

    public void SetGridMap(int width, int height, float size)
    {
        meshRenderer.material.mainTextureScale = new Vector2(width, height);
        transform.localScale = new Vector3(width, height, 1) * size;
    }
}
