using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color baseColour, offsetColour;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [SerializeField] private GameObject highlight;

    /// <summary>
    /// Changes tile colour if tile is offset 
    /// </summary>
    /// <param name="isOffset"></param>
    public void Init(bool isOffset) 
    {
        spriteRenderer.color = isOffset ? offsetColour : baseColour;
    }

    // Doesnt work :(
    private void OnMouseEnter()
    {
        highlight.SetActive(false);
    }

    private void OnMouseExit()
    {
        highlight.SetActive(false);
    }
}
