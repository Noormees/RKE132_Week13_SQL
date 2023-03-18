

using System;
using System.Data.SQLite;

//ReadData(CreateConnection());
//InsertCustomer(CreateConnection());
//RemoveCustomer(CreateConnection());
FindCustomer(CreateConnection());



static SQLiteConnection CreateConnection()
{
    SQLiteConnection connection = new SQLiteConnection("Data Source=mydb.db; Version = 3; New = True; Compress = True;");

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


static void ReadData(SQLiteConnection myConnection)
{ 
    Console.Clear();
    SQLiteDataReader reader;    // selle abil loetaks andmed vahemallu ridade kaupa
    SQLiteCommand command;      // objekt, mis votab paringu (kask!)

    command = myConnection.CreateCommand();     //luuakse paringu
    command.CommandText = "SELECT rowid, * FROM customer";

    reader = command.ExecuteReader();       // pane/salvesta paring readerisse. ALUSTAME!

    while (reader.Read())       // laheb jargmisele reale
    {
        string readerRowID = reader["rowID"].ToString();
        string readerStringFirstName = reader.GetString(1);
        string readerStringLastName = reader.GetString(2);
        string readerStringDoB = reader.GetString(3);

        Console.WriteLine($"{readerRowID}. Full name: {readerStringFirstName} {readerStringLastName}; DoB: {readerStringDoB}");

    }

   myConnection.Close();
}


static void InsertCustomer(SQLiteConnection myConnection)
{
    SQLiteCommand command;
    string fName, lName, dob;

    Console.WriteLine("Enter first name:");
    fName = Console.ReadLine();
    Console.WriteLine("Enter last name:");
    lName = Console.ReadLine(); 
    Console.WriteLine("Enter date of birth (mm-dd-yyyy):");
    dob = Console.ReadLine();

    command = myConnection.CreateCommand();
    command.CommandText = $"INSERT INTO customer (firstName,lastName, dateOfBirth) " +
        $"VALUES ('{fName}', '{lName}', '{dob}');";

    int rowInserted = command.ExecuteNonQuery();
    Console.WriteLine($"Row inserted: {rowInserted}");

    ReadData(myConnection); 

}


static void RemoveCustomer(SQLiteConnection myConnection)
{
    SQLiteCommand command;

    string IDtoDelete;
    Console.WriteLine("Enter an ID to delete a ustomer");
    IDtoDelete = Console.ReadLine();

    command = myConnection.CreateCommand();
    command.CommandText = $"DELETE FROM customer WHERE rowid = {IDtoDelete}";
    int rowRemoved = command.ExecuteNonQuery();
    Console.WriteLine($"{rowRemoved} was removed from the table Customer");

    ReadData(myConnection);
}

static void FindCustomer(SQLiteConnection myConnection)

{
    SQLiteDataReader reader;
    SQLiteCommand command;
    string searchName;
    Console.WriteLine("Enter a first name to display customer data:");
        searchName = Console.ReadLine();
    command = myConnection.CreateCommand();
    command.CommandText = $"SELECT customer.rowid, customer.firstName, customer.lastName, status.statusType " +
                            $"FROM customerStatus " +
                             $"JOIN customer ON customer.rowid = customerStatus.customerId " +
                            $"JOIN status ON status.rowid = customerStatus.statusId " +
                             $"WHERE firstname LIKE '{searchName}'";
    reader = command.ExecuteReader();

    while (reader.Read())
    {
        string readerRowid = reader["rowid"].ToString();
        string readerStringName = reader.GetString(1);
        string readerStringLastName = reader.GetString(2);
        string readerStringStatus = reader.GetString(3);
        Console.WriteLine($"Search result: ID: {readerRowid}. {readerStringName} {readerStringLastName}. Status: {readerStringStatus}");

    }

    myConnection.Close();
}