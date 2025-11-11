using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class NoteDestroyer : MonoBehaviour
{
    public string noteTag = "Note";

    private void Reset()
    {
        // Make sure collider is set as trigger
        var collider = GetComponent<BoxCollider2D>();
        collider.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Destroy any note that enters this trigger zone
        if (other.CompareTag("Note"))
        {
                Debug.Log("Deleted missed note.");

            Destroy(other.gameObject);
        }
    }
}
