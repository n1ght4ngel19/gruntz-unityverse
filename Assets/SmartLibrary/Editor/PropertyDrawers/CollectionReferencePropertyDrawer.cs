using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Bewildered.SmartLibrary.UI
{
    [CustomPropertyDrawer(typeof(CollectionReference<>))]
    internal class CollectionReferencePropertyDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            CollectionField field = new CollectionField();
            field.BindProperty(property.FindPropertyRelative("_collection"));
            field.RegisterValueChangedCallback(evt =>
            {
                var guidBytesProperty = property.FindPropertyRelative("_id._serializedGuid");
                if (evt.newValue is LibraryCollection newCollection)
                {
                    byte[] newGuidBytes = newCollection.ID.ToByteArray();
                    guidBytesProperty.arraySize = newGuidBytes.Length;
                    for (int i = 0; i < newGuidBytes.Length; i++)
                    {
                        guidBytesProperty.GetArrayElementAtIndex(i).intValue = newGuidBytes[i];
                    }
                }
                else
                {
                    guidBytesProperty.ClearArray();
                    guidBytesProperty.arraySize = 16;
                }
            });

            return field;
        }
    }
}
