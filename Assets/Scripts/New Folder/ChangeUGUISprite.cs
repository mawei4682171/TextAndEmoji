using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ChangeUGUISprite : MonoBehaviour
{

    public UGUISpriteAsset usa;
    private float fTime = 0.0f;

    // Update is called once per frame  
    void Update()
    {
        fTime += Time.deltaTime;
        if (fTime >= 0.3f)
        {
            GetComponent<Image>().sprite = usa.listSpriteAssetInfor[Random.Range(0, usa.listSpriteAssetInfor.Count)].sprite;
            fTime = 0.0f;
        }
    }
}