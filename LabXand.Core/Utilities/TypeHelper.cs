using System;
using System.Collections.Generic;
using System.Reflection;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Globalization;
using System.IO;
using Fasterflect;
using LabXand.Extensions;

namespace LabXand.Core
{
    public static class TypeHelper
    {
        public delegate T Creator<out T>();

        public static bool IsUnitializedValue(object value)
        {
            if (value == null)
            {
                return true;
            }
            object unitializedValue = CreateUnitializedValue(value.GetType());
            return value.Equals(unitializedValue);
        }

        public static object CreateUnitializedValue(Type type)
        {
            Guard.ArgumentNotNull(type, "type");

            if (type.IsGenericTypeDefinition)
                throw new ArgumentException("Type {0} is a generic type definition and cannot be instantiated.".FormatWith(CultureInfo.InvariantCulture, type), "type");

            if (type.IsClass || type.IsInterface || type == typeof(void))
                return null;
            if (type.IsValueType)
                return Activator.CreateInstance(type);
            throw new ArgumentException("Type {0} cannot be instantiated.".FormatWith(CultureInfo.InvariantCulture, type), "type");
        }

        /////// <summary>
        /////// Gets the type of the typed list's items.
        /////// </summary>
        /////// <param name="type">The list type.</param>
        /////// <returns>The type of the typed list's items.</returns>
        //public static Type GetListItemType(Type type)
        //{
        //    Guard.ArgumentNotNull(type, "type");
        //    Type genericListType;

        //    if (type.IsArray)
        //    {
        //        return type.GetElementType();
        //    }
        //    else if (type.IsSubClass(typeof(IList<>), out genericListType))
        //    {
        //        if (genericListType.IsGenericTypeDefinition)
        //            throw new Exception("Type {0} is not a list.".FormatWith(CultureInfo.InvariantCulture, type));

        //        return genericListType.GetGenericArguments()[0];
        //    }
        //    else if (typeof(IList).IsAssignableFrom(type))
        //    {
        //        return null;
        //    }
        //    else
        //    {
        //        throw new Exception("Type {0} is not a list.".FormatWith(CultureInfo.InvariantCulture, type));
        //    }
        //}

        public static Type GetElementType(Type type)
        {
            if (!type.IsPredefinedSimpleType())
            {
                if (type.HasElementType)
                {
                    return GetElementType(type.GetElementType());
                }
                if (type.IsPredefinedGenericType())
                {
                    return GetElementType(type.GetGenericArguments()[0]);
                }
                Type type2 = type.FindIEnumerable();
                if (type2 != null)
                {
                    Type type3 = type2.GetGenericArguments()[0];
                    return GetElementType(type3);
                }
            }
            return type;
        }

        public static void GetDictionaryKeyValueTypes(Type dictionaryType, out Type keyType, out Type valueType)
        {
            Guard.ArgumentNotNull(dictionaryType, "type");

            Type genericDictionaryType;
            if (dictionaryType.IsSubClass(typeof(IDictionary<,>), out genericDictionaryType))
            {
                if (genericDictionaryType.IsGenericTypeDefinition)
                    throw new Exception("Type {0} is not a dictionary.".FormatInvariant(dictionaryType));

                Type[] dictionaryGenericArguments = genericDictionaryType.GetGenericArguments();

                keyType = dictionaryGenericArguments[0];
                valueType = dictionaryGenericArguments[1];
                return;
            }
            if (!typeof(IDictionary).IsAssignableFrom(dictionaryType))
                throw new Exception("Type {0} is not a dictionary.".FormatInvariant(dictionaryType));
            keyType = null;
            valueType = null;
        }

        public static Type GetDictionaryValueType(Type dictionaryType)
        {
            Type keyType;
            Type valueType;
            GetDictionaryKeyValueTypes(dictionaryType, out keyType, out valueType);

            return valueType;
        }

        public static Type GetDictionaryKeyType(Type dictionaryType)
        {
            Type keyType;
            Type valueType;
            GetDictionaryKeyValueTypes(dictionaryType, out keyType, out valueType);

            return keyType;
        }

        /// <summary>
        /// Tests whether the list's items are their unitialized value.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <returns>Whether the list's items are their unitialized value</returns>
        public static bool ItemsUnitializedValue<T>(IList<T> list)
        {
            Guard.ArgumentNotNull(list, "list");

            Type elementType = GetElementType(list.GetType()); // GetListItemType(list.GetType());

            if (elementType.IsValueType)
            {
                object unitializedValue = CreateUnitializedValue(elementType);

                return list.All(t => t.Equals(unitializedValue));
            }
            if (elementType.IsClass)
            {
                return list.Cast<object>().All(value => value == null);
            }
            throw new Exception("Type {0} is neither a ValueType or a Class.".FormatWith(CultureInfo.InvariantCulture, elementType));
        }

        /// <summary>
        /// Gets the member's underlying type.
        /// </summary>
        /// <param name="member">The member.</param>
        /// <returns>The underlying type of the member.</returns>
        public static Type GetMemberUnderlyingType(MemberInfo member)
        {
            Guard.ArgumentNotNull(member, "member");

            switch (member.MemberType)
            {
                case MemberTypes.Field:
                    return ((FieldInfo)member).FieldType;
                case MemberTypes.Property:
                    return ((PropertyInfo)member).PropertyType;
                case MemberTypes.Event:
                    return ((EventInfo)member).EventHandlerType;
                default:
                    throw new ArgumentException("MemberInfo must be if type FieldInfo, PropertyInfo or EventInfo", "member");
            }
        }

        public static IList<string> GetPropertyNames(object target)
        {
            return target.GetType().GetProperties().Select(z => z.Name).ToList();
        }

        public static IEnumerable<MemberInfo> GetFieldsAndProperties<T>(BindingFlags bindingAttr)
        {
            return typeof(T).GetFieldsAndProperties(bindingAttr);
        }

        public static T GetAttribute<T>(ICustomAttributeProvider attributeProvider) where T : Attribute
        {
            return GetAttribute<T>(attributeProvider, true);
        }

        public static T GetAttribute<T>(ICustomAttributeProvider attributeProvider, bool inherit) where T : Attribute
        {
            T[] attributes = GetAttributes<T>(attributeProvider, inherit);
            return attributes.FirstOrDefault();
        }

        public static T[] GetAttributes<T>(ICustomAttributeProvider attributeProvider, bool inherit) where T : Attribute
        {
            Guard.ArgumentNotNull(attributeProvider, "attributeProvider");

            return (T[])attributeProvider.GetCustomAttributes(typeof(T), inherit);
        }

        /// <summary>
        /// Gets the value of a property through reflection.
        /// </summary>
        /// <param name="from">The <see cref="object"/> to get the value from.</param>
        /// <param name="propertyName">The name of the property to extract the value for.</param>
        /// <returns>The value of the property.</returns>
        public static object GetPropertyValue(object from, string propertyName)
        {
            Guard.ArgumentNotNull(from, "value");
            var propertyInfo = from.GetType().GetProperty(propertyName,
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic);
            return propertyInfo.GetValue(from, null);
        }

        public static IEnumerable GetCollectionPropertyValue(object from, string propertyName)
        {
            Guard.ArgumentNotNull(from, "value");
            var propertyInfo = from.GetType().GetProperty(propertyName,
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic);
            return (IEnumerable)propertyInfo.GetValue(from, null);
        }

        public static bool TryAction<T>(Creator<T> creator, out T output)
        {
            Guard.ArgumentNotNull(creator, "creator");

            try
            {
                output = creator();
                return true;
            }
            catch
            {
                output = default(T);
                return false;
            }
        }

        public static bool TryGetDescription(object value, out string description)
        {
            return TryAction(() => GetDescription(value), out description);
        }

        public static string GetDescription(object o)
        {
            Guard.ArgumentNotNull(o, "o");

            ICustomAttributeProvider attributeProvider = o as ICustomAttributeProvider;

            // object passed in isn't an attribute provider
            // if value is an enum value, get value field member, otherwise get values type
            if (attributeProvider == null)
            {
                Type valueType = o.GetType();

                if (valueType.IsEnum)
                    attributeProvider = valueType.GetField(o.ToString());
                else
                    attributeProvider = valueType;
            }

            DescriptionAttribute descriptionAttribute = GetAttribute<DescriptionAttribute>(attributeProvider);

            if (descriptionAttribute != null)
                return descriptionAttribute.Description;
            throw new Exception("No DescriptionAttribute on '{0}'.".FormatWith(CultureInfo.InvariantCulture, o.GetType()));
        }

        public static IList<string> GetDescriptions(IList values)
        {
            Guard.ArgumentNotNull(values, "values");

            string[] descriptions = new string[values.Count];

            for (int i = 0; i < values.Count; i++)
            {
                descriptions[i] = GetDescription(values[i]);
            }

            return descriptions;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">The type from which to infer namespace name.</param>
        /// <param name="resourceName">The relative name of the resource, without (default) namespace name.</param>
        /// <returns></returns>
        public static string GetStringFromEmbeddedResource(Type type, string resourceName)
        {
            Assembly assembly = type.Assembly;
            resourceName = String.Concat(type.Namespace, ".", resourceName);
            return GetStringFromEmbeddedResource(assembly, resourceName);
        }

        public static string GetStringFromEmbeddedResource(Assembly assembly, string resourceFullName)
        {
            using (Stream stream = assembly.GetManifestResourceStream(resourceFullName))
            {
                try
                {
                    return stream.AsString();
                    //using (StreamReader reader = new StreamReader(stream))
                    //{
                    //    return reader.ReadToEnd();
                    //}
                }
                catch (Exception e)
                {
                    throw new Exception("Error retrieving from Resources. Tried '" + resourceFullName + "'\r\n" + e);
                }
            }
        }

        public static Type GetSequenceType(Type elementType)
        {
            return typeof(IEnumerable<>).MakeGenericType(elementType);
        }

        public static Type GetMemberType(MemberInfo mi)
        {
            FieldInfo fi = mi as FieldInfo;
            if (fi != null)
                return fi.FieldType;
            PropertyInfo pi = mi as PropertyInfo;
            if (pi != null)
                return pi.PropertyType;
            EventInfo ei = mi as EventInfo;
            if (ei != null)
                return ei.EventHandlerType;
            return null;
        }

        public static void RegisterTypeConverter<T, TC>() where TC : TypeConverter
        {
            TypeDescriptor.AddAttributes(typeof(T), new TypeConverterAttribute(typeof(TC)));
        }

        public static T CreateInstance<T>(params object[] parameters) where T : class
        {
            return (T)typeof(T).CreateInstance(parameters);
        }

        public static object CreateInstance(Type type, params object[] parameters)
        {
            return type.CreateInstance(parameters);
        }

        public static Type FindImplimentedSpecificInterface<T>(string assemblyName)
        {
            Assembly targetAssembly = Assembly.Load(assemblyName);
            Type interfaceType = typeof(T);
            if (targetAssembly == null) return null;
            if (!interfaceType.IsGenericType)
            {
                Type targetType = targetAssembly.GetTypes().FirstOrDefault(type => type.GetInterface(interfaceType.Name) != null);
                return targetType;
            }
            IEnumerable<Type> targetTypes = targetAssembly.GetTypes().Where(type => type.GetInterface(interfaceType.Name) != null);
            var y = targetTypes.Where(t => t.BaseType.GetGenericArguments().EqualsAll(interfaceType.GetGenericArguments().ToList())).ToList();
            return y.First();
        }

        private static void SetPropertyValue(object inputObject, object value, PropertyInfo property)
        {
            if (property != null && property.CanWrite)
            {
                try
                {
                    property.SetValue(inputObject, value, null);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public static void SetPropertyValues(object[] inputObjects, string propertyName, object value)
        {
            PropertyInfo property = null;
            foreach (object item in inputObjects)
            {
                if (property == null)
                    property = GetPropertyInfo(item, propertyName);
                SetPropertyValue(item, value, property);
            }
        }

        public static void SetPropertyValue(object inputObject, string propertyName, object value)
        {
            PropertyInfo property = GetPropertyInfo(inputObject, propertyName);
            if (property != null)
                SetPropertyValue(inputObject, value, property);
            else
                throw new Exception(string.Format("{0} data type don't have {1} property.", inputObject.GetType().Name, propertyName));

        }

        private static PropertyInfo GetPropertyInfo(object inputObject, string propertyName)
        {
            if (inputObject != null)
                return inputObject.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            return null;
        }

        public static TResult InvokeMethod<TResult, T>(Type type, T instance, string methodName, Type[] types, object[] args)
        {
            MethodInfo method = type.GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, types, null);
            if (method != null)
            {
                try
                {
                    var t = (TResult)method.Invoke(instance, args);
                    return t;
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null)
                    {
                        throw ex.InnerException;
                    }
                    throw;
                }
            }
            else
                throw new Exception(string.Format("{0} does not containe {1} method.", type.Name, methodName));
        }

        public static object InvokeGenericMethod(Type type, string methodName, object obj, object[] arguments, params Type[] genericTypes)
        {
            MethodInfo method = type.GetMethod(methodName);
            return method.MakeGenericMethod(genericTypes).Invoke(obj, arguments);
        }

        public static object InvokeGenericMethod(Type type, string methodName, object obj, Type[] argumentTypes, object[] arguments, params Type[] genericTypes)
        {
            MethodInfo method = type.GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, argumentTypes, null);
            return method.MakeGenericMethod(genericTypes).Invoke(obj, arguments);
        }

        public static bool IsPropertyACollection(object inputObject, string propertyName)
        {
            PropertyInfo prop = GetPropertyInfo(inputObject, propertyName);
            if (prop != null)
                return IsPropertyACollection(prop);
            return false;
        }
        public static bool IsPropertyACollection(PropertyInfo property)
        {
            return property.PropertyType.GetInterface(typeof(IEnumerable<>).FullName) != null;
        }

        public static object GetDefaultValue(Type type)
        {
            if (type.IsValueType)
                return Activator.CreateInstance(type);

            return null;
        }
        public static MethodInfo GetEnumerableAnyMethod(Type genericType)
        {
            return typeof(Enumerable).GetMethods().Where(m => m.Name == "Any").FirstOrDefault(m => m.GetParameters().Count() == 2).MakeGenericMethod(genericType);
        }

        public static string GetPropertyDisplayName<T>(string propertyName)
        {
            var memberInfo = ExpressionHelper.GetMemberExpression<T>(propertyName).Member;
            //var memberInfo = typeof(T).GetMember(propertyName).FirstOrDefault();
            if (memberInfo == null)
            {
                throw new ArgumentException(
                    string.Format("No property with {0} name was found.", propertyName),
                    "propertyName");
            }

            var attr = memberInfo.GetAttribute<DisplayNameAttribute>(false);
            if (attr == null)
            {
                return memberInfo.Name;
            }

            return attr.DisplayName;
        }
    }
}