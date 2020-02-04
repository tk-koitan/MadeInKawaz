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
            StartCoroutine(Delete());
        }

        private IEnumerator Delete()
        {
            SaveManager.Instance.DeleteAllData();

            // データが削除されるまで待つ
            yield return new WaitForSeconds(0.2f);

            ScoreManager.Instance.Reload();
        }
    }
}