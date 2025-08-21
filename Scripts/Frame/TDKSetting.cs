using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TDKSetting", menuName = "TDKToolkit/TDKSetting")]
public class TDKSetting : ScriptableObject
{
    public string ModleResources;
    public string PrefebsResources;
    public string UIResources;


    public List<Sprite> lodingSpriteIMG;
    public Sprite GetRodomSprite()
    {

        if (lodingSpriteIMG == null || lodingSpriteIMG.Count == 0)
        {
            Debug.LogWarning("lodingSpriteIMG �б�Ϊ�ջ�δ��ʼ����");
            return null;
        }

        int randomIndex = UnityEngine.Random.Range(0, lodingSpriteIMG.Count);
        return lodingSpriteIMG[randomIndex];

    }


}
