using UnityEngine;
using System.IO;

public class SpriteExporter : MonoBehaviour
{
    public Camera renderCamera;  // Assign a camera that looks at the sprite
    public SpriteRenderer spriteRenderer;

    private void Start()
    {
        CaptureSprite();
    }
    public void CaptureSprite()
    {
        int width = 512;  // Adjust as needed
        int height = 512;

        RenderTexture renderTex = new RenderTexture(width, height, 24, RenderTextureFormat.ARGB32);
        renderTex.antiAliasing = 8;  // Improves edges
        renderCamera.targetTexture = renderTex;

        Texture2D texture = new Texture2D(width, height, TextureFormat.RGBA32, false);

        renderCamera.clearFlags = CameraClearFlags.SolidColor;
        renderCamera.backgroundColor = new Color(0, 0, 0, 0);  // Transparent background

        renderCamera.Render();
        RenderTexture.active = renderTex;
        texture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        texture.Apply();

        renderCamera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(renderTex);

        byte[] bytes = texture.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + "/ExportedSprite.png", bytes);
        Debug.Log("Sprite saved at " + Application.dataPath + "/ExportedSprite.png");
    }
}
