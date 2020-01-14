using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TadaLib.Save;

namespace Save
{
    public class SaveDeleter : MonoBehaviour
    {
        public void DeleteData()
        {
            SaveManager.Instance.DeleteAllData();
        }
    }
}