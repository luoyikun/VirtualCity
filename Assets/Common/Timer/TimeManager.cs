using System.Collections.Generic;  
using UnityEngine;
using Framework.Pattern;

namespace Framework.Tools
{
    public delegate void TimeTaskDelegate();

    /// <summary>  
    /// 定时任务管理器  
    /// </summary>  
    public class TimeManager : SingletonMono<TimeManager>
    {
        /// <summary>  
        /// 定时任务列表  
        /// </summary>  
        private List<TimeTask> taskList = new List<TimeTask>();
        private Dictionary<string, TimeTask> m_dicTask = new Dictionary<string, TimeTask>();
        /// <summary>  
        /// 添加定时任务  
        /// </summary>  
        /// <param name="timeDelay">延时执行时间间隔</param>  
        /// <param name="repeat">是否可以重复执行</param>  
        /// <param name="timeTaskCallback">执行回调</param>  
        public void AddTask(float timeDelay, bool repeat, TimeTaskDelegate timeTaskCallback)
        {
            AddTask(new TimeTask(timeDelay, repeat, timeTaskCallback));
        }
        public void AddTask(TimeTask taskToAdd)
        {
            if (taskList.Contains(taskToAdd) || taskToAdd == null) return;
            taskList.Add(taskToAdd);
        }


        public void StartUp()
        {

        }
        public void DicAddTask(string key, float timeDelay, bool repeat, TimeTaskDelegate timeTask)
        {
            if (m_dicTask.ContainsKey(key))
            {
                m_dicTask.Remove(key);
            }
            TimeTask task = new TimeTask(timeDelay, repeat, timeTask);
            m_dicTask[key] = task;
        }

        /// <summary>  
        /// 移除定时任务  
        /// </summary>  
        /// <param name="taskToRemove"></param>  
        /// <returns></returns>  
        public bool RemoveTask(TimeTaskDelegate taskToRemove)
        {
            if (taskList.Count == 0 || taskToRemove == null) return false;
            for (var i = 0; i < taskList.Count; i++)
            {
                TimeTask item = taskList[i];

                if (item.timeTaskCallBack == taskToRemove)
                    return taskList.Remove(item);
            }
            return false;
        }

        public void RemoveAllTask()
        {
            taskList.Clear();
        }
        void Update()
        {
            Tick();
        }

        /// <summary>  
        /// 执行定时任务  
        /// </summary>  
        private void Tick()
        {
            if (taskList == null) return;
            for (var i = 0; i < taskList.Count;)
            {
                TimeTask task = taskList[i];
                task.timeDelay -= Time.deltaTime;
                if (task.timeDelay <= 0)
                {
                    if (task.timeTaskCallBack != null)
                    {
                        task.timeTaskCallBack();
                    }
                    if (!task.repeat)
                    {
                        taskList.Remove(task);
                        continue;
                    }
                    task.timeDelay = task.timeDelayOnly;
                }
                i++;
            }
        }
    }
}