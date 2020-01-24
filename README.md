# WPF Demos

This is a WPF demo in C# and in VB.Net, this demo use an SQL Server DB</br>
To use this demo you need:
1. To extract the 7zip file in the DB folder.
2. Create a new database by attaching these file in the SQL Server under the SQL 'sa' user.
3. Run the initDB.sql file inside the DB folder using Microsoft SQL Server Management Studio.</br>
   it will create the tables and populate some data into the database.
4. You need to modify the App.Config inside the C#/MyZooManager and VB.Net/VBZooManager.</br>
    You need to write your SQL Server sa password in the ConnectionString section.</br>
</br>
##### The Zoo Manager Application</br>
The Application help you to manage the animals that exists inside multiple Zoos.</br>
You can Add, Update or Delete Zoos.</br>
You can Add or Remove Animals to or from the selected Zoo.</br>
You can Add, Update or Delete an Animals from the animals list.
