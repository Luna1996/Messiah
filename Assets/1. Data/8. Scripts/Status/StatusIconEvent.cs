using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
public class StatusIconEvent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    //public Text text;
    Image statusIconImage = null;
    [SerializeField]
    TextMeshProUGUI statusIconText = null;
    Status status = null;
    int number = 0;
    public void SetStatus(Status status, int duration)
    {
        this.status = status;
        number = duration;
        statusIconText.text = number.ToString();
        statusIconImage.sprite = this.status.sprite;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(status)//点击icon可以看见相应的描述
        //text.enabled = true;
            StatusDisplayDetailsFromIcon.instance.StartDisplay(status, number);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        //text.enabled = false;
        StatusDisplayDetailsFromIcon.instance.StopDisplay();
    }
}
