using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gear.Manager.Core.Extensions.EntityComparison
{
    public static class UpdateExtension
    {
        public static IEnumerable<string> EnumeratePropertyDifferences<T, TContext>(this T obj1, T obj2, TContext context) where TContext : IGearContext
        {
            var properties = typeof(T).GetProperties();
            var changes = new List<string>();

            var mapping = context.Model.FindEntityType(obj1.GetType()).Relational();
            var mainTableName = mapping.TableName;

            var foreignKeysMetaData = context.Model.FindEntityType(obj1.GetType()).GetForeignKeys();
            var foreignKeyList = foreignKeysMetaData.Select(foreignKey => new ForeignKeyListItem()
            {
                FkeyName = foreignKey.Properties.Select(x => x.Name).First(),
                Model = foreignKey.PrincipalEntityType.ClrType,
                TableName = context.Model.FindEntityType(foreignKey.PrincipalEntityType.ClrType).Relational().TableName
            }).ToList();


            foreach (var pi in properties)
            {
                var value1 = typeof(T).GetProperty(pi.Name)?.GetValue(obj1, null);
                var value2 = typeof(T).GetProperty(pi.Name)?.GetValue(obj2, null);

                if (value1!= null && !IsSimple(value1.GetType())) continue;
                if (value1 != value2 && (value1 == null || !value1.Equals(value2)))
                {
                    if (foreignKeyList.Any(x => x.FkeyName == pi.Name))
                    {
                        var entityType = foreignKeyList.First(x => x.FkeyName == pi.Name);
                        var firstValue = "none";

                        //Optional Fields Fix
                        var fieldName = entityType.Model.GetProperties().First(x => x.Name.ToString().ToLower() == "name" 
                                                                                    || x.Name.ToString().ToLower() == "username" ).Name;
                        if (value1 != null) firstValue = (string)GetPropertyValue(context.Find(entityType.Model, value1), fieldName);
                        var secondValue = "none";
                        if (value2 != null)
                            secondValue = (string)GetPropertyValue(context.Find(entityType.Model, value2), fieldName);
                        changes.Add($"Property {pi.Name} was changed from: {firstValue} to {secondValue} {Environment.NewLine} ");
                    }
                    else
                    {
                        changes.Add($"Property {pi.Name} was changed from {value1} to {value2} {Environment.NewLine}");
                    }
                }

            }
            return changes;
        }

        private static object GetPropertyValue(this object type, string propertyName)
        {
            return type.GetType().GetProperties()
                .Single(pi => pi.Name == propertyName)
                .GetValue(type, null);
        }

        private static bool IsSimple(Type type)
        {
            var typeInfo = type.GetTypeInfo();
            if (typeInfo.IsGenericType && typeInfo.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                // nullable type, check if the nested type is simple.
                return IsSimple(typeInfo.GetGenericArguments()[0]);
            }

            return typeInfo.IsPrimitive
                   || typeInfo.IsEnum
                   || type.Equals(typeof(string))
                   || type.Equals(typeof(decimal))
                   || type.Equals(typeof(Guid));
        }


    }
}
