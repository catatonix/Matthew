using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Matthew.Models
{
    /// <summary>
    /// Class that stores helpful details such as last spoken about location, helps bot make assumptions as to what the user is reffering to
    /// 
    /// Effectively super-conte
    /// 
    /// todo room for state enums such as conversation tone and pace
    /// todo add functionality to just check entities and intents from last message / conversation block only
    /// todo store memories for different conversation blocks and use appropriately
    /// </summary>
    public static class ShortTermMem
    {

        private static readonly int MEMORY_POWER = 10;      // readonly to prevent skynet, increase at your own peril

        public static List<Tuple<string, string>> lastEntities = new List<Tuple<string, string>>(MEMORY_POWER);   // most recent conversation topics
        public static Location searchLocation = new Location("London");


        /// <summary>
        /// Returns the value for the last entity if it exists within the ShortTermMem
        /// Returns "" if not found (empty string)
        /// </summary>
        /// <param name="entity">Entity to be searched for (i.e. Places.AbsoluteLocation</param>
        /// <returns></returns>
        public static string getLastEntityValue(string entity)
        {
            try
            {
                return lastEntities.Where(e => e.Item1 == entity).First().Item2;
            }
            catch
            {
                return ""; 
            }
        }


        /// <summary>
        /// Adds entities from the last message to the short term memory
        /// WARNING: start of long sentences may be forgotten if ShortTermMem.MEMORY_POWER set too low
        /// </summary>
        /// <param name="result">LUIS result object</param>
        public static void updateMemory(LuisResult result)
        {
            foreach(EntityRecommendation entity in result.Entities)
            {
                _addEntity(entity.Type, entity.Entity);
            }
        }

        // todo finish this method, it displays the contents of the short term memory as a string
        //public string whatsOnMyMind()
        //{
        //    string thought = "";
        //    for (int i=0; i<lastEntities.Count(); i++)
        //    {
        //        string.Join(",", myList.ToArray())

        //    }
        //}

        /// <summary>
        /// Method to add entities to the short term memory
        /// </summary>
        /// <param name="entity">Entity Type</param>
        /// <param name="value">Entity String</param>
        private static void _addEntity(string entity, string value)
        {
            if (lastEntities.Count == MEMORY_POWER)
            {
                lastEntities.RemoveAt(MEMORY_POWER-1);   // remove oldest memory
            }
            lastEntities.Insert(0, new Tuple<string, string>(entity, value));
        }


    }
}