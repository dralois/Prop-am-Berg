using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasCtrl : MonoBehaviour
{
    Service<PropController.GuiUpdate> guiUpdate;
    [SerializeField] PropController.PlayerIndex pIndex;
    private Image[] _images;


    private Canvas canvas;
    // Start is called before the first frame update
    void Start()
    {
        _images = GetComponentsInChildren<Image>();
        canvas = GetComponent<Canvas>();
        ServiceLocator<PropController.GuiUpdate, PropController.PlayerIndex>.OnServiceAdded += AddService;
    }

    // Update is called once per frame
    void Update()
    {
        if(guiUpdate != null)
        {
            if (guiUpdate.GetData()._buttonPressed)
            {
                canvas.enabled = true;
            }
            else
            {
                canvas.enabled = false;
            }
        }
    }
    void AddService(PropController.PlayerIndex p)
    {
        if(p == pIndex)
        {
            guiUpdate = ServiceLocator<PropController.GuiUpdate, PropController.PlayerIndex>.GetService(pIndex);
            for(int i = 0; i< guiUpdate.GetData()._spriteArray.Length; i++)
            {
                _images[i].sprite = guiUpdate.GetData()._spriteArray[i];
                _images[i].enabled = true;
            }
        }

    }
}
