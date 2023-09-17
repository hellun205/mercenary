using System;
using Item;
using Store.Equipment;
using UnityEngine;
using UnityEngine.UI;

namespace Store.Inventory
{
  public class DragItem : MonoBehaviour
  {
    [SerializeField]
    private Image targetImage;

    public IPossessible itemData;

    public bool isDragging;

    public bool isWeaponInventory;

    public int wrapperIndex;
    public int weaponInventoryIndex;

    [NonSerialized]
    public RectTransform rectTransform;

    public WeaponSlotWrapperList list;

    public void SetItem(IPossessible data, bool isWeaponInventory, int wrapperIndex = 0, int wIndex = 0)
    {
      itemData = data;
      targetImage.sprite = data.icon;
      isDragging = true;
      this.isWeaponInventory = isWeaponInventory;
      this.wrapperIndex = wrapperIndex; 
      weaponInventoryIndex = wIndex;
      gameObject.SetActive(true);
    }

    public void EndDrag()
    {
      isDragging = false;
      gameObject.SetActive(false);
    }

    private void Awake()
    {
      rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
      gameObject.SetActive(false);
    }
  }
}
