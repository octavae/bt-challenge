# BT challenge
The goal is to send the data stored in the table "Tasks" of a relational database to a web api for processing\
The web api has a single controller, jobs, and four actions:notificare, fisier, plata, incasare. The attribute "TaskType" of the entity "Tasks" has also 4 possible value, matching exactly the 4 actions of the web api.  
## Web api configuration
* Create a web site in IIS using the port 8085 and the full path to "JobsWebAPI" 
## How to run the console application
* Check the <appSettings> node in App.config then start debugging in Visual Studio 