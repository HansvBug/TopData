TopData
This will be a program with which you can run queries on an Oracle database schema. You can create and manage the queries using the program. It will be possible to execute 1 query, but also several in succession. The result is filterable and exportable to CSV and Excel format. Because you will initially be working with user groups, you can indicate per group which queries someone is allowed to perform. It also depends on the group whether you can create, delete or only view queries.
The program is still under construction but part of it is already working. Below are the approximate steps that will be taken.

Ready:
• Testing a connection to Oracle and saving the connection data in the application database is ready. (02-11-2021)
• Create and save user data. (02-11-2021)
• Login with user name. (02-11-2021)
• Create, modify, delete queries and store them in the application database. (02-11-2021)
• Run queries. (02-11-2021)
• Filter result. (02-11-2021)
• Export query result. (02-11-2021)

To do:
• Bug fixes.
• Support Dutch and Englisch language.
• Execute several queries which belong to a querygroup.
• Encrypt queries. Store the key in a key container.
• Show the Oracle geometry field in the datagrid.
• instruction manual. 
• ...

After compiling, start the program for the first time in the command line: TopData.exe Install. This creates the SQLite database in which things are stored. The program opens with pens with a login screen. a login screen. For the first time, the username is System and the password is Welcome. Then change this System password. Remark, the encryption will change so everything you store now will be lost when the first version will be released.

The program has 2 languages. English (now only 1%) and Dutch (now 99%). Go to menu, Options, Language and choose Dutch or Englisch. If you want to set up an Oracle connection go to menu, Maintain, Connections. On the New tab you can enter and test the connection details. Only if the testing goes well you can save the connection data. If you want to add a Query goto to menu, Maintain, Queries.
If the details of a connection are saved, the connection name will be visible in menu Program, Connections.
