﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameProtocol.model.fight;

public class UI_Head : MonoBehaviour {
    /// <summary>
    /// 头像位置的集合
    /// </summary>
    public List<Vector3> PosList = new List<Vector3>();

    /// <summary>
    /// 玩家头像集合
    /// </summary>
    Dictionary<int, GameObject> HeadList = new Dictionary<int, GameObject>();

    void Awake()
    {
        GameApp.Instance.UI_HeadScript = this;
    }

    void Start()
    {
        //初始化玩家头像位置
        switch (GameSession .Instance .RoomeType)
        {
            //赢三张
            case GameProtocol.SConst.GameType.WINTHREEPOKER:
                {
                    //玩家从自己到最后一家的头像位置
                    PosList.Add(new Vector3(-375, -392, 0));
                    PosList.Add(new Vector3(-862, 22, 0));
                    PosList.Add(new Vector3(-862, 279, 0));
                    PosList.Add(new Vector3(871, 279, 0));
                    PosList.Add(new Vector3(871, 22, 0));
                }
                break;
        }
    }

    /// <summary>
    /// 当前待刷新的玩家信息和玩家自己的方位
    /// </summary>
    /// <param name="model"></param>
    /// <param name="userdir"></param>
    public void UpdateItem(FightUserModel model,int userdir)
    {
        //头像待刷新的位置
        Vector3 pos;
        //如果是玩家自己，直接使用第0个
        if(model .id==GameSession .Instance .UserInfo.id)
        {
            pos = PosList[0];
        }else
        {
            //如果玩家方位大于自己的方位的话
            if(model .direction >userdir)
            {
                //直接用玩家的方位减去自己的方位即为玩家的头像位置
                pos = PosList[model.direction - userdir];
            }else
            {
                //用玩家最大人数减去自己的方位再加上玩家的方位，即为玩家的位置
                pos = PosList[PosList.Count - userdir + model.direction];
            }
        }
        //移除原有信息
        if (HeadList.ContainsKey(model.id))
        {
            Destroy(HeadList[model.id]);
            HeadList.Remove(model.id);
        }
        //加载头像到页面中
        string path = GameResources.ItemResourcesPath + GameData.Instance.ItemName[GameResources.ItemTag.TPHEAD];
        GameObject go = GameApp.Instance.ResourcesManagerScript.LoadInstantiateGameObject(path, transform, pos);
        go.AddComponent<HeadItem>().UpdateItem(model);
        HeadList.Add(model.id, go);
    }


}