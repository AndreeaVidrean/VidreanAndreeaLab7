using System;
using VidreanAndreeaLab7.Data;
using System.IO;

namespace VidreanAndreeaLab7
{
    public partial class App : Application
    {
        // Define the database field within the App class
         static ShoppingListDatabase database;

        // Property to access the database
        public static ShoppingListDatabase Database
        {
            get
            {
                if (database == null)
                {
                    // Initialize the database with the correct file path
                    database = new
                    ShoppingListDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.
                    LocalApplicationData), "ShoppingList.db3"));
                }
                return database;
            }
        }

        // Constructor for the App class
        public App()
        {
            InitializeComponent();
            MainPage = new AppShell(); // Assign the MainPage
        }
    }
}
