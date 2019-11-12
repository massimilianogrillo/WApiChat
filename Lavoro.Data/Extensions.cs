using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Lavoro.Data
{
    public static class DbSetExtensions
    {
        public static T AddIfNotExists<T>(this DbSet<T> dbSet, T entity, Expression<Func<T, bool>> predicate) where T : class, new()
        {
            var element = dbSet.Where(predicate).FirstOrDefault();
            if (element == null)
            {
                dbSet.Add(entity);
                return entity;
            }
            return element;

        }

        public static void AddIfNotExists<T, TKey>(this DbSet<T> dbSet, List<T> entites, List<Func<T, TKey>> predicates) where T : class, new()
        {
            var db = dbSet.ToList();
            var list = new List<T>();
            foreach(var entity in entites)
            {
                if (!db.Exists(predicates, entity))
                    list.Add(entity);
            }
            dbSet.AddRange(list);
            
        }

        public static void AddRangeIfNotExists<TEnt, TKey>(this DbSet<TEnt> dbSet, IEnumerable<TEnt> entities, Func<TEnt, TKey> predicate) where TEnt : class
        {
            var entitiesExist = from ent in dbSet
                                where entities.Any(add => predicate(ent).Equals(predicate(add)))
                                select ent;

            dbSet.AddRange(entities.Except(entitiesExist));
        }

        public static void AddRangeIfNotExists<TEnt, TKey>(this DbSet<TEnt> dbSet, IEnumerable<TEnt> entities, List<Func<TEnt, TKey>> predicates) where TEnt : class
        {
            var entitiesExist = (from ent in dbSet
                                where entities.Exists(predicates, ent)
                                select ent).ToList();
            var entitiesToAdd = entities.Except(entitiesExist);
            dbSet.AddRange(entitiesToAdd);
        }
        private static bool Exists<TEnt, TKey>(this IEnumerable<TEnt> entities, IEnumerable<Func<TEnt, TKey>> predicates, TEnt add) where TEnt : class
        {
            var exists = entities.Any(ent => Check(predicates, ent, add));
            return exists;
        }

        private static bool Check<TEnt, TKey>(IEnumerable<Func<TEnt, TKey>> predicates, TEnt ent, TEnt add) where TEnt : class
        {
            foreach (var predicate in predicates)
            {
                if (!predicate(ent).Equals(predicate(add)))
                    return false;
            }
            return true;
        }

    }

}
