using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color baseColour, offsetColour;
    private SpriteRenderer spriteRenderer;

    [SerializeField] private GameObject highlight;

    /// <summary>
    /// Changes tile colour if tile is offset 
    /// </summary>
    /// <param name="isOffset"></param>
    /// 
    public void Awake()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }
    public void Init(bool isOffset) 
    {
        spriteRenderer.color = isOffset ? offsetColour : baseColour;
    }

    // Doesnt work :(
    void OnMouseEnter()
    {
        highlight.SetActive(true);
        Debug.Log("Mouse has entered" + gameObject.name);
    }

    void OnMouseExit()
    {
        highlight.SetActive(false);
    }
}
