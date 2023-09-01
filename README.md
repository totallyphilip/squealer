# Squealer 2022
A Windows console application to simplify creation and management of stored procedures, views, and functions.

This project is written in VB.NET. I would like to rewrite it as C# but I'm too lazy.

I wrote the first version of this project around 1999. I was a SQL DBA and had created templates for developers on my team to write their stored procedures. The templates took care of things like handling deadlocks, rolling back transactions on errors, etc., with a "PUT YOUR T-SQL CODE HERE" section in the middle of each template. The problem was that every time I upgraded the templates, I'd have hundreds of existing stored procedures to fix. Squealer was the solution to that problem: rather than having developers type code into the middle of a template, they simply type code into a file that has essentially nothing but code. Then the Squealer program reads those code files, wraps the latest templates around them, and spits out the result for the developers to paste into SQL Server.

Once the main goal was accomplished, I added additional features like code generators, basic Git integration, conversion between object types (procs, functions, views), and the list keeps growing. A secondary but equally important goal was to make sure SQL developers could quit using this tool at any time without their existing procs being captive to it. To accomplish this, simply save the generated output to disk. This tool does not do anything magical! At its core, it's just a gigantic text replacement program.

I hope you get some use out of this. If you would like to contribute to its ongoing development, please donate to https://paypal.me/totallyphilip. I have spent decades providing support to developers, and at this point I'm not ashamed to ask for some help in return.

This project includes https://github.com/TaoK/PoorMansTSqlFormatter, a very awesome package that deserves your support. UPDATE: I removed this function. I don't remember why, but I may re-add it.

Thanks, and happy coding!

-TotallyPhilip
