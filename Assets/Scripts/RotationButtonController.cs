using UnityEngine;
using System.Collections;

public class RotationButtonController : MonoBehaviour
{
    //
    // instance fields
    // 

    [SerializeField] private GameObject interactionsPaneTargetObject;
    [SerializeField] private string interactionsPaneTargetMessage = "ButtonClick";

    [SerializeField] private bool cwButton;
    public Color highlightColor = Color.cyan;

    private Vector3 originalScale;
    private Color originalColor;

    //
    // setter and getter methods
    //

    //
    // mutator methods
    //

    public void Start()
    {
        // get original scale of button
        originalScale = transform.localScale;

        // get original color of button
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        originalColor = sprite.color;
    }

    public void OnMouseOver()
    {
        // enlarge button
        transform.localScale = new Vector3(1.1f * originalScale.x, 1.1f * originalScale.y, 1.1f * originalScale.z);

        // highlight button
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        if (sprite != null)
            sprite.color = highlightColor;
    }

    public void OnMouseUp()
    {
        if (interactionsPaneTargetObject != null)
        {
            if (cwButton)
                interactionsPaneTargetObject.SendMessage(interactionsPaneTargetMessage, true);
            else
                interactionsPaneTargetObject.SendMessage(interactionsPaneTargetMessage, false);
        }
    }

    public void OnMouseExit()
    {
        // reset button scale
        transform.localScale = this.originalScale;

        // unhighlight button
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        if (sprite != null)
            sprite.color = originalColor;
    }
}
