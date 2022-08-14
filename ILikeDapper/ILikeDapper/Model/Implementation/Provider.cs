using Dapper;
using ILikeDapper.Model.Interface;
using Npgsql;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace ILikeDapper.Model.Implementation
{
    public class Provider
    {
        private readonly string _connectionString = "Host=localhost;Username=postgres;Password=123;Database=parameters";
        private readonly string _groupTableName = "groups";
        private readonly string _parameterTableName = "parameterTable";
        public void CreateTables()
        {
            using var con = new NpgsqlConnection(_connectionString);
            con.Open();

            using var cmd = new NpgsqlCommand();
            cmd.Connection = con;

            //group table
            cmd.CommandText = "DROP TABLE IF EXISTS groups";
            cmd.ExecuteNonQuery();

            cmd.CommandText = @"CREATE TABLE groups(id SERIAL PRIMARY KEY, id_parent integer ,
                                name VARCHAR(255))";
            cmd.ExecuteNonQuery();

            //parameter table
            cmd.CommandText = "DROP TABLE IF EXISTS parameter";
            cmd.ExecuteNonQuery();

            cmd.CommandText = @"CREATE TABLE groups(id SERIAL PRIMARY KEY, id_parent integer ,
                                name VARCHAR(255))";
            cmd.ExecuteNonQuery();

            //name attribute table
            cmd.CommandText = "DROP TABLE IF EXISTS nameAttribute";
            cmd.ExecuteNonQuery();

            cmd.CommandText = @"CREATE TABLE groups(id SERIAL PRIMARY KEY, id_parent integer ,
                                name VARCHAR(255))";
            cmd.ExecuteNonQuery();

            //cmd.CommandText = "INSERT INTO groups(name) VALUES('Aluminium') RETURNING id;";
            //var result = cmd.ExecuteNonQuery();

            //cmd.CommandText = "INSERT INTO groups(name) VALUES('10')";
            //cmd.ExecuteNonQuery();



            Console.WriteLine("Table groups created");
        }

        //public async void InsertGroup(SimpleParameterGroup group)
        //{
        //    using (IDbConnection connection = new NpgsqlConnection(_connectionString))
        //    {
        //        var p = new DynamicParameters();
        //        p.Add("@id", 0, DbType.Int32, ParameterDirection.Output);
        //        p.Add("@name", "holdrio");

        //        string sql = $@"insert into groups (name) 
        //                        values (@name) RETURNING id;";
        //        connection.Execute(sql,p);

        //        int newIdentity = p.Get<int>("@id");
        //        Console.WriteLine($"The new Id is {newIdentity}");
        //        //var newId = connection.QuerySingle<int>(sql, new { Id = (int?)null, Name = "newName" });
        //    }

        //}

        public async Task InsertValueAttribute<T>()
        {

        }

        public async Task InsertAttribute(IAttribute attribute)
        {
            
        }

        public async Task InsertParameter(IParameter parameter)
        {
            var p = new DynamicParameters();
            var parentId = parameter.Parent?.Id;
            string sql = "";

            int createdId = -1;
            if (parentId == null) // group is no subgroup
            {
                p.Add("@id", 0, DbType.Int32, ParameterDirection.Output);
                p.Add("@id_parent", null);
                sql = $@"insert into parameter (id_parent) values( @id_parent) RETURNING id;";

            }

            else
            {
                p.Add("@id", 0, DbType.Int32, ParameterDirection.Output);
                p.Add("@name", "holdrio");
                p.Add("@id_parent", parentId.Value);
                sql = $@"insert into parameter (name, id_parent) 
                                values (@name, @id_parent) RETURNING id;";


            }

            using (IDbConnection connection = new NpgsqlConnection(_connectionString))
            {

                await connection.ExecuteAsync(sql, p);

                createdId = p.Get<int>("@id");

            }


            parameter.Id = createdId;
            parameter.Attributes?.ForEach(async attribure =>
            {
                await InsertAttribute(attribure);
            });

        }
        public async Task InsertGroup(IGroup group)
        {
            var p = new DynamicParameters();
            var parentId = group.Parent?.Id;
            string sql = "";

            int createdId = -1;
            if (parentId == null) // group is no subgroup
            {
                p.Add("@id", 0, DbType.Int32, ParameterDirection.Output);
                p.Add("@id_parent", null);
                sql = $@"insert into groups (id_parent) values( @id_parent) RETURNING id;";

            }

            else
            {
                p.Add("@id", 0, DbType.Int32, ParameterDirection.Output);
                p.Add("@name", "holdrio");
                p.Add("@id_parent", parentId.Value);
                 sql = $@"insert into groups (name, id_parent) 
                                values (@name, @id_parent) RETURNING id;";

                
            }

            using (IDbConnection connection = new NpgsqlConnection(_connectionString))
            {

                await connection.ExecuteAsync(sql, p);

                createdId = p.Get<int>("@id");

            }


            group.Id = createdId;
            group.Groups?.ForEach(async subGroup => 
            {
                await InsertGroup(subGroup);
            });
        }


        public async Task<List<SimpleParameterGroup>> GetAllGroups()
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            string commandText = $"SELECT * FROM {_groupTableName}";
            var groups = await connection.QueryAsync<SimpleParameterGroup>(commandText);



            return groups.ToList();
        }

    }
}
