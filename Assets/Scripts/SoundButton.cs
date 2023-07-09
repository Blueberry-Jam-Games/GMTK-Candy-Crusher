using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundButton : MonoBehaviour
{
    private AudioSource source;
    private Button target;

    void Start()
    {
        source = GetComponent<AudioSource>();
        target = transform.parent.gameObject.GetComponent<Button>();

        target.onClick.AddListener(OnButtonPressed);        
    }

    void OnButtonPressed()
    {
        source.Play();
    }
}
