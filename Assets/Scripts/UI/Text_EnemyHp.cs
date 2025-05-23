using UnityEngine;
using TMPro;

public class Text_EnemyHp : MonoBehaviour
{
    TMP_Text _text;
    Enemy _enemy;

    void Start()
    {
        _enemy = GetComponentInParent<Enemy>();
        _text = GetComponent<TMP_Text>();
        _text.text = $"{(int)_enemy.MaxHp}/{(int)_enemy.MaxHp}";
        _enemy.OnHpChangeEvent += UpdateText;
    }

    void OnDestroy()
    {
        _enemy.OnHpChangeEvent -= UpdateText;
    }

    void UpdateText(float hp)
    {
        _text.text = $"{(int)hp}/{_enemy.MaxHp}";
    }
}
