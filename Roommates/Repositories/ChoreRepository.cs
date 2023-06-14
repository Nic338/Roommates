using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Data.SqlClient;
using Roommates.Models;
using System.Collections.Generic;

namespace Roommates.Repositories
{
    /// <summary>
    /// This class is responsible for interacting with Chore data.
    /// It inherits from BaseRepository class so that is can use the BaseRepository's Connection property
    /// </summary>
    public class ChoreRepository : BaseRepository
    {
        public ChoreRepository(string connectionString) : base(connectionString) { }

    public List<Chore> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand()) 
                {
                    //setup the command with SQL we want to execute before we execute it
                    cmd.CommandText = "SELECT Id, Name FROM Chore";

                    //execute the SQL in teh database and get a "reader" that will give us access to the data.
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Chore> chores = new List<Chore>();

                    while (reader.Read()) 
                    {
                        ///Ordinal is the numeric position of the column in the query results
                        ///Id has an ordinal value of 0 and Name has a value of 1
                        int idColumnPosition = reader.GetOrdinal("Id");

                        ///Use the readers Get methods to get the value for a particular ordinal.
                        int idValue = reader.GetInt32(idColumnPosition);

                        int nameColumnPosition = reader.GetOrdinal("Name");
                        string nameValue = reader.GetString(nameColumnPosition);

                        Chore chore = new Chore
                        {
                            Id = idValue,
                            Name = nameValue,
                        };
                        ///Add the chore to the chore list
                        chores.Add(chore);
                    }
                    ///Close() the reader. Can't use a using block here.
                    reader.Close();
                    ///Return the list of chores to whoever called this method
                    return chores;
                }
            }
        }
        ///<summary>
        ///Returns a single chore with a given Id
        ///</summary>
        public Chore GetById(int id) 
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand()) 
                {
                    cmd.CommandText = "SELECT Name From Chore WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    Chore chore = null;

                    if (reader.Read())
                    {
                        chore = new Chore
                        {
                            Id = id,
                            Name = reader.GetString(reader.GetOrdinal("Name"))
                        };
                    }    
                    reader.Close();

                    return chore;
                }
            }
        }
        ///Method to add a new chore to the database
        ///This method sends data but returns nothing
        public void Insert(Chore chore)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Chore (Name)
                                            OUTPUT INSERTED.Id
                                            VALUES (@name)";
                    cmd.Parameters.AddWithValue("@name", chore.Name);
                    int id = (int)cmd.ExecuteScalar();

                    chore.Id = id;
                }
            }
        }
        public List<Chore> GetUnassignedChores()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    //setup the command with SQL we want to execute before we execute it
                    cmd.CommandText = "SELECT Chore.Id, Chore.Name FROM Chore LEFT JOIN RoommateChore on RoommateChore.ChoreId = Chore.Id WHERE RoommateChore.RoommateId is NULL";

                    //execute the SQL in teh database and get a "reader" that will give us access to the data.
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Chore> unassignedChores = new List<Chore>();

                    while (reader.Read())
                    {
                        ///Ordinal is the numeric position of the column in the query results
                        ///Id has an ordinal value of 0 and Name has a value of 1
                        int idColumnPosition = reader.GetOrdinal("Id");

                        ///Use the readers Get methods to get the value for a particular ordinal.
                        int idValue = reader.GetInt32(idColumnPosition);

                        int nameColumnPosition = reader.GetOrdinal("Name");
                        string nameValue = reader.GetString(nameColumnPosition);

                        Chore chore = new Chore
                        {
                            Id = idValue,
                            Name = nameValue,
                        };
                        ///Add the chore to the chore list
                        unassignedChores.Add(chore);
                    }
                    ///Close() the reader. Can't use a using block here.
                    reader.Close();
                    ///Return the list of chores to whoever called this method
                    return unassignedChores;
                }
            }
        }
    }
}
