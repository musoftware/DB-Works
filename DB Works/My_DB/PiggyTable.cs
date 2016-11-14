using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DB_Works.My_DB
{
    class PiggyTable
    {
        static DBWorks dbc = new My_DB.DBWorks(dbPath());

        public static string dbPath()
        {
            string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            string programPath = Path.Combine(docPath, Application.ProductName);

            if (!Directory.Exists(programPath))
                Directory.CreateDirectory(programPath);

            string dbPath = Path.Combine(programPath, "piggy.db");

            return dbPath;
        }

        public static DataTable LoadTable()
        {
            DataTable dt = null;
            dbc.StartWork((sh) =>
            {
                try
                {
                    dt = sh.Select("select * from piggyTable;");
                }
                catch
                {

                }
            });
            return dt;
        }


        public static void insert(string pname, string pmoney)
        {
            decimal dmoney = 0.0M;
            if (!decimal.TryParse(pmoney, out dmoney))
                throw new Exception("Money must be numrical value");

            if (string.IsNullOrEmpty(pname))
                throw new Exception("name must be non empty value");

            dbc.StartWork((sh) =>
            {
                sh.BeginTransaction();
                try
                {

                    var dic = new Dictionary<string, object>();
                    dic["person_name"] = pname;
                    dic["person_money"] = dmoney;
                    sh.Insert("piggyTable", dic);

                    sh.Commit();
                }
                catch
                {
                    sh.Rollback();
                }
            });

        }

        internal static void update(int id, string pname, string pmoney)
        {
            decimal dmoney = 0.0M;
            if (!decimal.TryParse(pmoney, out dmoney))
                throw new Exception("Money must be numrical value");

            if (string.IsNullOrEmpty(pname))
                throw new Exception("name must be non empty value");

            dbc.StartWork((sh) =>
            {
                sh.BeginTransaction();
                try
                {
                    var dic = new Dictionary<string, object>();
                    dic["person_name"] = pname;
                    dic["person_money"] = dmoney;
                    sh.Update("piggyTable", dic, "id", id);
                    sh.Commit();
                }
                catch
                {
                    sh.Rollback();
                }
            });
        }

        internal static void delete(int id)
        {

            dbc.StartWork((sh) =>
            {
                sh.BeginTransaction();
                try
                {
                    sh.Execute("delete from piggyTable where id = '" + sh.Escape(id.ToString()) + "'");
                    sh.Commit();
                }
                catch
                {
                    sh.Rollback();
                }
            });
        }

        public static void Initialise()
        {
            if (!File.Exists(dbPath()))
            {
                SQLiteConnection.CreateFile(dbPath());

                dbc.StartWork((sh) =>
                {
                    SQLiteTable tb = new SQLiteTable("piggyTable");
                    tb.Columns.Add(new SQLiteColumn("id", true));
                    tb.Columns.Add(new SQLiteColumn("person_name"));
                    tb.Columns.Add(new SQLiteColumn("person_money", ColType.Decimal));
                    sh.CreateTable(tb);
                });
            }
        }




    }
}
