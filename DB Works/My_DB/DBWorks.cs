using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace DB_Works.My_DB
{
    public class DBWorks
    {
        private string filename;

        public DBWorks(string filename)
        {
            this.filename = filename;
        }

        public void StartWork(Action<SQLiteHelper> action)
        {
            using (SQLiteConnection conn = new SQLiteConnection("data source=" + this.filename + ""))
            {
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    cmd.Connection = conn;
                    conn.Open();

                    SQLiteHelper sh = new SQLiteHelper(cmd);

                    action.Invoke(sh);

                    conn.Close();
                }
            }
        }

    }
}
