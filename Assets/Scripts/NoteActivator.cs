using UnityEngine;

public class NoteActivator : MonoBehaviour
{
    private bool canBeActivated;
    public KeyCode keyPress;

    void Update()
    {
        // Allow activation both on press and while held
        if ((Input.GetKeyDown(keyPress) || Input.GetKey(keyPress)) && canBeActivated)
        {
            HitNote();
        }
    }

    void HitNote()
    {
        // Deactivate the note
        gameObject.SetActive(false);
        canBeActivated = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Activator"))
            canBeActivated = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Activator"))
            canBeActivated = false;
    }
}
