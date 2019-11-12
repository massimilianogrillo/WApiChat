using System;
using System.Collections.Generic;
using System.Linq;

namespace Extensions
{

    public static class IEnumerableExtensions
    {
        /// <summary>
        /// Filter elements that not exist using the predicates in the second argument
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="db"></param>
        /// <param name="entites"></param>
        /// <param name="predicates"></param>
        /// <returns></returns>
        public static IEnumerable<T> ElementsNotExist<T, TKey>(this IEnumerable<T> db, IEnumerable<T> entites, IEnumerable<Func<T, TKey>> predicates) where T : class, new()
        {
            if (predicates == null)
                return entites;
            var list = new List<T>();
            foreach (var entity in entites)
            {
                if (!db.Exists(predicates, entity))
                    list.Add(entity);
            }
            return list;

        }

        /// <summary>
        /// Finds if an entity exists 
        /// </summary>
        /// <typeparam name="TEnt"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="entities"></param>
        /// <param name="predicates"></param>
        /// <param name="add"></param>
        /// <returns></returns>
        public static bool Exists<TEnt, TKey>(this IEnumerable<TEnt> entities, IEnumerable<Func<TEnt, TKey>> predicates, TEnt add) where TEnt : class
        {
            var exists = entities.Any(ent => Check(predicates, ent, add));
            return exists;
        }

        /// <summary>
        /// Check if 2 entities are equal as depicted by predicates
        /// </summary>
        /// <typeparam name="TEnt"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="predicates"></param>
        /// <param name="ent"></param>
        /// <param name="add"></param>
        /// <returns></returns>
        public static bool Check<TEnt, TKey>(IEnumerable<Func<TEnt, TKey>> predicates, TEnt ent, TEnt add) where TEnt : class
        {
            foreach (var predicate in predicates)
            {
                if (!predicate(ent).Equals(predicate(add)))
                    return false;
            }
            return true;
        }


    }

    public static class StringExtensions
    {
        /// <summary>
        /// Finds if a string contains any of the elements in the list arguments
        /// </summary>
        /// <param name="str"></param>
        /// <param name="elements"></param>
        /// <returns></returns>
        public static bool ContainsAny(this string str, List<string> elements)
        {
            foreach (string element in elements)
            {
                if (str.Contains(element))
                    return true;
            }
            return false;
        }

    }



}
