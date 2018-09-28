using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitButtonController : MonoBehaviour {

    //
    // instance fields
    //

	public Color highlightColor = Color.red;

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

    public void OnMouseDown()
    {
        Debug.Log("exit");

        // quit application
        Application.Quit();                                 // in standalone application
    }

    public void OnMouseExit() {
        // reset button scale
        transform.localScale = originalScale;

        // unhighlight button
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        if (sprite != null)
            sprite.color = originalColor;
    }
}
