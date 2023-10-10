using System;
using System.Linq;
using Manager;
using Store.Equipment;
using UnityEngine;
using Util;
using Util.UI;
using Weapon;
using ContextMenu = Util.UI.ContextMenu;

namespace Tutorial
{
  public class TutorialScene : MonoBehaviour
  {
    private bool event1;
    private bool event2;
    private bool event7;
    private Information infoDown;
    private Information infoUp;
    private Highlight[] highlights;
    private UIVisibler ignore;

    private void Awake()
    {
      infoDown = GameObject.Find("info_down").GetComponent<Information>();
      infoUp = GameObject.Find("info_up").GetComponent<Information>();
      highlights = FindObjectsOfType<Highlight>().OrderBy(x => x.name).ToArray();
      GameManager.Manager.onClear += ManagerOnClear;
    }

    private void ManagerOnClear()
    {
      GameManager.Manager.onClear -= ManagerOnClear;
      try
      {
        GameManager.Spawn.onSpawned -= WaveOnEnemySpawn;
      }
      catch
      {
      }
      try
      {
        GameManager.Wave.onStoreOpen -= WaveOnStoreOpen;
      }
      catch
      {
      }
      try
      {
        GameManager.Wave.onStoreOpen -= WaveOnStoreOpen;
      }
      catch
      {
      }

      try
      {
        ContextMenu.onOpened -= ContextMenuOnonOpened;
        
      }
      catch
      {
        
      }
      try
      {
        var wui = FindObjectOfType<WeaponInventoryUI>();
        wui.onDuplicated -= WuiOnonDuplicated;
      }
      catch
      {
        
      }

      try
      {
        WeaponSlot.onSale -= WeaponSlotOnonSale;
      }
      catch
      {
        
      }

      try
      {
        GameManager.Camera.SetZoom();
      }
      catch
      {
        
      }
    }

    private void Start()
    {
      ignore = GameManager.UI.Find<UIVisibler>("$ignore");
      GameManager.Spawn.onSpawned += WaveOnEnemySpawn;
      GameManager.Wave.onStoreOpen += WaveOnStoreOpen;
    }

    private void WaveOnEnemySpawn()
    {
      1.5f.Wait(() =>
      {
        if (event1)
        {
          GameManager.Spawn.onSpawned -= WaveOnEnemySpawn;
          return;
        }

        event1 = true;
        Event1();
      });
    }
    
    private void WaveOnStoreOpen()
    {
      if (event2)
      {
        GameManager.Wave.onStoreOpen -= WaveOnStoreOpen;
        return;
      }

      event2 = true;
      Event2();
    }

    private void Event1()
    {
      GameManager.Camera.SetTarget(FindAnyObjectByType<TargetableObject>().transform);
      GameManager.Camera.SetZoom(7f);
      Time.timeScale = 0f;
      infoDown.StartWrite("외계인을 잡아 코인을 획득하고 웨이브 끝까지 생존하세요.", () =>
      {
        GameManager.Camera.SetTarget(GameManager.Player.transform);
        GameManager.Camera.SetZoom();
        Time.timeScale = 1f;
      });
    }
    
    private void Event2()
    {
      highlights[0].StartHighlighting();
      ignore.SetVisible(true);
      infoDown.StartWrite("웨이브가 끝난 후 상점에서 무기, 용병, 아이템, 소모품 등을 구매할 수 있습니다.", () =>
      {
        highlights[0].StopHighlighting();
        Event3();
      });
    }

    private void Event3()
    {
      highlights[1].StartHighlighting();
      infoDown.StartWrite("무기와 용병, 소모품은 구매 시 자동으로 장착되며, 아이템을 통해 능력치를 상승시킬 수 있습니다.", () =>
      {
        highlights[1].StopHighlighting();
        Event4();
      });
    }
    
    private void Event4()
    {
      highlights[2].StartHighlighting();
      infoDown.StartWrite("상점에서 원하는 아이템이 없으면 새로 고침을 통해 매물을 바꿀 수 있습니다.", () =>
      {
        highlights[2].StopHighlighting();
        Event5();
      });
    }
    
    private void Event5()
    {
      GameManager.UI.Find<RadioButtonList>("$store_tab_buttons").SetEnable("weapon");
      highlights[3].StartHighlighting();
      infoDown.StartWrite("무기에는 속성이 있으며 같은 속성의 무기를 장착하는 것은 캐릭터를 강하게 해줍니다.", () =>
      {
        highlights[3].StopHighlighting();
        Event6();
      });
    }
    
    private void Event6()
    {
      GameManager.UI.Find<RadioButtonList>("$store_tab_buttons").SetEnable("partner");
      highlights[3].StartHighlighting();
      infoDown.StartWrite("용병에게도 속성이 있고, 같은 속성의 무기를 장착하면 무기가 강화됩니다.", () =>
      {
        highlights[3].StopHighlighting();
        Event7();
      });
    }
    
    private void Event7()
    {
      var wui = FindObjectOfType<WeaponInventoryUI>();
      wui.list[0].slots[1].Set(wui.list[0].slots.First(x => x.weapon.HasValue).weapon);
      
      ContextMenu.onOpened += ContextMenuOnonOpened;
      wui.onDuplicated += WuiOnonDuplicated;
      WeaponSlot.onSale += WeaponSlotOnonSale;
      
      infoDown.StartWrite("무기와 용병은 같은 티어의 아이템끼리 결합 할 수 있습니다. \n한 번 결합 해 보세요.", () =>
      {
        ignore.SetVisible(false);
        highlights[4].StartHighlighting();
      });
    }
    
    private void ContextMenuOnonOpened()
    {
      ContextMenu.onOpened -= ContextMenuOnonOpened;
      highlights[4].StopHighlighting();
    }

    private void WeaponSlotOnonSale()
    {
      WeaponSlot.onSale -= WeaponSlotOnonSale;
      try
      {
        FindObjectOfType<WeaponInventoryUI>().onDuplicated -= WuiOnonDuplicated;
      }
      catch
      {
        
      }
      if (event7) return;
      
      infoDown.StartWrite("그래요, 다른 무기를 사용하고 싶으면 판매하실 수도 있답니다.", () =>
      {
        event7 = true;
        Event8();
      });
    }
    
    private void WuiOnonDuplicated()
    {
      FindObjectOfType<WeaponInventoryUI>().onDuplicated -= WuiOnonDuplicated;
      try
      {
        WeaponSlot.onSale -= WeaponSlotOnonSale;
      }
      catch
      {
        
      }
      if (event7) return;
      
      event7 = true;
      infoDown.StartWrite("잘하셨습니다. 또 무기를 판매할 수도 있으니 참고하세요.", () =>
      {
        Event8();
      });
    }

    private void Event8()
    {
      ignore.SetVisible(true);
      highlights[5].StartHighlighting();
      infoUp.StartWrite("소모품은 최대 3개까지 챙길 수 있으며, 한 번 사용하면 사라집니다.", () =>
      {
        highlights[5].StopHighlighting();
        Event9();
      });
    }
    
    private void Event9()
    {
      highlights[6].StartHighlighting();
      infoUp.StartWrite("다음 웨이브를 클리어하고 튜토리얼을 끝내세요.", () =>
      {
        highlights[6].StopHighlighting();
        ignore.SetVisible(false);
      });
    }
  }
}
