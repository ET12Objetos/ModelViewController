using LiteDB;
using Microsoft.Extensions.Options;
using System;

namespace ejemplomvc.Models
{
    public class LiteDbContext
    {
        public readonly LiteDatabase Context;
        public LiteDbContext(IOptions<LiteDbConfig> configs)
        {
            try
            {
                var db = new LiteDatabase(configs.Value.DatabasePath);
                if (db != null)
                    Context = db;
            }
            catch (Exception ex)
            {
                throw new Exception("No se encuentra o no se puede crear LiteDB.", ex);
            }
        }           
    }    
}