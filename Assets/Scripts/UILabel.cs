using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class UILabel : MonoBehaviour
{
    private TMP_Text _text;

    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
    }

    public void ChangeText(int data)
    {
        _text.text = data.ToString();
    }
}
