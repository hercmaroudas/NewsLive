----------------
   Read Me 
----------------

This application demonstrates using Entity Framework Database First method for data access and therefor
assumes that the database exists before running the application. 

Please follow the steps below in order to run and debug the website without any issues. Later in the
document can see the overall architecture and a simple explanation accompanying it. 

----------
1. Setup
----------

    --------------------
    1.1. Database Setup
    --------------------

    Open and run the SQL script named, "create_news_database.sql" which can be found in the root directory 
    in the folder named, "DatabaseScripts". 

    The script creates a database names, "NewsLive" and all the tables necessary, with basic data in 
    order to run the application prior to doing anything. The script also creates two database diagrams
    namely, "dbo.Business" that shows the relationship between the business aspect of the application 
    in relational terms. The other is, "dbo.Membership" that shows the simple relationship for an 
    individual of the application. 

    PLEASE NOTE: the application currently does not support a complex model for creating and 
    authenticating users and may be enhanced in future to demonstrate this. The design has been created
    with extensibility in mind.
    
    ----------------------------------------
    1.1.1. Known issues running the script.
    ----------------------------------------
    
      This script and the database was created using Sql Server Management Studio (SSMS) for SQL 2014.
      
      Running the script with COMPATIBILITY_LEVEL = 120 may cause the script to fail, although it will 
      continue to create the database. It is safe to ignore the errors.
  
    --------------------
    1.2. Solution Setup
    --------------------
    Locate the solution file named, "NewsLive.sln" in the root directory and open it. It should be 
    straight forward, open and compile with no issues. 
  
      -----------------------
      1.2.1. Startup Project
      -----------------------
      Make sure the, "NewsLive.Angular" project is the Startup application. Right click the project 
      and from the context menu click on "Set as Startup Project".
    
      -----------------------
      1.2.2. Latest Packages
      -----------------------

      Although the application includes all the packages that are needed in order to run, there has 
      been a known issue whereby the application fails to run if Entity Framework is not installed 
      in the project NewsLive.Angular. If this is the case then install Entity Framework v6.x on the 
      solution. Current version to date being used is v6.1.3.

      To install Entity Framework using NuGet Package manager, right click on the project, "NewsLive.
      Angular" and from the context menu select, Manage NuGet Packages..." from the context menu. 
      Usually the first choice by default is Entity Framework, click it and install it on the project.
      If it does not exist, search for it and install it on the project.
    
---------------------
2. Using the Website
---------------------

    -------------------
    2.1. Default Users
    -------------------

    After running the database scripts from section 1.1. you will notice that some data has been 
    seeded or created for you. 

    There are 3 members of which 2 members have a "Publisher" role and 1 member, Marry Murray has an 
    Employee role. This is to satisfy the requirement found in the document named, "initial.docx", 
    found in the root directory of the application, within the folder named."BusinessRequirement".

    In order to login simply use the username of the user found in the Membership table and login 
    with a blank password.

    PLEASE NOTE: The application is by no means a complete application, but rather is to demonstrate 
    from the ground up creating and architecting a simple requirement into something usable.
  
----------------
3. Architecture
----------------

    -------------------------
    3.1. NewsLive.DataAccess
    -------------------------

    The application is a typical 3 tier application, with the NewsLive.DataAccess serving as the 
    business layer, in that it performs all and encapsulates business requirement type tasks.

    This layer is decoupled and isolated in order to separate business logic with presentation logic, 
    that in turn aids for isolated testability and can serve as a central point for any other services
    or applications that need to consume and manipulate business centric activities within the business 
    domain. 

    Ultimately this level of the application is not be publicly available, meaning it is not exposed 
    to the public domain outside of the company. Rather this serves as a service within the 
    organisation. The reason being is, this layer exposes methods which allow direct access the database. 
    It would not be ideal for anyone to do this for obvious reasons, the primary one being security.

    Database and data manipulation is done using Entity Framework, commonly using a direct database
    connection to a SQL database. Methods performed at this level do not AND should not contain any 
    complexities in order to select, insert, update or delete from the database, however at this level 
    certain precautions action should be taken to avoid application failure when accessing data. 
    The database contains tables and rows. To select, insert, update or delete from the database we 
    need to access methods in order to achieve this. Using Entity Framework objects are mapped directly 
    to each table belonging to the database, the term for this type of database communication is typically
    known as ORM (Object Relational Mapping). Simply when we want to get data from a table, then we 
    simply get data from a table, when we want to insert, then we simply insert data and so forth and 
    so forth for update and delete. The next point in the application contains the business related tasks,
    for example, get me all Articles for an Author where the Articles are published. The ORM part is get 
    me all Articles for the author and the Repository service layer will check or calculate if the articles
    are published or not. Let's take a look at the functionality of the Repository.

    The application uses Repository objects, these are the meat of the application so to speak. These 
    repositories encapsulate the business logic by performing computations, looking at what rules need 
    to be satisfied before being able to manipulate or return data. Once performed successfully, 
    any data to be retrieved is mapped and returned in an object that encapsulates the particular task
    performed. Often these type objects are referred to as DTO's (Dee Tee Ows), which stands for Data 
    Transfer Object, which simply is an object created in order to encapsulate a certain business task
    and transfer that encapsulated object to the consumer, consumer being the service that calls the
    particular business function. This also allows us to only return back an object with properties that 
    are important for that specific task. In the application these objects are named Models. 

    In the application the repositories contain all the business logic required to manipulate the data. As 
    an example the ArticleRepository contains all the tasks to perform on retrieving, inserting, updating,
    and deleting articles from the database, satisfying any requirements. This allows us to also visually 
    see and make sense of the application domain and gives us a quick logical view to find any business 
    functions related to an article. In isolation it is easy to see and identify if a task you wish to 
    perform on an Article exists, if not a request can be done to create it. 

    Further more, in order to separate the database entities from the repository objects, a simple data 
    access object is used, in the application the object is called DataService. This service encapsulates 
    all direct database access methods on all tables and serves as a central service to use in all the 
    repositories for manipluating data. Because logically the application doesn't mapp directly to a 
    database schema, we will need a mechanism to join the two. For example, we may need to get all articles 
    and articles that have been liked. At some point we will need to separate objects form a one dimensional  
    database view of calling or performing a task on a single entity. Keeping our repositories separate is 
    a good thing, as explained before, this provides a better architecture which enables us to put in 
    place more robust testing framework, and also aids in overall better readability and maintainability of 
    the application.

    To give an example on what was mentioned above, so to demonstrate in order to better understand, lets say
    we wish to obtain an article and all the articles likes. knowing articles and likes are represented
    relationally in separate tables, we will at some point in the ArticleRepository need to get the associated 
    likes for that article. Our data access object exposes all objects in which we can perform data centric 
    tasks on both articles and article likes, and any other direct database tasks, such as article comments
    etc. At some point if the application grows, this service can be broken down into smaller service objects 
    which service particular aspects of the business domain. For now, there's no need to complicate things 
    and no reason why it cannot exist as a single object, given the simple business domain.
  
  
    -------------------------
    3.2. NewsLive.Angular
    -------------------------
  
    Although contained within a single project, this project serves primarily as a web service layer. Ideally 
    the actual application containing the views would be separate, but for demonstration and development 
    there's no reason other than making things simpler for coding and debugging to keep them as one.

    The /API is the publically available service that is an ensd point to any application being developed for 
    the bisiness. These services are named according to the function they perform. They serve as a proxy to 
    the presentation layer and the data access layer. These services are RESTFul services which allow an 
    application to interact with the business related functions by, retrieving, inserting, updating and deleting
    using web verbs GET, POST, PUT and DELETE respectively.

    The application presentation is represented by HTML markup and the means of generating and manipulating the 
    views according to the business requirements, driven by the data access layer.

  
-------------------------
4. Whats Next
-------------------------

    --------------------------------
    4.1. Extending the Architecture
    --------------------------------
    
    The current design is not sufficient for large sets of data. We are able to query for articles sufficiently
    only because there are a few articles and associated likes, comments and comment likes. But what if we had 
    thousands of article rows? Not to mention all the sub rows of likes and comments associated with comment 
    likes.
    
    One of the huge issues currently is, when retrieving articles from the data access layer, all the article 
    likes, comments and comment likes associated with each article are returned. As you can imagine this is not 
    very practical when retrieving articles from a large subset of articles. The reason why all child rows are 
    returned is because the data when queried using Entity Framework under the current setup is setup to use 
    Lazy Loading. This means that the Entity Framework automatically loads a related entity when the navigation 
    property for that entity is dereferenced. Articles contains a navigation property of Likes and comments and 
    so comments contains a navigation property to comment likes and so forth.
    
    Next we will look at different ways of loading data using Entity Framework and a better more robust and 
    efficient way to query data from our application.
    


-------------------------
  The End
-------------------------