using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using System.Configuration;
using Oracle.ManagedDataAccess;
using Oracle.ManagedDataAccess.Types;
using Oracle.ManagedDataAccess.Client;

using MvcMovie.Models;
using OracleHeaper;

namespace MvcMovie.Data
{
    public abstract class OracleManager
    {
         public OracleManager()
        { 
        }

        public static List<Movie> toList()
        {
            List<Movie> movies = new List<Movie>();
            string sqlString = "select * from Movies";
            var res=DbHelperOra.Query(sqlString).Tables[0];
            foreach(DataRow row in res.Rows)
            {
                Movie m=new Movie(Convert.ToInt32(row["Id"]),(string)row["Title"],(DateTime)row["ReleaseDate"],(string)row["Genre"],(decimal)row["Price"]);
                movies.Add(m);
            }

            return movies;
        }

        public static Movie FirstOrDefault(int? id)
        {
            var sql = "select * from Movies where Id=:id";
            List<OracleParameter> parameter = new List<OracleParameter>();
            parameter.Add(new OracleParameter(":id",id));
            var res = DbHelperOra.Query(sql, parameter.ToArray()).Tables[0];
            if(res.Rows.Count==0)
            {
                return null;
            }
            Movie m = new Movie(Convert.ToInt32(res.Rows[0]["Id"]), (string)res.Rows[0]["Title"], (DateTime)res.Rows[0]["ReleaseDate"], (string)res.Rows[0]["Genre"], (decimal)res.Rows[0]["Price"]);
            return m;
        }

        public static bool Exists(int? id)
        {
            if(id==null)
            {
                return false;
            }
            var sql = "select Id from Movies where Id=:id";
            List<OracleParameter> parameter = new List<OracleParameter>();
            parameter.Add(new OracleParameter(":id", id));
            var res = DbHelperOra.Query(sql, parameter.ToArray()).Tables[0];
            if (res.Rows.Count == 0)
            {
                return false;
            }
            //else
            return true;
        }

        public static void Add(Movie movie)
        {
            string sql = "insert into Movies values (:id,:title,:releaseDate,:genre,:price)";
            List<OracleParameter> parameter = new List<OracleParameter>();
            Console.WriteLine(movie.Id + " " + movie.Title + " " + movie.ReleaseDate + " " + movie.Genre + " " + movie.Price);
            parameter.Add(new OracleParameter(":id", movie.Id));
            parameter.Add(new OracleParameter(":title", movie.Title));
            parameter.Add(new OracleParameter(":releaseDate", movie.ReleaseDate));
            parameter.Add(new OracleParameter(":genre", movie.Genre));
            parameter.Add(new OracleParameter(":price", movie.Price));
            DbHelperOra.ExecuteSql(sql, parameter.ToArray());
        }

        public static void SaveChanges()
        {
            string sql = "commit";
            var res = DbHelperOra.ExecuteSql(sql);
        }

        public static void Updage(Movie movie)
        {
            if (Exists(movie.Id)==false)
                return;
            string sql = "update Movies set Title=:title,ReleaseDate=:releaseDate,Genre=:genre,Price=:price where Id=:id";
            List<OracleParameter> parameter = new List<OracleParameter>();
            parameter.Add(new OracleParameter(":title", movie.Title));
            parameter.Add(new OracleParameter(":releaseDate", movie.ReleaseDate));
            parameter.Add(new OracleParameter(":genre", movie.Genre));
            parameter.Add(new OracleParameter(":price", movie.Price));
            parameter.Add(new OracleParameter(":id", movie.Id));
            DbHelperOra.ExecuteSql(sql, parameter.ToArray());
        }
        
        public static void Remove(int id)
        {
            if (Exists(id) == false)
                return;
            string sql = "delete from Movies where Id=:id";
            List<OracleParameter> parameter = new List<OracleParameter>();
            parameter.Add(new OracleParameter(":id", id));
            DbHelperOra.ExecuteSql(sql, parameter.ToArray());

        }
    }
}