using Autofac;
using MicroOrms.Entities;
using MicroOrms.User.Autofac;
using System;
using System.Collections.Generic;

namespace MicroOrms.User
{
    class Program
    {
        private static IContainer Container { get; set; }

        private static OrmType OrmType { get; set; }

        public static ITodoDatabase TodoDatabase => Container.Resolve<ITodoDatabase>();

        static void Main(string[] args)
        {
            HandleUserInput();
            ConfigureOrm();

            PrintAllTodoItems(TodoDatabase.TodoItems.ReadAll());
            var createdId = TodoDatabase.TodoItems.Create(new TodoItem { Name = "Item1", IsComplete = false, UserId = 1 });
            var createdItem = TodoDatabase.TodoItems.Read(createdId);
            PrintTodoItem(createdItem);
            createdItem.IsComplete = true;
            PrintTodoItem(createdItem);
            TodoDatabase.TodoItems.Update(createdItem);
            PrintAllTodoItems(TodoDatabase.TodoItems.ReadAll());
            var createdId2 = TodoDatabase.TodoItems.Create(new TodoItem { Name = "Item2", IsComplete = true, UserId = 1 });
            PrintAllTodoItems(TodoDatabase.TodoItems.ReadAll());
            PrintAllUsers(TodoDatabase.Users.ReadAll());

            var createdUserId = TodoDatabase.Users.Create(new Entities.User { Name = "user_02" });
            var createdUser = TodoDatabase.Users.Read(createdUserId);
            PrintUser(createdUser);
            TodoDatabase.TodoItems.Create(new TodoItem { Name = "Item3", IsComplete = false, UserId = createdUserId });
            TodoDatabase.TodoItems.Create(new TodoItem { Name = "Item4", IsComplete = true, UserId = createdUserId });
            PrintUser(createdUser);
            createdUser.Name = "user_03";
            TodoDatabase.Users.Update(createdUser);
            PrintAllUsers(TodoDatabase.Users.ReadAll());
            PrintAllTodoItems(TodoDatabase.TodoItems.ReadAll());
            TodoDatabase.Users.Delete(createdUserId);
            PrintAllUsers(TodoDatabase.Users.ReadAll());
            PrintAllTodoItems(TodoDatabase.TodoItems.ReadAll());

            TodoDatabase.TodoItems.Delete(createdId2);
            PrintAllTodoItems(TodoDatabase.TodoItems.ReadAll());
            TodoDatabase.TodoItems.Delete(createdId);
            PrintAllTodoItems(TodoDatabase.TodoItems.ReadAll());
            PrintAllUsers(TodoDatabase.Users.ReadAll());
            Console.ReadKey();
        }

        private static void HandleUserInput()
        {
            Console.WriteLine("Select the ORM to use:");
            foreach (var ormType in Enum.GetValues(typeof(OrmType)))
            {
                Console.WriteLine($"{(int)ormType} - {ormType}");
            }

            try
            {
                var userInput = Console.ReadLine();
                var userInputAsInt = int.Parse(userInput);
                if (Enum.IsDefined(typeof(OrmType), userInputAsInt))
                {
                    OrmType = (OrmType)Enum.ToObject(typeof(OrmType), userInputAsInt);
                }
                OrmType = OrmType.Dapper;
            }
            catch
            {
                OrmType = OrmType.Dapper;
            }
        }

        private static void ConfigureOrm()
        {
            var containerBuiler = new ContainerBuilder();
            var todoDataBaseModule = new TodoDatabaseModule
            {
                OrmType = OrmType
            };
            containerBuiler.RegisterModule(todoDataBaseModule);
            Container = containerBuiler.Build();
        }

        private static void PrintTodoItem(TodoItem todoItem)
        {
            Console.WriteLine($"{todoItem.Id}, {todoItem.Name}, {todoItem.IsComplete}, {todoItem.UserId}");
        }

        private static void PrintAllTodoItems(IEnumerable<TodoItem> todoItems)
        {
            foreach (var todoItem in todoItems)
            {
                PrintTodoItem(todoItem);
            }
        }

        private static void PrintUser(Entities.User user)
        {
            Console.WriteLine($"{user.Id}, {user.Name}");

            if (user.TodoItems.Count == 0)
            {
                Console.WriteLine("   <No TodoItem>");
                return;
            }

            foreach (var todoItem in user.TodoItems)
            {
                Console.Write("   ");
                PrintTodoItem(todoItem);
            }
        }

        private static void PrintAllUsers(IEnumerable<Entities.User> users)
        {
            foreach (var user in users)
            {
                PrintUser(user);
            }
        }
    }
}
