using System.Diagnostics;

using System;
using System.Data.SQLite;

DisplayProduct(CreateConnection());

static SQLiteConnection CreateConnection()
{
    SQLiteConnection connection = new SQLiteConnection("Data Source=bar.db; Version = 3; New = True; Compress = True;");

    // proovime luua connection
    try
    {
        connection.Open();

        // Console.WriteLine("DB found.");
    }
    catch
    {

        Console.WriteLine("DB not found.");
    }

    return connection;

}

static void DisplayProduct(SQLiteConnection myConnection)
{

    SQLiteDataReader reader;

    SQLiteCommand command;

    command = myConnection.CreateCommand();

    command.CommandText = "SELECT rowid, ProductName, Price FROM Product";
    reader = command.ExecuteReader();


    while (reader.Read())
    {

        string readerRowid = reader["rowid"].ToString();
        string readerProductName = reader.GetString(1);
        int readerProductPrice = reader.GetInt32(2); //hinna tüüp andmebaasis on int, nii et siin loeme andmebaasis ka int-tüüpi andmeid

        Console.WriteLine($"{readerRowid}. {readerProductName}. Price: {readerProductPrice}");
    }

    myConnection.Close();
}
