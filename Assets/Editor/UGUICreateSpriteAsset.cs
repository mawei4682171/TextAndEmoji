using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using System.Collections.Generic;

/*写一个编辑类，用来创建asset文件，在文件夹中，选中第一步的资源图片，右键然后点击"Create/UGUI Sprite Asset"，
即可以创建一个同图片同名的.asset文件，UGUICreateSpriteAsset此编辑类需要放在Editor文件夹中*/
public static class UGUICreateSpriteAsset
{
    [MenuItem("Assets/Create/UGUI Sprite Asset", false, 10)]//Assets 文件夹下创建目录
    static void main()
    {
        //Object[] textAssets = Selection.GetFiltered(typeof(TextAsset), SelectionMode.DeepAssets);
        Object target = Selection.activeObject;  //点击选中选择命令
        if (target == null || target.GetType() != typeof(Texture2D))  
            return;
        Texture2D sourceTex = target as Texture2D;  //选中的贴图转为Texture
        //整体路径  
        string filePathWithName = AssetDatabase.GetAssetPath(sourceTex);
        //带后缀的文件名  
        string fileNameWithExtension = Path.GetFileName(filePathWithName);
        //不带后缀的文件名  
        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePathWithName);
        //不带文件名的路径  
        string filePath = filePathWithName.Replace(fileNameWithExtension, "");

        UGUISpriteAsset spriteAsset = AssetDatabase.
            LoadAssetAtPath(filePath + fileNameWithoutExtension + ".asset", typeof(UGUISpriteAsset)) as UGUISpriteAsset;//将脚本UGUISpriteAsset创建为asset文件；
        bool isNewAsset = spriteAsset == null ? true : false;
        if (isNewAsset)
        {
            spriteAsset = ScriptableObject.CreateInstance<UGUISpriteAsset>();
            spriteAsset.texSource = sourceTex;
            spriteAsset.listSpriteAssetInfor = GetSpritesInfor(sourceTex);
            AssetDatabase.CreateAsset(spriteAsset, filePath + fileNameWithoutExtension + ".asset");    //创建资源
        }
    }

    public static List<SpriteAssetInfor> GetSpritesInfor(Texture2D tex)
    {
        List<SpriteAssetInfor> m_sprites = new List<SpriteAssetInfor>();

        string filePath = UnityEditor.AssetDatabase.GetAssetPath(tex);   //获取图片的总路径

        Object[] objects = UnityEditor.AssetDatabase.LoadAllAssetsAtPath(filePath);//获取所有图片的子路径

        for (int i = 0; i < objects.Length; i++)
        {
            if (objects[i].GetType() == typeof(Sprite))
            {
                SpriteAssetInfor temp = new SpriteAssetInfor();
                Sprite sprite = objects[i] as Sprite;    //获取所有精灵的值，并存入List;
                temp.ID = i;
                temp.name = sprite.name;
                temp.pivot = sprite.pivot;
                temp.rect = sprite.rect;
                temp.sprite = sprite;
                m_sprites.Add(temp);
            }
        }
        return m_sprites;
    }

}