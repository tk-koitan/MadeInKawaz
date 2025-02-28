using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <sumary>
/// 時間を計測するクラス
/// </sumary>
/// 
namespace ZakkyLib
{
    public class Timer
    {
        private float start_time_;
        private float limit_time_;

        public Timer(float limit_time)
        {
            limit_time_ = limit_time;
            start_time_ = Time.time;
        }

        public void TimeReset()
        {
            start_time_ = Time.time;
        }

        public bool IsTimeout()
        {
            return Time.time - start_time_ >= limit_time_;
        }

        public float GetTime()
        {
            return Time.time - start_time_;
        }
    }
}