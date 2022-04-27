using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class randomColorTitle : MonoBehaviour
{
    private TextMeshPro m_TextMeshPro;
    private string lable = "UNO";

    void Awake() {
        m_TextMeshPro = gameObject.GetComponent<TextMeshPro>() ?? gameObject.AddComponent<TextMeshPro>();
        m_TextMeshPro.text = lable;
        m_TextMeshPro.color = Color.cyan;

    }
}
