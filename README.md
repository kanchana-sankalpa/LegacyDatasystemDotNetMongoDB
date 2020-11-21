# LegacyDatasystemDotNetMongoB

The .Net C# application intended to demostrate the use of C# with MongoDB to improve the repositary of many leagacy systems and facilaite the data searching. For auntantication and aunthorization application used JWT token and the actions and results triggered using REST API calls.
Application required to have simple structure of meta tables.

Meta table structure 
https://github.com/kanchana-sankalpa/LegacyDatasystemDotNetMongoDB/blob/main/UserAcess%20Model.PNG


All the legacy systems and there datasets has to feed to Legacy System and Dataset collections and the acess rights need to grantted to user, user roles and role dataset tables.
Then the new datasets required for text indexing using indexing Rest in SearchController.

The Search service provides the resulting data in Json as list of lists of BasonDocuments.

The Front end for the Application developed using the AngualrJs.
https://github.com/kanchana-sankalpa/LegacyDatasystemDotNetMongoFront

