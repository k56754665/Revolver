using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Slot : MonoBehaviour, IDropHandler
{
    public int SlotNum;

    BulletInSlot _childBullet;

    void Start()
    {
        _childBullet = GetComponentInChildren<BulletInSlot>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        BulletInSlot bulletInSlot = eventData.pointerDrag.GetComponent<BulletInSlot>();
        if (bulletInSlot != null)
        {
            if (_childBullet.Id != -1) // 현재 슬롯에 총알이 있다면
            {
                // 두개 위치 바꾸기
                int tempId = bulletInSlot.Id; // 드래그 하고 있는 총알 저장
                Managers.EquipManager.SetBulletInSlot(bulletInSlot.SlotNum, _childBullet.Id);
                Managers.EquipManager.SetBulletInSlot(_childBullet.SlotNum, tempId);
            }
            else // 현재 슬롯에 총알이 없다면
            {
                // 원래 위치의 총알 없애고 현재 슬롯에 총알 넣기
                Managers.EquipManager.SetBulletInSlot(SlotNum, bulletInSlot.Id);
                Managers.EquipManager.SetBulletInSlot(bulletInSlot.SlotNum, -1);
            }
        }

        Bullet origin = eventData.pointerDrag.GetComponent<Bullet>();
        if (origin != null && (_childBullet.Id == -1))
        {
            Managers.EquipManager.SetBulletInSlot(SlotNum, origin.Id);
            origin.OnDropInSlot();
        }
    }
}
