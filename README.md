[![Coverage Status](https://coveralls.io/repos/github/diegofox2/jobofferbackend/badge.svg)](https://coveralls.io/github/diegofox2/jobofferbackend)

# Job Offer Backend

This project is a real-world example of DDD in a backend application

It applies the concept of Entity, Value Object, Root, Aggregate, Services, Repositories and Ubiquitous Language

There are some classic unit tests based on Services but also there are some tests over domain entities as well

[See Class Diagram](https://drive.google.com/file/d/1uShlMMqiRUyP1xUp-wJ1Lpi65frffNCy/view?usp=sharing)

Also, this project doesn't use interfaces like many projects do because  in practice they are not really needed to do Unit Testing neither Inversion of Control. To go deeper on this topic, read  [Fake Abstractions](https://medium.com/@dcamacho31/foreword-224a02be04f8) and [Header Interface](https://martinfowler.com/bliki/HeaderInterface.html)


## Data layer
This project uses MongoDB as a database engine through [MongoDriver](https://docs.mongodb.com/drivers/csharp) for C#

Nowadays, new projects do not need relational databases to store data. Instead, NoSQL databases like Mongo let you reduce a lot of code because ORM's like both Entity Framework and Dapper are no longer necessary on this paradigm.
In addition, NoSQL databases are really much faster than relational databases, and most of their important features like Constraints, Triggers, Indexes, Security, Mirror Replication and Load Balance now are available for main NoSQL databases, like MongoDB.

## How to run it

Download MongoDB and install it with default credentials because the project use it as it is. Otherwise, you will need to update the credentials into the project.

You must to run the console application "InitialDataCreator" so that you have a basic data set preloaded in your mongo database.

Before you set as startup proyect "JobOfferBackend.WebAPI" and run it, you have to be sure you have [installed .Net Core SDK 2.2](https://dotnet.microsoft.com/download/dotnet/thank-you/sdk-2.2.207-windows-x64-installer)

This WebAPI project is running Swagger in order to see and test the endpoints. You only have to open the browser and go to localhost:[your port]/swagger.

## Code Coverage
In order to create code coverage reports, this project also uses [Coverlet](https://github.com/coverlet-coverage/coverlet) and [Report Generator](https://danielpalme.github.io/ReportGenerator/), which can be included in some build pipelines like Azure Devops


## Create a .bat file with the following script in order to execute all the tests and see the code coverage in Chrome

> Remember to replace the things required in this script. This script runs on Windows

:: THIS INSTALL COVERLET AS GLOBAL

dotnet tool install --global coverlet.console 

:: THIS INSTALL REPORT GENERATOR AS GLOBAL

dotnet tool install -g dotnet-reportgenerator-globaltool 

:: THIS INSTALL COVERALLS.NET

dotnet tool install -g coveralls.net --version 1.0.0 


:: REPLACE USING THE FOLDER OF THE SOLUTION

CD C:\Fuentes\JobOffersBackendDotNet\jobofferbackend

:: DELETE PREVIOUS CODE COVERAGE FILES AND REPORT

RMDIR C:\SourceCode\JobOffersTestResult /S /Q

:: REPLACE D: WITH THE DISK FOR SAVING THE COVERAGE REPORT

dotnet test JobOfferBackendDotNet.sln /p:CollectCoverage=true /p:CoverletOutput=D:\TestResults\Coverage\ /p:MergeWith="D:\TestResults\Coverage\coverage.json" /p:CoverletOutputFormat=\\"cobertura,json\\" -m:1

:: REPLACE D: WITH THE DISK FOR READING THE COVERAGE REPORT

reportgenerator "-reports:D:\TestResults\Coverage\coverage.cobertura.xml" "-targetdir:D:\TestResults\Coverage\CodeCoverageReport" -reporttypes:HtmlInline


:: DEPLACE THE SOURCE OF CHROME.EXE AND THE PATH OF THE REPORT GENERATED

"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe" "D:\TestResults\Coverage\CodeCoverageReport\index.htm"

PAUSE
