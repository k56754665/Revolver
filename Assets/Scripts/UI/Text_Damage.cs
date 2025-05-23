using UnityEngine;
using TMPro;
using DG.Tweening;

public class Text_Damage : MonoBehaviour
{
    TMP_Text _text;
    Vector3 _originPos;

    void Start()
    {
        _originPos = transform.position;
        _text = GetComponent<TMP_Text>();
        _text.text = "";
        Managers.GameManager.OnDamageEvent += UpdateDamage;
    }

    void OnDestroy()
    {
        Managers.GameManager.OnDamageEvent -= UpdateDamage;
    }

    void UpdateDamage(float damage)
    {
        _text.DOKill();       // 텍스트의 알파 트윈 제거
        transform.DOKill();   // 위치 흔들림 트윈 제거

        _text.text = $"-{(int)damage}";

        // 초기화
        _text.alpha = 1f;
        transform.localPosition = _originPos;

        // 흔들림
        transform.DOShakePosition(
            duration: 0.3f,
            strength: new Vector3(0.2f, 0.5f, 0),
            vibrato: 30,
            randomness: 90,
            fadeOut: true
        );

        // 서서히 사라짐
        _text.DOFade(0f, 0.5f).SetDelay(0.3f);
    }
}
