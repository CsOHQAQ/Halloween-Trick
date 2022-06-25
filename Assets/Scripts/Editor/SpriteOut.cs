using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Windows;

public class SpriteOut : Editor
{
   [MenuItem("Tools/ExportSprite")]
    public static void ExportSprite()
    {
        // 拿到选中的资源
        Object[] selects = Selection.objects;

        // 
        string savePath = Application.dataPath + "/outSprite/";
        Directory.CreateDirectory(savePath);
        foreach (Object item in selects)
        {
            Sprite sprite = item as Sprite;
            if (sprite == null)
                continue;

            // 获取精灵的贴图
            Texture2D t = sprite.texture;

            // 创建一个新的贴图
            Texture2D newTex = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height, TextureFormat.ARGB32, false);
            // 设置像素点为 选择贴图的像素点
            newTex.SetPixels(t.GetPixels((int)sprite.rect.xMin, (int)sprite.rect.yMin, (int)sprite.rect.width, (int)sprite.rect.height));

            newTex.Apply();

            // 把创建的贴图对象，转换为bytes
            byte[] buffer = newTex.EncodeToPNG();
            // 写出
            File.WriteAllBytes(savePath + sprite.name + ".png", buffer);
        }
    }
}
