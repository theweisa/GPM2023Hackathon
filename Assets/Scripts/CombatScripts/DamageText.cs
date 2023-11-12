using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class DamageText : MonoBehaviour
{
    public enum TextType { Damage, Heal, Player };
    public float angle = 20f;
    public float angleOffset = 5f;
    public float textDist = 0.7f;
    public float textDur = 0.7f;
    public TMP_Text text;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TMP_Text>();
    }

    public void Init(float dmg, TextType type=TextType.Damage) {
        switch(type) {
            case TextType.Damage:
                break;
            case TextType.Heal:
                text.color = Color.green;
                text.fontSize *= 1.4f;
                break;
            case TextType.Player:
                text.color = Color.red;
                break;
            default:
                break;
        }
        text.text = dmg.ToString("F0");
        if (dmg < 0) text.text = dmg.ToString("F1");
        float newAngle = (angle+UnityEngine.Random.Range(-angleOffset, angleOffset))*Mathf.Deg2Rad;
        Vector2 dir = new Vector2(Mathf.Sin(newAngle), Mathf.Cos(newAngle)) * textDist;
        LeanTween.move(gameObject, (Vector2)transform.position + dir, textDur).setEase(LeanTweenType.easeOutQuart).setOnComplete(()=>{
            Destroy(gameObject);
        });

        Color currColor = text.color;
        Color fadeColor = currColor;
        fadeColor.a = 0;
        LeanTween.value(gameObject, (Color val)=>{ text.color=val; }, currColor, fadeColor, textDur).setEase(LeanTweenType.easeInExpo);
    }
}
