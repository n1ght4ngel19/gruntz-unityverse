using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
#if UNITY_EDITOR
using static VInspector.Libs.VUtils;
#endif


namespace VInspector
{
    public class ButtonAttribute : System.Attribute
    {
        public string name;

        public ButtonAttribute() => this.name = "";
        public ButtonAttribute(string name) => this.name = name;
    }

    public class ButtonSizeAttribute : System.Attribute
    {
        public float size;

        public ButtonSizeAttribute(float size) => this.size = size;
    }

    public class ButtonSpaceAttribute : System.Attribute
    {
        public float space;

        public ButtonSpaceAttribute() => this.space = 10;
        public ButtonSpaceAttribute(float space) => this.space = space;
    }



    public class RangeResettable : PropertyAttribute
    {
        public float min;
        public float max;

        public RangeResettable(float min, float max) { this.min = min; this.max = max; }
    }



    public class VariantsAttribute : PropertyAttribute
    {
        public string[] variants;

        public VariantsAttribute(params string[] variants) => this.variants = variants;
    }



    public class TabAttribute : System.Attribute
    {
        public string name;

        public TabAttribute(string name) => this.name = name;
    }

    public class EndTabAttribute : System.Attribute { }



    public class FoldoutAttribute : System.Attribute
    {
        public string name;

        public FoldoutAttribute(string name) => this.name = name;
    }

    public class EndFoldoutAttribute : System.Attribute { }



    public abstract class IfAttribute : System.Attribute
    {
        public string variableName;
        public object variableValue;

#if UNITY_EDITOR
        public bool Evaluate(object target) => target.GetMemberValue(variableName, false) is object curValue && curValue.Equals(variableValue);
#endif

        public IfAttribute(string boolName) { this.variableName = boolName; this.variableValue = true; }
        public IfAttribute(string variableName, object variableValue) { this.variableName = variableName; this.variableValue = variableValue; }
    }

    public class HideIfAttribute : IfAttribute
    {
        public HideIfAttribute(string boolName) : base(boolName) { }
        public HideIfAttribute(string variableName, object variableValue) : base(variableName, variableValue) { }
    }

    public class ShowIfAttribute : IfAttribute
    {
        public ShowIfAttribute(string boolName) : base(boolName) { }
        public ShowIfAttribute(string variableName, object variableValue) : base(variableName, variableValue) { }
    }

    public class EnableIfAttribute : IfAttribute
    {
        public EnableIfAttribute(string boolName) : base(boolName) { }
        public EnableIfAttribute(string variableName, object variableValue) : base(variableName, variableValue) { }
    }

    public class DisableIfAttribute : IfAttribute
    {
        public DisableIfAttribute(string boolName) : base(boolName) { }
        public DisableIfAttribute(string variableName, object variableValue) : base(variableName, variableValue) { }
    }

    public class EndIfAttribute : System.Attribute { }


}