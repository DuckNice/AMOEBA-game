using UnityEditor;
using System.Collections;
using UnityEngine;

public class NPCCreatorWindow : EditorWindow {
    [MenuItem ("Window/NPCCreator")]

    public static void ShowWindow()
    {
        GetWindow(typeof(NPCCreatorWindow));
    }


    void OnGUI()
    {
        DragPersonGUI();
    }

    void DragPersonGUI(Rect dropArea, SerializedProperty property)
    {
        Event evt = Event.current;
        Rect drop_area = GUILayoutUtility.GetRect(0.0f, 50.0f, GUILayout.ExpandWidth(true));
        GUI.Box(drop_area, "Add Trigger");
        
        switch (evt.type)
        {
            case EventType.MouseDown:
                DragAndDrop.PrepareStartDrag();

                CustomDragData dragData = new CustomDragData();
                dragData.originalIndex = ;
                dragData.originalList = this.targetList;
                DragAndDrop.SetGenericData(dragDropIdentifier, dragData);

                Object[] objectReferences;
                if (property.objectReferenceValue != null)
                {
                    objectReferences = new Object[1] { property.objectReferenceValue };
                }
                else
                {
                    objectReferences = new Object[0];
                }
                DragAndDrop.objectReferences = objectReferences;

                evt.Use();

                break;

            case EventType.MouseDrag:
                CustomDragData existingDragData = DragAndDrop.GetGenericData(dragDropIdentifier) as CustomDragData;

                if(existingDragData != null)
                {
                    DragAndDrop.StartDrag("DraggingListElement");
                    evt.Use();
                }
                break;

            case EventType.DragExited:
                DragAndDrop.PrepareStartDrag();
                break;

            case EventType.DragUpdated:
                if (IsDragTargetValid())
                    DragAndDrop.visualMode = DragAndDropVisualMode.Link;
                else
                    DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;

                evt.Use();
                break;

            case EventType.Repaint:
                if (DragAndDrop.visualMode == DragAndDropVisualMode.None || DragAndDrop.visualMode == DragAndDropVisualMode.Rejected)
                    break;

                EditorGUI.DrawRect(dropArea, Color.grey);
                break;
            case EventType.DragPerform:
                DragAndDrop.AcceptDrag();

                CustomDragData receivedDragData = DragAndDrop.GetGenericData(dragDropIdentifier) as CustomDragData;

                if(receivedDragData != null receivedDragData.originalList == this.targetList)
                    ReorderObject();

                else 
                    AddDraggedObjectsToList();

                evt.Use();

            case EventType.MouseUp:
                DragAndDrop.PrepareStartDrag();
                break;
            
        }
    }


}

public class CustomDragData
{
    public int originalIndex;
    public IList originalList;
}