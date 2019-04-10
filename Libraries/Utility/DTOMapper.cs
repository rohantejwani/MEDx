using log4net.Core;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace Utility
{
    public class DTOMapper
    {
        public  string Mapper(Dictionary<String, string> SourceDTO, object DestinationDto, SBMapper map)
        {
            JObject dJsonString1 = null;
            try
            {
                dJsonString1 = JObject.FromObject(DestinationDto);
                foreach (var a in map.MapperCollection)
                {
                   // Logger.Log(Level.Debug, "a.DestinationProperty : " + a.DestinationProperty);
                    //Logger.Log(Level.Debug, "a.SourceProperty : " + a.SourceProperty);
                    //Logger.Log(Level.Debug, "SourceDTO[a.SourceProperty] : " + SourceDTO[a.SourceProperty]);
                    if (!string.IsNullOrEmpty(SourceDTO[a.SourceProperty]))
                        dJsonString1.SelectToken(a.DestinationProperty).Replace(SourceDTO[a.SourceProperty]);
                    else
                    {
                        //Logger.Log(Level.Debug, "Yes its not a string.");
                        dJsonString1.SelectToken(a.DestinationProperty).Replace("");
                    }
                }
               // Logger.Log(Level.Debug, dJsonString1.ToString());
            }
            catch (Exception ex)
            {
                var lineNumber = 0;
                const string lineSearch = ":line ";
                var index = ex.StackTrace.LastIndexOf(lineSearch);
                var lineNumberText = ex.StackTrace.Substring(index + lineSearch.Length);
                if (int.TryParse(lineNumberText, out lineNumber))
                {
                }
                throw new Exception("in mapper :: "+ex.ToString() + " \n line number " + lineNumber + " \n line number text " + lineNumberText);

               // Logger.Log(Level.Error, "Error in Mapping Entity to DTO ");
                //throw ex;
            }
            return dJsonString1.ToString();
        }

        public  DataTable ConvertListToDataTable<T>(IList<T> data, DataTable table, SBMapper map)
        {
            JObject dJsonString1 = null;
            foreach (T item in data)
            {
                dJsonString1 = JObject.FromObject(item);
                DataRow row = table.NewRow();
                foreach (var a in map.MapperCollection)
                {
                    row[a.SourceProperty] = dJsonString1.GetValue(a.DestinationProperty);
                }
                table.Rows.Add(row);
            }
            return table;
        }



    }
}
