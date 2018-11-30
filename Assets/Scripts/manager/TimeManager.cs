﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TimeManager : MonoBehaviour {
    public delegate void TaskEvent();
    Dictionary<int, TimeTaskModel> TaskDic = new Dictionary<int, TimeTaskModel>();
    int Index = 0;
    List<int> RemoveList = new List<int>();

    void Awake()
    {
        GameApp.Instance.TimeManagerScript = this;
        //StartCoroutine(AddShedule(5000,delegate() {Debug .Log (""); }));
    }

    void FixedUpdate()
    {
        for (int i=0;i<RemoveList.Count; i++)
        {
            TaskDic.Remove(RemoveList[i]);
        }
        RemoveList.Clear();
        List<int> taskId = new List<int>(TaskDic.Keys);
        long time = DateTime.Now.Ticks;
        for (int i=0;i<taskId.Count; i++)
        {
            if(TaskDic [taskId [i]].time <=time)
            {
                RemoveList.Add(taskId[i]);
                try
                {
                    TaskDic[taskId[i]].Run();
                }catch (Exception e)
                {
                    Debug.LogError(e);
                }
            }
        }
    }

    /// <summary>
    /// 添加计时器任务
    /// </summary>
    /// <param name="task"></param>
    /// <param name="time">于time毫秒后执行</param>
    /// <returns></returns>
    public int AddShedule(TaskEvent task,long time)
    {
        Index++;
        TimeTaskModel m = new TimeTaskModel(Index, time * 10000 + DateTime.Now.Ticks, task);
        TaskDic.Add(Index, m);
        return Index;
    }

    public bool Remove(int idx)
    {
        if(RemoveList .Contains (idx))
        {
            return true;
        }
        if(TaskDic .ContainsKey (idx))
        {
            RemoveList.Add(idx);
            return true;
        }
        return false;
    }

    #region unity自带的计时器
    //IEnumerator AddShedule(float time, TaskEvent task)
    //{
    //    yield return new WaitForSeconds(time);
    //    task();
    //}
    #endregion
}
