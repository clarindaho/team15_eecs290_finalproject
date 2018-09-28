using UnityEngine;
using System.Collections;

public class UIButton : MonoBehaviour
{
    //
    // instance fields
    //

    [SerializeField] private GameObject sceneControllerTargetObject;
    [SerializeField] private string sceneControllerMethod = "DeactivateTrigger";
    [SerializeField] private string sceneControllerRotateMethod;

    public Color highlightColor = Color.cyan;

    private bool clickable = true;
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
        if (clickable)
        {
            // enlarge button
            transform.localScale = new Vector3(1.1f * originalScale.x, 1.1f * originalScale.y, 1.1f * originalScale.z);

            // highlight button
            SpriteRenderer sprite = GetComponent<SpriteRenderer>();
            if (sprite != null)
                sprite.color = highlightColor;
        }
    }

    public void OnMouseDown()
    {
        if (clickable && sceneControllerTargetObject != null)
        {
            Debug.Log("Test Button: CLICKED");

            // deactive trigger of goal node
			sceneControllerTargetObject.SendMessage(sceneControllerMethod);

			if (!sceneControllerRotateMethod.Equals("NONE")) {
				// rotate puzzle
				sceneControllerTargetObject.SendMessage (sceneControllerRotateMethod);
			}
        }
    }

    public void OnMouseExit()
    {
        // reset button scale
        transform.localScale = originalScale;

        // unhighlight button
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        if (sprite != null)
            sprite.color = originalColor;
    }

    public void Enable()
    {
        clickable = true;
    }

    public void Disable()
    {
        clickable = false;
    }
}
