using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreDisplay : MonoBehaviour
{
    TextMeshProUGUI text;
    public MaskBehavior maskBehavior;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = $"清潔度:{maskBehavior.GetClearRate() * 100:F1}％";
    }
}
