using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogTrigger : MonoBehaviour
{
    [SerializeField] private Dialog dialog;

    public void TriggerDialog()
    {
        GetComponent<DialougeManager>().StartDialog(dialog);
    }
    //Triggering a Dialog with a button
}
