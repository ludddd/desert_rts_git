using System;

namespace editor.attr
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class ButtonAttribute: Attribute
    {
    }
}
