using Autofac;
using MicroOrms.User.Autofac;
using System;
using System.Collections.Generic;

namespace MicroOrms.User
{
    class Program
    {
        private static IContainer Container { get; set; }

        public static ITodoDatabase TodoDatabase => Container.Resolve<ITodoDatabase>();

        static Program()
        {
            var containerBuiler = new ContainerBuilder();
            containerBuiler.RegisterModule(new TodoDatabaseModule());
            Container = containerBuiler.Build();
        }
        
        static void Main(string[] args)
        {
            PrintAllTodoItems(TodoDatabase.ReadAll());
            var createdId = TodoDatabase.Create(new TodoItem { Name = "Item1", IsComplete = false, UserId = 1 });
            var createdItem = TodoDatabase.Read(createdId);
            PrintTodoItem(createdItem);
            createdItem.IsComplete = true;
            PrintTodoItem(createdItem);
            TodoDatabase.Update(createdItem);
            PrintAllTodoItems(TodoDatabase.ReadAll());
            var createdId2 = TodoDatabase.Create(new TodoItem { Name = "Item2", IsComplete = true, UserId = 1 });
            PrintAllTodoItems(TodoDatabase.ReadAll());
            TodoDatabase.Delete(createdId2);
            PrintAllTodoItems(TodoDatabase.ReadAll());
            TodoDatabase.Delete(createdId);
            PrintAllTodoItems(TodoDatabase.ReadAll());
            Console.ReadKey();
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
    }
}
