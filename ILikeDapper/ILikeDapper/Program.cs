using ILikeDapper.Model.Implementation;
using System;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var provider = new Provider();
            provider.CreateTables();
            //var groups = await provider.GetAllGroups();
            //foreach(var group in groups)
            //{
            //    Console.WriteLine(group);
            //}

            var parent = new SimpleParameterGroup();
            var child = new SimpleParameterGroup() { Parent = parent};
            parent.Groups.Add(child);
            var subChild = new SimpleParameterGroup() { Parent = child };
            child.Groups.Add(subChild);

            var parameaterA = new SimpleParameter() { };

            await provider.InsertGroup(parent);

            Console.WriteLine("Done");
        }
    }
}