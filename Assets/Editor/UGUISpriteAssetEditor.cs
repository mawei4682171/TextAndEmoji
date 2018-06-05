using UnityEngine;
using UnityEditor;
using System.Collections;



/*选择.asset文件，你就会发现上面就会有一些你想要存储的信息，
但是并不是很美观(其实我改过之后还是很一般)，这里我们可以通过一个[CustomEditor] 来自定义属性面板，
他需要继承Editor，并在类名上声明他为哪个类自定义属性表，需要使用OnInspectorGUI函数来绘制，
 使用方法与OnGUI一样，注意关键字override，同样UGUISpriteAssetEditor类也需要放在Editor文件夹中*/
 [CustomEditor(typeof(UGUISpriteAsset))]
public class UGUISpriteAssetEditor : Editor
{

    UGUISpriteAsset spriteAsset;

    public void OnEnable()
    {
        spriteAsset = (UGUISpriteAsset)target;
    }
    private Vector2 ve2ScorllView;
    public override void OnInspectorGUI() //绘制自定义属性表，用法跟OnGUI类似
    {
        ve2ScorllView = GUILayout.BeginScrollView(ve2ScorllView);  //开始绘制
        GUILayout.Label("UGUI Sprite Asset");                                    //写标题
        if (spriteAsset.listSpriteAssetInfor == null)                              
            return;
        for (int i = 0; i < spriteAsset.listSpriteAssetInfor.Count; i++)
        {
            GUILayout.Label("\n");
           
            EditorGUILayout.ObjectField("", spriteAsset.listSpriteAssetInfor[i].sprite, typeof(Sprite));
            EditorGUILayout.IntField("ID:", spriteAsset.listSpriteAssetInfor[i].ID);
            EditorGUILayout.LabelField("name:", spriteAsset.listSpriteAssetInfor[i].name);
            EditorGUILayout.Vector2Field("povit:", spriteAsset.listSpriteAssetInfor[i].pivot);
            EditorGUILayout.RectField("rect:", spriteAsset.listSpriteAssetInfor[i].rect);
            GUILayout.Label("\n");
        }
        GUILayout.EndScrollView();
    }

}