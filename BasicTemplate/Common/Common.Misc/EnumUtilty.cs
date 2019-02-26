using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Common.Misc
{
    public static class EnumUtilty
    {
        private static Dictionary<string, Dictionary<int, string>> _cacheEnumList = new Dictionary<string, Dictionary<int, string>>();

        /// <summary>获取枚举描述</summary>
        /// <param name="en"></param>
        /// <returns></returns>
        public static string GetEnumDescription(this Enum en)
        {
            MemberInfo[] member = en.GetType().GetMember(en.ToString());
            if (member != null && (uint)member.Length > 0U)
            {
                object[] customAttributes = member[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (customAttributes != null && (uint)customAttributes.Length > 0U)
                    return ((DescriptionAttribute)customAttributes[0]).Description;
            }
            return en.ToString();
        }

        /// <summary>获取枚举中所有成员</summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static XElement GetEnumList<T>()
        {
            FieldInfo[] fields = typeof(T).GetFields();
            XElement xelement1 = new XElement((XName)"table");
            foreach (FieldInfo fieldInfo in fields)
            {
                object[] customAttributes = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (customAttributes.Length == 1)
                {
                    DescriptionAttribute descriptionAttribute = (DescriptionAttribute)customAttributes[0];
                    string name = fieldInfo.Name;
                    int num = (int)fieldInfo.GetValue((object)null);
                    string str = string.IsNullOrEmpty(descriptionAttribute.Description) ? fieldInfo.Name : descriptionAttribute.Description;
                    XElement xelement2 = new XElement((XName)"row");
                    xelement2.Add((object)new XElement((XName)"name", (object)name));
                    xelement2.Add((object)new XElement((XName)"value", (object)num));
                    xelement2.Add((object)new XElement((XName)"description", (object)str));
                    xelement1.Add((object)xelement2);
                }
            }
            return xelement1;
        }

        /// <summary>枚举转字典</summary>
        /// <param name="enumType">typeof(枚举类型)</param>
        public static Dictionary<int, string> EnumToDictionary(Type enumType)
        {
            string fullName = enumType.FullName;
            if (!EnumUtilty._cacheEnumList.ContainsKey(fullName))
            {
                Dictionary<int, string> dictionary = new Dictionary<int, string>();
                foreach (int key in Enum.GetValues(enumType))
                {
                    string name = Enum.GetName(enumType, (object)key);
                    string str = "";
                    object[] customAttributes = enumType.GetField(name).GetCustomAttributes(typeof(DescriptionAttribute), false);
                    if (customAttributes != null && (uint)customAttributes.Length > 0U)
                        str = ((DescriptionAttribute)customAttributes[0]).Description;
                    dictionary.Add(key, string.IsNullOrEmpty(str) ? name : str);
                }
                object obj = new object();
                if (!EnumUtilty._cacheEnumList.ContainsKey(fullName))
                {
                    lock (obj)
                    {
                        if (!EnumUtilty._cacheEnumList.ContainsKey(fullName))
                            EnumUtilty._cacheEnumList.Add(fullName, dictionary);
                    }
                }
            }
            return EnumUtilty._cacheEnumList[fullName];
        }

        public static T StringToEnum<T>(this string str)
        {
            return (T)Enum.Parse(typeof(T), str);
        }
    }
}
