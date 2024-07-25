using System;
using UnityEngine;

namespace Managers
{
    public class InventoryUpdater : MonoBehaviour
    {
        [HideInInspector] public InventoryManager manager;
        [HideInInspector] public RectTransform[] gridContents;
        [HideInInspector] public RectTransform cell;

        private void OnEnable()
        {
            for (var i = 0; i < manager.Count; i++)
            {
                foreach (var (key, value) in manager[i])
                {
                    var obj = Instantiate(value.UIPrefab, gridContents[i]);
                    obj.name = $"Inv_Item_{i}_{key.x}_{key.y}";
                    var t = obj.GetComponent<RectTransform>();
                    t.anchoredPosition = key * cell.rect.size;
                }
            }
        }

        private void OnDisable()
        {
            for (var i = 0; i < transform.childCount; i++)
                Destroy(transform.GetChild(0));
        }
    }
}