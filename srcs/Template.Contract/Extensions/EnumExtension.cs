﻿using System.ComponentModel.DataAnnotations;

namespace Template.Contract.Extensions;

public static class EnumExtension
{
    //This extension method is broken out so you can use a similar pattern with
    // other MetaData elements in the future. This is your base method for each.
    private static T GetAttribute<T>(this Enum value) where T : Attribute
    {
        var type = value.GetType();
        var memberInfo = type.GetMember(value.ToString());
        var attributes = memberInfo[0].GetCustomAttributes(typeof(T), false);
        return (T)attributes[0];
    }

    // This method creates a specific call to the above method, requesting the
    // Description MetaData attribute.
    public static string ToName(this Enum value)
    {
        var attribute = value.GetAttribute<DisplayAttribute>();
        return attribute == null ? value.ToString() : attribute.Name;
    }
}
