using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DragAndDropFile : MonoBehaviour
{
    void OnGUI()
    {
        // Get the current event
        Event evt = Event.current;
        switch (evt.type)
        {
            case EventType.DragUpdated:
            case EventType.DragPerform:
                // Accept drag and drop
                DragAndDrop.AcceptDrag();
                if (evt.type == EventType.DragPerform)
                {
                    // Show paths of dragged files
                    foreach (string path in DragAndDrop.paths)
                    {
                        Debug.Log(path);
                    }
                }
                break;
        }
    }
}
