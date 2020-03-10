namespace System
{
  static public class EnumDescriptionAttributeExtension
  {
    /// <summary>
    /// https://stackoverflow.com/questions/1415140/can-my-enums-have-friendly-names#1415187
    /// 
    /// 
    /// See: <see cref="System.ComponentModel.DescriptionAttribute" />
    /// </summary>
    public static string GetEnumDescriptionAttribute(this Enum value)
    {
      Type type = value.GetType();
      string name = Enum.GetName(type, value);
      if (name != null)
      {
        Reflection.FieldInfo field = type.GetField(name);
        if (field != null)
        {
          ComponentModel.DescriptionAttribute attr = Attribute.GetCustomAttribute(field, typeof(ComponentModel.DescriptionAttribute)) as ComponentModel.DescriptionAttribute;
          if (attr != null) return attr.Description;
        }
      }
      return null;
    }
  }
}
