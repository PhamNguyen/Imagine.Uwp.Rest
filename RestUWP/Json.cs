using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Data.Json;
using System.Reflection;
using JsonUWP.Attributes;
using System.Collections;
using System.Diagnostics;

namespace JsonUWP
{
    public static class TypeExtensions
    {
        public static bool IsTypeOf<T>(this Type type)
        {
            return typeof(T) == type;
        }
    }

    public static class Json
    {
        public static T Parse<T>(string data)
        {
            var result = default(T);

            if (string.IsNullOrEmpty(data))
                return result;

            var type = typeof(T);
            var typeInfo = type.GetTypeInfo();
            if (typeInfo.IsClass || typeInfo.IsInterface)
            {
                var types = typeInfo.ImplementedInterfaces;

                var collection = types.FirstOrDefault(p => p.FullName.Contains("Generic") || p.FullName.Contains("IList") || p.FullName.Contains("Collection") || p.FullName.Contains("IEnumerable"));

                if (collection != null)
                {
                    var itemType = type.GetGenericArguments().FirstOrDefault();

                    var items = (IList)Activator.CreateInstance(type);

                    var jsonArray = JsonArray.Parse(data);

                    foreach (var json in jsonArray)
                    {
                        var item = ConvertObject(itemType, json.ToString());
                        items.Add(item);

                    }
                    result = (T)items;
                }
                else
                {
                    result = (T)ConvertObject(type, data);
                }

            }
            else
            {
                result = (T)ConvertObject(type, data);
            }

            return result;
        }

        public static object ConvertObject(Type type, string data)
        {
            if (string.IsNullOrEmpty(data)
                || (!data.StartsWith("{") && !data.EndsWith("}"))
                || data.ToLower().Equals("null")
                || data.ToLower().Equals("[]")
                || data.ToLower().Equals("{}"))
            {
                return null;
            }

            JsonObject jsonObject = JsonObject.Parse(data);

            //if(!JsonObject.TryParse(data, out jsonObject))
            //{
            //    return null;
            //}

            var result = Activator.CreateInstance(type);

            var properties = type.GetProperties();
            Dictionary<int, bool> checkedIndexs = new Dictionary<int, bool>();
            int count = properties.Length;
            foreach (var json in jsonObject)
            {
               // Debug.WriteLine($"{json.Key} : {json.Value.ToString()}");

                if (count == 0)
                    break;
                for (int i = 0; i < count; i++)
                {
                    // Check Impelement checked

                    bool isChecked = false;
                    if (checkedIndexs.ContainsKey(i))
                        isChecked = checkedIndexs[i];
                    if (isChecked) continue;

                    // Check Property Sign
                    var property = properties.GetValue(i) as PropertyInfo;
                    var sign = property.GetCustomAttribute<SignAttribute>();
                    if (sign == null)
                        continue;

                    if (json.Key.Equals(sign.Name))
                    {
                        if (property != null)
                        {
                            var propertyType = property.PropertyType;
                            var typeInfo = propertyType.GetTypeInfo();
                            IJsonValue jsonValue = json.Value;

                            //Debug.Write(jsonValue.ValueType);

                            if (typeInfo.IsValueType)
                            {
                                if (propertyType.IsTypeOf<int>())
                                {
                                    if (jsonValue.ValueType == JsonValueType.Null)
                                    {
                                        property.SetValue(result,
                                                  Convert.ChangeType(0, propertyType));
                                    }
                                    else if (jsonValue.ValueType == JsonValueType.Number)
                                        property.SetValue(result,
                                            Convert.ChangeType(jsonValue.GetNumber(), propertyType));
                                    else
                                    {
                                        int number;
                                        property.SetValue(result,
                                            int.TryParse(jsonValue.GetString(), out number)
                                                ? Convert.ChangeType(number, propertyType)
                                                : Convert.ChangeType(0, propertyType));
                                    }
                                }
                                else if (propertyType.IsTypeOf<bool>())
                                {
                                    if (jsonValue.ValueType == JsonValueType.Null)
                                    {
                                        property.SetValue(result,
                                                  Convert.ChangeType(false, propertyType));
                                    }
                                    else if (jsonValue.ValueType == JsonValueType.Boolean)
                                        property.SetValue(result, Convert.ChangeType(jsonValue.GetBoolean(), propertyType));
                                    else
                                    {
                                        bool boolean;
                                        property.SetValue(result,
                                            bool.TryParse(jsonValue.GetString(), out boolean)
                                                ? Convert.ChangeType(boolean, propertyType)
                                                : Convert.ChangeType(0, propertyType));
                                    }
                                }
                                else if (propertyType.IsTypeOf<long>())
                                {
                                    if (jsonValue.ValueType == JsonValueType.Null)
                                    {
                                        property.SetValue(result,
                                                  Convert.ChangeType(0, propertyType));
                                    }
                                    else if (jsonValue.ValueType == JsonValueType.String || jsonValue.ValueType == JsonValueType.Object)
                                    {
                                        long number;
                                        property.SetValue(result,
                                            long.TryParse(jsonValue.GetString(), out number)
                                                ? Convert.ChangeType(number, propertyType)
                                                : Convert.ChangeType(0, propertyType));
                                    }
                                    else
                                        property.SetValue(result, Convert.ChangeType(jsonValue.GetNumber(), propertyType));
                                }
                                else if (propertyType.IsTypeOf<double>())
                                {
                                    if (jsonValue.ValueType == JsonValueType.Null)
                                    {
                                        property.SetValue(result,
                                                  Convert.ChangeType(0, propertyType));
                                    }
                                    else if (jsonValue.ValueType == JsonValueType.String || jsonValue.ValueType == JsonValueType.Object)
                                    {
                                        double number;
                                        property.SetValue(result,
                                            double.TryParse(jsonValue.GetString(), out number)
                                                ? Convert.ChangeType(number, propertyType)
                                                : Convert.ChangeType(0, propertyType));
                                    }
                                    else
                                        property.SetValue(result, Convert.ChangeType(jsonValue.GetNumber(), propertyType));
                                }
                                else if (propertyType.IsTypeOf<byte>())
                                {
                                    if (jsonValue.ValueType == JsonValueType.Null)
                                    {
                                        property.SetValue(result,
                                                  Convert.ChangeType(0, propertyType));
                                    }
                                    else if (jsonValue.ValueType == JsonValueType.String || jsonValue.ValueType == JsonValueType.Object)
                                    {
                                        byte number;
                                        property.SetValue(result,
                                            byte.TryParse(jsonValue.GetString(), out number)
                                                ? Convert.ChangeType(number, propertyType)
                                                : Convert.ChangeType(0, propertyType));
                                    }
                                }
                            }
                            else if (propertyType.IsTypeOf<string>() && jsonValue.ValueType == JsonValueType.String)
                            {
                                if (jsonValue.ValueType == JsonValueType.Null)
                                {
                                    property.SetValue(result,
                                              Convert.ChangeType(String.Empty, propertyType));
                                }
                                else if (jsonValue.ValueType == JsonValueType.String)
                                {
                                    property.SetValue(result, Convert.ChangeType(jsonValue.GetString(), propertyType));
                                }
                                else if (jsonValue.ValueType == JsonValueType.Number)
                                {
                                    property.SetValue(result, Convert.ChangeType(jsonValue.GetNumber().ToString(), propertyType));
                                }
                                else if (jsonValue.ValueType == JsonValueType.Boolean)
                                {
                                    property.SetValue(result, Convert.ChangeType(jsonValue.GetBoolean().ToString(), propertyType));
                                }
                                else if (jsonValue.ValueType == JsonValueType.Object)
                                {
                                    property.SetValue(result, Convert.ChangeType(jsonValue.GetObject().ToString(), propertyType));
                                }

                            }
                            else if (typeInfo.IsClass)
                            {
                                // generic
                                if (typeInfo.IsGenericType)
                                {
                                    var types = typeInfo.ImplementedInterfaces;

                                    var collection = types.FirstOrDefault(p => p.FullName.Contains("Generic") || p.FullName.Contains("IList") || p.FullName.Contains("Collection") || p.FullName.Contains("IEnumerable"));

                                    if (collection != null) // Generic Collection
                                    {
                                        var itemType = propertyType.GetGenericArguments().FirstOrDefault();

                                        string arrayData = json.Value != null ? json.Value.ToString() : string.Empty;

                                        if (!string.IsNullOrEmpty(arrayData))
                                        {
                                            var items = (IList)Activator.CreateInstance(propertyType);

                                            var jsonArray = JsonArray.Parse(arrayData);
                                            //     List<object> data = new List<Object>();
                                            foreach (var _jsonValue in jsonArray)
                                            {
                                                var item = ConvertObject(itemType, _jsonValue.ToString());
                                                items.Add(item);
                                                //      data.Add(item);
                                            }
                                            property.SetValue(result, Convert.ChangeType(items, propertyType));
                                        }
                                    }
                                    else // Generic class
                                    {
                                        property.SetValue(result, Convert.ChangeType(ConvertObject(propertyType, json.Value.ToString()), propertyType));
                                    }
                                }
                                else // reference class
                                {
                                    //property.SetValue(result, Convert.ChangeType(jsonValue.GetString(), propertyType));
                                    property.SetValue(result, Convert.ChangeType(ConvertObject(propertyType, json.Value.ToString()), propertyType));
                                }
                            }
                        }

                        checkedIndexs[i] = true;
                        break;
                    }

                }
            }

            return result;
        }
    }
}