# WeText
WeText is a sample application that demonstrates the implementation of DDD/CQRS and microservice architectural patterns in C#.

| Windows Server 2012 | Ubuntu 14.04.4 LTS | CentOS 7 |
|---------------------|--------------------|----------|
|[![Build Status](http://daxnet.me:8080/buildStatus/icon?job=we-text-win)](http://daxnet.me:8080/job/we-text-win/)|[![Build Status](http://daxnet.me:8080/buildStatus/icon?job=we-text-ubuntu)](http://daxnet.me:8080/job/we-text-ubuntu/)|[![Build Status](http://daxnet.me:8080/buildStatus/icon?job=we-text-centos)](http://daxnet.me:8080/job/we-text-centos/)|


# Architecture
![](https://raw.githubusercontent.com/wiki/daxnet/we-text/img/Architecture.png)

# Prerequisites
- Visual Studio 2015 with latest update (For development and debugging purpose)
	- If you want to setup the CI server, you don't need to install the whole Visual Studio pack on your CI server, which is neither necessary nor cost-efficient. Please install the following packs instead:
		- MS Build Tool: [https://www.microsoft.com/en-sg/download/details.aspx?id=48159](https://www.microsoft.com/en-sg/download/details.aspx?id=48159)
		- .NET 4.6.1 Developer Pack: [https://www.microsoft.com/en-us/download/details.aspx?id=49978](https://www.microsoft.com/en-us/download/details.aspx?id=49978)
- Rabbit MQ (For lightweight message queue)
- Mongo DB (For CQRS Event Store)
- MySQL (In case you wish to use MySQL as Table Data Gateway)

# Environment Setup
Follow the instructions below to setup your environment so that you can debug and run the WeText application.

- Download the source code from GitHub

		git clone https://github.com/daxnet/we-text
-  Download and install Rabbit MQ with default preferences (Default port numbers, default server configuration)
	-  For more information about Rabbit MQ installation, please refer to [https://www.rabbitmq.com/download.html](https://www.rabbitmq.com/download.html)
-  Download and install Mongo DB with default preferences (Default port numbers, default server configuration)
	-  For more information about MongoDB installation, please refer to [https://docs.mongodb.org/manual/installation/](https://docs.mongodb.org/manual/installation/)
-  Download and install MySQL community edition with default preferences (In case you wish to use MySQL as Table Data Gateway)
	-  For more information about MySQL server installation, please refer to [http://dev.mysql.com/doc/refman/5.7/en/installing.html](http://dev.mysql.com/doc/refman/5.7/en/installing.html)
-  Initialize databases with the scripts (These scripts are only for use by querying)
	-  Execute the database initialization script file `mysql_query_databases.sql` under the `scripts` folder
-  (Optional) Install MySQL client management tools
	-  Please refer to [http://dev.mysql.com/downloads/workbench/](http://dev.mysql.com/downloads/workbench/ "MySQL Workbench")

## Run on Windows with Visual Studio 2015
Follow the instructions below to run WeText application from within Visual Studio 2015.

-  Open `WeText.sln` from Visual Studio 2015
-  Set `WeText.Service` project as startup project and run
-  Set `WeText.Web` project as startup project and run
-  Enjoy! ^_^

Note that the default configuration assumes that all the message queuing and database services are run on the same machine as the application (localhost). You can change the settings by editing the `App.config` file in `WeText.Service` application according to your environment setup. For debugging and demonstration purpose, running on the same local machine is recommended.

## Build and Run on Linux
The steps below are for building we-text service under Ubuntu 14.04.4 LTS. We-text service can also be built on other Linux variants such as CentOS.
### Install Mono
Building we-text service requires the Mono framework and xbuild tool chain. Please refer to [http://www.mono-project.com/docs/compiling-mono/linux/](http://www.mono-project.com/docs/compiling-mono/linux/) for more information about how to install Mono on your Linux machine.

### Install MongoDB
In we-text, MongoDB is used as the backend database for storing domain events. Please refer to [https://docs.mongodb.org/manual/tutorial/install-mongodb-on-ubuntu/](https://docs.mongodb.org/manual/tutorial/install-mongodb-on-ubuntu/) for more information about how to install MongoDB on your linux machine.

### Install RabbitMQ
For more information about how to install RabbitMQ on Linux systems, please refer to [http://www.rabbitmq.com/install-debian.html](http://www.rabbitmq.com/install-debian.html)

### Install MySQL
For more information about how to install MySQL on Linux systems, please refer to [https://www.digitalocean.com/community/tutorials/how-to-install-mysql-on-ubuntu-14-04](https://www.digitalocean.com/community/tutorials/how-to-install-mysql-on-ubuntu-14-04)

### Compile Source Code
1. Use the git command below to clone the repository:

		git clone https://github.com/daxnet/we-text
2. Switch into `we-text\scripts` folder, execute `mysql -u root -p` to start MySQL client, after the client has connected to the server successfully, execute `\. mysql_query_databases.sql` command to initialize the query database
3. 

# Screenshots
### Application Home Page
![](https://raw.githubusercontent.com/wiki/daxnet/we-text/img/ApplicationHomePage.png)

### My Texts
![](https://raw.githubusercontent.com/wiki/daxnet/we-text/img/ApplicationMyTexts.png)

### My Friends
![](https://raw.githubusercontent.com/wiki/daxnet/we-text/img/ApplicationMyFriends.png)

# Documentation
[Under Construction]
