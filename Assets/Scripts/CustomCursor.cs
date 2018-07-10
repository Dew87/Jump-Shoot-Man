using UnityEngine;

public class CustomCursor : MonoBehaviour
{
    public Texture2D Texture;

    private void Start()
    {
        Cursor.visible = false;
    }

    private void OnGUI()
    {
        Vector2 position = Event.current.mousePosition;
        position.x -= Texture.width / 2;
        position.y -= Texture.height / 2;
        Vector2 size = new Vector2(Texture.width, Texture.height);
        Rect rectangle = new Rect(position, size);

        GUI.DrawTexture(rectangle, Texture);
    }
}
