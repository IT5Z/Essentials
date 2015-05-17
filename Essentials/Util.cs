using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SDG;
using System.Reflection;
using Rocket.Unturned;
using UnityEngine;
using Essentials.Extensions;

namespace Essentials
{
    static class Util
    {
        public static string getPluginFilePath(string filename)
        {
            return string.Format("{0}Plugins/{1}/{2}", Implementation.Instance.HomeFolder, typeof(Plugin).Assembly.GetName().Name, filename);
        }

        public static void ResetItems()
        {
            ItemManager manager = ItemManagerExtensions.getItemManager();
            ItemRegion[,] region = ItemManagerExtensions.getItemRegion();
            for (byte b = 0; b < region.GetLength(0); b ++)
            {
                for (byte b2 = 0; b2 < region.GetLength(1); b2++)
                {
                    for (int num = region[b, b2].getItemData().Count; num >= 0; num--)
                    {
                        manager.SteamChannel.send("tellTakeItem", ESteamCall.CLIENTS, b, b2, 1, ESteamPacket.UPDATE_TCP_BUFFER, new object[] { b, b2, num });
                    }
                }
            }
            manager.resetItems();
            region = ItemManagerExtensions.getItemRegion();
            for (byte b = 0; b < region.GetLength(0); b++)
            {
                for (byte b2 = 0; b2 < region.GetLength(1); b2++)
                {
                    foreach (ItemData data in region[b, b2].getItemData())
                    {

                        manager.SteamChannel.send("tellItem", ESteamCall.CLIENTS, ESteamPacket.UPDATE_TCP_BUFFER, new object[] { b, b2, data.getItem().ItemID, data.getItem().Durability, data.getItem().Metadata, data.getVector3() });
                    }
                }
            }
        }

        public static T2 getField<T1, T2>(object obj, BindingFlags bindingAttr)
        {
            FieldInfo[] fields = typeof(T1).GetFields(bindingAttr);
            foreach(FieldInfo field in fields)
            {
                if(typeof(T2).Equals(field.FieldType))
                {
                    return (T2)field.GetValue(obj);
                }
            }
            return default(T2);
        }

        public static T2 getOffsetField<T1, T2>(object obj, BindingFlags bindingAttr, string name, byte offset)
        {
            FieldInfo[] fields = typeof(T1).GetFields(bindingAttr);
            int index;
            for (int i = 0; i < fields.Length; i++)
            {
                index = i + offset;
                if (index >= 0 && index < fields.Length && name.Equals(fields[i].Name) && typeof(T2).Equals(fields[index].FieldType))
                {
                    return (T2)fields[index].GetValue(obj);
                }
            }
            return default(T2);
        }

        public static object invokeMethod<T>(object obj, BindingFlags bindingAttr, Type returnType, Type[] pars, object[] args)
        {
            MethodInfo[] methods = typeof(T).GetMethods(bindingAttr);
            foreach (MethodInfo method in methods)
            {
                if (returnType.Equals(method.ReturnType) && Util.ArgsEquals(pars, method.GetParameters()))
                {
                    return method.Invoke(obj, args);
                }
            }
            return null;
        }

        public static bool ArgsEquals(Type[] pars, ParameterInfo[] args)
        {
            if (pars.Length != args.Length) return false;
            for (int i = 0; i < pars.Length; i++)
            {
                if (!pars[i].Equals(args[i].ParameterType))
                {
                    return false;
                }
            }
            return true;
        }

        public static float parsePosition(string expression, float origin)
        {
            Regex regex = new Regex(@"^~(([+-])(\d+))?$");
            if (regex.IsMatch(expression))
            {
                GroupCollection groups = regex.Match(expression).Groups;
                if (!string.IsNullOrEmpty(groups[1].Value))
                {
                    int number = int.Parse(groups[3].Value);
                    switch (groups[2].Value)
                    {
                        case "+":
                            return origin + number;
                        case "-":
                            return origin - number;
                        default:
                            return origin;
                    }
                }
                else
                {
                    return origin;
                }
            }
            else
            {
                return int.Parse(expression);
            }
        }
    }
}
