using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class title : MonoBehaviour
{
    private TMP_Text m_TextMeshPro;

    private string label = "UNO";
    
    void Awake() {
        m_TextMeshPro = gameObject.GetComponent<TMP_Text>() ?? gameObject.AddComponent<TMP_Text>();

        m_TextMeshPro.text = label;
        m_TextMeshPro.color = new Color(247, 0, 255, 1);
    }

    
}
