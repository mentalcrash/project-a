using System;
using UnityEngine.UI;

namespace MC.Project.UI.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class UIClassAttribute : Attribute
    {
        
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class UIFieldAttribute : Attribute
    {
        public readonly string path;

        public UIFieldAttribute(string path)
        {
            this.path = path;
        }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public abstract class UIMethodAttribute : Attribute
    {
        public readonly string path;
        public abstract Type targetType { get; }

        public UIMethodAttribute(string path)
        {
            this.path = path;
        }
    }

    public class UIButtonAttribute : UIMethodAttribute
    {
        public override Type targetType => typeof(Button);

        public UIButtonAttribute(string path) : base(path)
        {
        }
    }
}