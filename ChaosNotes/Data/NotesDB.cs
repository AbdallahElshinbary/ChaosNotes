using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using ChaosNotes.ViewModels;

namespace ChaosNotes.Data
{
    public static class NotesDB
    {
        //######################################### Helpers ##########################################//

        public static string groupsTableName = "Groups";
        public static string groupsColumnId = "Id";
        public static string groupsColumnTitle = "Title";

        public static string notesTableName = "Notes";
        public static string notesColumnId = "Id";
        public static string notesColumnContent = "Content";
        public static string notesColumnColor = "Color";
        public static string notesColumnGroupId = "GroupId";

        public static string eventsTableName = "Events";
        public static string eventsColumnId = "Id";
        public static string eventsColumnContent = "Content";
        public static string eventsColumnColor = "Color";
        public static string eventsColumnDate = "Date";

        public static int currentGroupId;  // dirty trick :)

        //######################################### Connect ##########################################//

        public static SqlConnection db_connect()
        {
            string conn_string = Properties.Settings.Default.connection_string;
            SqlConnection conn = new SqlConnection(conn_string);
            if (conn.State != ConnectionState.Open) conn.Open();
            return conn;
        }

        //######################################### Helpers ##########################################//

        public static DataTable db_getTable(string query)
        {
            SqlConnection conn = db_connect();
            DataTable table = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
            adapter.Fill(table);
            return table;
        }

        public static void db_execute(string query)
        {
            SqlConnection conn = db_connect();
            SqlCommand command = new SqlCommand(query, conn);
            command.ExecuteNonQuery();
        }

        //######################################### Insert ###########################################//
       
        public static void db_insertGroup(GroupModel group)
        {
            string query = $"INSERT INTO {groupsTableName} ({groupsColumnTitle}) " +
                           $"VALUES('{group.Title}')";
            db_execute(query);
        }

        public static void db_insertNote(NoteModel note)
        {
            string query = $"INSERT INTO {notesTableName} ({notesColumnContent}, {notesColumnColor}, {notesColumnGroupId}) " +
                           $"VALUES('{note.Content}', '{note.Color}', '{note.GroupID}')";
            db_execute(query);
        }

        public static void db_insertEvent(EventModel eve)
        {
            string query = $"INSERT INTO {eventsTableName} ({eventsColumnContent}, {eventsColumnColor}, {eventsColumnDate}) " +
                           $"VALUES('{eve.Content}', '{eve.Color}', '{eve.Date}')";
            db_execute(query);
        }

        //######################################### Query ############################################//

        public static List<GroupModel> db_queryGroups()
        {
            string query = $"SELECT * FROM {groupsTableName}";
            DataTable table = db_getTable(query);

            List<GroupModel> groups = new List<GroupModel>();
            foreach(DataRow row in table.Rows)
            {
                groups.Add(new GroupModel { Id = int.Parse(row[$"{groupsColumnId}"].ToString()),
                                            Title = row[$"{groupsColumnTitle}"].ToString() });
            }
            return groups;
        }

        public static GroupModel db_queryGroup(int groupId)
        {
            string query = $"SELECT * FROM {groupsTableName} WHERE {groupsColumnId} = {groupId}";
            DataTable table = db_getTable(query);
            return new GroupModel { Id = int.Parse(table.Rows[0][$"{groupsColumnId}"].ToString()),
                                    Title = table.Rows[0][$"{groupsColumnTitle}"].ToString() };
        }

        public static List<NoteModel> db_queryNotes(int groupId)
        {
            string query = $"SELECT * FROM {notesTableName} WHERE {notesColumnGroupId} = {groupId}";
            DataTable table = db_getTable(query);

            List<NoteModel> notes = new List<NoteModel>();
            foreach (DataRow row in table.Rows)
            {
                notes.Add(new NoteModel { Id = int.Parse(row[$"{notesColumnId}"].ToString()),
                                          Content = row[$"{notesColumnContent}"].ToString(),
                                          Color = row[$"{notesColumnColor}"].ToString(),
                                          GroupID = int.Parse(row[$"{notesColumnGroupId}"].ToString()) });
            }
            return notes;
        }

        public static List<EventModel> db_queryEvents()
        {
            string query = $"SELECT * FROM {eventsTableName}";
            DataTable table = db_getTable(query);

            List<EventModel> events = new List<EventModel>();
            foreach(DataRow row in table.Rows)
            {
                events.Add(new EventModel{ Id = int.Parse(row[$"{eventsColumnId}"].ToString()),
                                           Content = row[$"{eventsColumnContent}"].ToString(),
                                           Color = row[$"{eventsColumnColor}"].ToString(),
                                           Date = row[$"{eventsColumnDate}"].ToString() });
            }
            return events;
        }
        
        //######################################### Update ###########################################//
    
        public static void db_updateGroup(GroupModel group)
        {
            string query = $"UPDATE {groupsTableName} SET {groupsColumnTitle} = '{group.Title}' " +
                           $"WHERE {groupsColumnId} = {group.Id}";
            db_execute(query);
        }

        public static void db_updateNote(NoteModel note)
        {
            string query = $"UPDATE {notesTableName} SET {notesColumnContent} = '{note.Content}' " +
                    $"WHERE {notesColumnId} = {note.Id}";
            db_execute(query);
        }

        public static void db_updateEventContent(EventModel eve)
        {
            string query = $"UPDATE {eventsTableName} SET {eventsColumnContent} = '{eve.Content}' " +
                           $"WHERE {eventsColumnId} = {eve.Id}";
            db_execute(query);
        }        
        
        public static void db_updateEventDate(EventModel eve)
        {
            string query = $"UPDATE {eventsTableName} SET {eventsColumnDate} = '{eve.Date}' " +
                           $"WHERE {eventsColumnId} = {eve.Id}";
            db_execute(query);
        }

        //######################################### Delete ###########################################//

        public static void db_deleteGroup(int id)
        {
            string query = $"DELETE FROM {notesTableName} WHERE {notesColumnGroupId} = {id}";
            db_execute(query);
            
            query = $"DELETE FROM {groupsTableName} WHERE {groupsColumnId} = {id}";
            db_execute(query);
        }

        public static void db_deleteAllGroup()
        {
            string query = $"DELETE FROM {notesTableName}";
            db_execute(query);

            query = $"DELETE FROM {groupsTableName}";
            db_execute(query);
        }

        public static void db_deleteNote(int id)
        {
            string query = $"DELETE FROM {notesTableName} WHERE {notesColumnId} = {id}";
            db_execute(query);
        }

        public static void db_deleteEvent(int id)
        {
            string query = $"DELETE FROM {eventsTableName} WHERE {eventsColumnId} = {id}";
            db_execute(query);
        }

        public static void db_deleteAllEvent()
        {
            string query = $"DELETE FROM {eventsTableName}";
            db_execute(query);
        }

        //######################################### Close ############################################//

        public static void db_close()
        {
            string conn_string = Properties.Settings.Default.connection_string;
            SqlConnection conn = new SqlConnection(conn_string);
            if (conn.State != ConnectionState.Closed) conn.Close();
        }
    }
}
