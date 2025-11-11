using UnityEngine;
using System.Collections;

public class SpriteController : MonoBehaviour
{
    private SpriteRenderer SR;
    public Sprite defaultSprite;
    public Sprite pressedSprite;

    public KeyCode keyPress;

    void Start()
    {
        SR = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if(Input.GetKey(keyPress))
        {
            SR.sprite = pressedSprite;
        }

        if(Input.GetKeyUp(keyPress))
        {
            SR.sprite = defaultSprite;
        }
    }

}
