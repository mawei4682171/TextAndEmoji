using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine.UI;

public class TextPic : Text
{
    /// <summary>
    /// 图片池
    /// </summary>
    private readonly List<Image> m_ImagesPool = new List<Image>();

    /// <summary>
    /// 图片的最后一个顶点的索引
    /// </summary>
    private readonly List<int> m_ImagesVertexIndex = new List<int>();

    /// <summary>
    /// 正则取出所需要的属性
    /// </summary>
    private static readonly Regex s_Regex =
          new Regex(@"<quad name=(.+?) size=(\d*\.?\d+%?) width=(\d*\.?\d+%?) />", RegexOptions.Singleline);
    //          (.+?)表示  .任意字符  +一个 或 多个 ？可选
    //(\d*\.?\d+%?)   \d *  表示0个或多个数字 \. 表示转义字符 小数点   \d +  %？表示%可选
    public override void SetVerticesDirty()
    {
        base.SetVerticesDirty();
        UpdateQuadImage();
    }
    //protected override void Start()
    //{
    //    Debug.Log("kaishi");
    //    UpdateQuadImage();
    //}

    /*利用Match类和MatchCollection类，
    可以获得通过一个正则表达式实现的每一个匹配的细节。Match表示一次匹配，
    而MatchCollection类是一个Match对象的集合，其中的每一个对象都表示了一次成功的匹配。
  我们可以使用Regex对象的Match()方法和Matches()方法来检索匹配。
  https://blog.csdn.net/u011982340/article/details/39136401 */
    protected void UpdateQuadImage()
    {
        m_ImagesVertexIndex.Clear();
        foreach (Match match in s_Regex.Matches(text))
        {
            var picIndex = match.Index + match.Length - 1;//文字个数
            Debug.Log("picIndex:" + picIndex);
            var endIndex = picIndex * 4 + 3;      //计算最后顶点
            m_ImagesVertexIndex.Add(endIndex);       //顶点加入List

            m_ImagesPool.RemoveAll(image => image == null);//图片对象池移除所有图片
            if (m_ImagesPool.Count == 0)
            {
                GetComponentsInChildren<Image>(m_ImagesPool);
            }
            if (m_ImagesVertexIndex.Count > m_ImagesPool.Count)//如果顶点信息数大于图片数，新建图片
            {
                var resources = new DefaultControls.Resources();
                var go = DefaultControls.CreateImage(resources);
                go.layer = gameObject.layer;
                var rt = go.transform as RectTransform;
                if (rt)
                {
                    rt.SetParent(rectTransform);
                    rt.localPosition = Vector3.zero;
                    rt.localRotation = Quaternion.identity;
                    rt.localScale = Vector3.one;
                }
                m_ImagesPool.Add(go.GetComponent<Image>());
            }

            var spriteName = match.Groups[1].Value;
            var size = float.Parse(match.Groups[2].Value);
            var img = m_ImagesPool[m_ImagesVertexIndex.Count - 1];
            if (img.sprite == null || img.sprite.name != spriteName)
            {


                img.sprite = Resources.Load<Sprite>(spriteName);
            }
            img.rectTransform.sizeDelta = new Vector2(size, size);
            img.enabled = true;
        }

        for (var i = m_ImagesVertexIndex.Count; i < m_ImagesPool.Count; i++)
        {
            if (m_ImagesPool[i])
            {
                m_ImagesPool[i].enabled = false;
            }
        }
    }

    protected override void OnPopulateMesh(Mesh toFill)
    {
        base.OnPopulateMesh(toFill);
        var verts = toFill.vertices;

        for (var i = 0; i < m_ImagesVertexIndex.Count; i++)
        {
            var endIndex = m_ImagesVertexIndex[i];
            var rt = m_ImagesPool[i].rectTransform;
            var size = rt.sizeDelta;
            if (endIndex < verts.Length)
            {
                rt.anchoredPosition = new Vector2(verts[endIndex].x + size.x / 2, verts[endIndex].y + size.y / 2);

                // 抹掉左下角的小黑点
                for (int j = endIndex, m = endIndex - 3; j > m; j--)
                {
                    verts[j] = verts[m];
                }
            }
        }

        if (m_ImagesVertexIndex.Count != 0)
        {
            toFill.vertices = verts;
            m_ImagesVertexIndex.Clear();
        }
    }
}