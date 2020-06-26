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


## Code Coverage
In order to create code coverage reports, this project also uses [Coverlet](https://github.com/coverlet-coverage/coverlet) and [Report Generator](https://danielpalme.github.io/ReportGenerator/), which can be included in some build pipelines like Azure Devops


## Create a .bat file with the following script in order to execute all the tests and see the code coverage in Chrome

> Remember to replace the things required in this script. This script runs on Windows

dotnet tool install --global coverlet.console :: THIS INSTALL COVERLET AS GLOBAL

dotnet tool install -g dotnet-reportgenerator-globaltool :: THIS INSTALL REPORT GENERATOR AS GLOBAL


:: REPLACE USING THE FOLDER OF THE SOLUTION

CD C:\Fuentes\JobOffersBackendDotNet\jobofferbackend\JobOfferBackendDotNet


:: REPLACE D:  WITH THE DISK FOR SAVING THE COVERAGE REPORT

dotnet test JobOfferBackendDotNet.sln /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:CoverletOutput=D:\TestResults\Coverage\


:: REPLACE D:  WITH THE DISK FOR READING THE COVERAGE REPORT

reportgenerator "-reports:D:\TestResults\Coverage\coverage.cobertura.xml" "-targetdir:D:\TestResults\Coverage\CodeCoverageReport" -reporttypes:HtmlInline


:: DEPLACE THE SOURCE OF CHROME.EXE AND THE PATH OF THE REPORT GENERATED

"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe" "D:\TestResults\Coverage\CodeCoverageReport\index.htm"

PAUSE

## How to contribute

If you want to contribute, please see the tab "Project" in the header and there you will find some tasks that I created to extend this project.
Feel free to create your Fork or clone the repo, create a feature branch and the send pull request directly to this repo.

Thank you very much!

