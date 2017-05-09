ASP.NET File Application

Problem: realization the requirements of "ASP.NET File Application"

Description: https://github.com/akovrigin/FileApp/blob/master/WebApp/FileApp.doc

Source code: https://github.com/akovrigin/FileApp

Deployment:
This project uses Telerik ASP.NET WebForms components (telerik.com), so it's necessary to install it first.
Get source code and build project in Visual Studio.
Application (WebApp) can be executed directly from Visual Studio. In this case the simple way is to deploy it on IIS Express (by default) and go to url: http://localhost:64055/Default.aspx

Configuration:
Before execution it is necessary to set in web.config file value of "MainFolder" key to "root" folder on the disk of the file system.
<appSettings>
  <add key="MainFolder" value="C:\Temp\FileApp\" />
</appSettings>

To test application it is enough just to select the root "folder" on the Default.aspx page in the browser and right click on it. Then it’s allowed to create a new folder or to upload a file. After that there will be available other action on created folders and uploaded files: copy, rename, delete and download exactly for the files. On the right part of the page label "Meta-data" shows the size of the selected file or of the selected folder (with the sum of sizes of all nested folders and files).

The description of the design.

"CoreLibrary" is the project of business logic that contains all the classes for work with tree-structure of folders and files in memory and process operations like save and load to/from disk of the file system. It’s possible to use this library in another project. For example, in unit-test projects: "UnitTests", "IntegrationTests".

Element is the base class that represents folders, files and other objects, that can be used in the future. It has interface IElement for primary actions. For a folder-like object it can be used an interface IContainer. For objects like a file it can be possible to use an interface IHasData which allows binding data to the object. For file it can be possible to make some addition processing by using an object with an interface IProcessibility which can be injected through one of the constructor of the file-class. For supporting new types of actions it can be possible to use a class with an IVisitor interface (for example the EditVisitor class). To make a new type of objects we can inherit from Element class or just realize an IElement interface. This realization of library bases on the file system and use class FileSystemStorage for purpose to communicate with file system.  But it’s possible to realize DataBaseStorage class methods and use this library to store the data in a database. In any case there is the singleton class Storage which provides the access to selected data storage.

Database has to have a table Element to store all the objects (folders and files) with Id-ParentId structure. One another table PropertyType has to store names of properties of objects with fields Id, ObjectType, PropertyType, PropertyName. And the table ElementProperty to store the values of these properties: Id, PropertyTypeId, Value. In this table it’s possible to have reference to one another table that can store files data.

"WebApp" is the project to demonstrate how to use "CoreLibrary" classes and test the requirements of the "ASP.NET File Application".

To use "IntegrationTests" it’s necessary to extract FileApp_Test.7z to "C:\Temp\" folder.
