using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// SpriteAsset脚本:,实际继承的是Object 生成的是emjo。asset；
/// </summary>
public class UGUISpriteAsset : ScriptableObject
{
    /// <summary>  
    /// 图片资源  
    /// </summary>  
    public Texture texSource;
    /// <summary>  
    /// 所有sprite信息 SpriteAssetInfor类为具体的信息类  
    /// </summary>  
    public List<SpriteAssetInfor> listSpriteAssetInfor;
}