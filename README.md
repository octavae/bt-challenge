# BT challenge
The goal is to send the data stored in the table "Tasks" of a relational database to a web api for processing<br>
The web api has a single controller, jobs, and four actions: notificare, fisier, plata, incasare. The attribute "TaskType" of the entity "Tasks" has also 4 possible values, matching exactly the 4 actions of the web api. 
## Short analysis of implementation options
One requirement was to use a relational database, not a specific database. As such, the implementation must be able to connect to more than one database server.<br>
The second requirement, not explicitly stated, but implied, was to use either .NET framework or .NET core.
To satisfy the requirements, the following options were evaluated:<br>
* a database procedure that reads all the tasks and calls the web service; this option can be currently implemented in MSSQL server and Oracle SQL server, but it excludes other existing relational databases that might perform better in the given scenario
* a new api that reads all the tasks and calls the web service; this option is the most flexible, but it is more complex because it has to implement code to connect to the remote web service and code to expose api methods that are consumed by other applications that use the api
* a Windows service (or Linux daemon) that reads all the tasks and calls the web service; this option is tied to Windows (or Linux) => not flexible enough
* a console application that reads all the tasks and calls the web service; this option is flexible enough and simple to implement and it can run on MacOS or Linux if .NET core is used to implement it<br>

After analyzing all the above options, the simplest and flexible enough solution was a console application implemented using C# on .NET Framework. 
## Web api configuration
* Create a web site in IIS using the port 8085 and the full path to "JobsWebAPI" 
## How to run the console application
* Check the <appSettings> node in App.config then start debugging in Visual Studio 