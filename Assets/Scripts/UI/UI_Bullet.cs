using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Bullet : MonoBehaviour
{
    public void InitBullet(int id)
    {
        TMP_Text _text = GetComponentInChildren<TMP_Text>();
        Image _image = GetComponentInChildren<Image>();
        BulletData bullet = Managers.BulletManager.GetBullet(id);
        _text.text = bullet.title;
        _image.color = bullet.color;
    }
}
