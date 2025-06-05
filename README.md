# Simple Sample Blog App (.NET 6)

This is a Blog Web API developed as part of my software development portfolio. Built with ASP.NET Core, Entity Framework Core, and Identity, the API supports user registration, login, and logout using JWT-based authentication.

The API enables users to create and manage blog posts under two main categories based on mood type and post type. Users have the option to either publish their posts immediately or save them as drafts for future publication. This setup demonstrates essential features such as authentication, authorization, and CRUD operations using modern .NET technologies.

## Disclaimer
This repository is not intended for deployment in a production environment. It serves solely as a proof of concept for educational purposes.

The project incorporates various design patterns—some provided by the .NET Core framework and others that I’ve learned and experimented with independently. At its core, this is a lab-style project where I explore different implementation approaches for common functionalities such as authentication and authorization, TDD and design patterns like options pattern, strategy and others.

Feel free to use this repository in a similar way—for learning, experimentation, or as a starting point for your own ideas.

**Note on Project Status**

The WebAPI.Client and WebApp projects are still under active development. They are not yet fully functional and may undergo significant changes. These components are being built to extend the API's capabilities and provide a user interface for interacting with the backend services.

Stay tuned for updates as these projects evolve.

## Features

- ASP.NET Core Web API with JWT authentication
- Entity Framework Core with SQL Server support
- ASP.NET Core Identity for user and role management
- AutoMapper for object mapping
- Modular architecture with separate Business Logic and Data Access layers
- OpenAPI/Swagger documentation

## Technologies Used

- .NET 6
- ASP.NET Core Web API
- Entity Framework Core
- AutoMapper
- Swashbuckle (Swagger)
- Microsoft Identity

## Getting Started

**Prerequisites**

Make sure you have the following installed:

- [Docker Desktop](https://www.docker.com/products/docker-desktop/)
- [Microsoft Visual Studio Community 2022 ](https://visualstudio.microsoft.com/es/vs/community/)
- [Git](https://git-scm.com/)
- Optional: [Postman](https://www.postman.com/downloads/) or any API testing tool

### Setup

1. **Clone the repository:**

```
   git clone https://github.com/alfcanres/SimpleBlog.git

   cd SimpleBlog
```

2. **Setup SQL Server:**

Change "Your_password123" for the password you want to use on your local instance

```
   docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=Your_password123" \
   -p 1433:1433 --name sqlserver \
   -d mcr.microsoft.com/mssql/server:2019-latest
```

3. **Update appsettings**

   Go to appsettings.Development.json and change "Your_password123" for your password
```
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=localhost;Initial Catalog=SimpleBlog;User Id=sa; Password=Your_password123"
  }
```

4. **Run UPDATE-DATABASE command**

   Open Package Manager Console from Visual Studio, Set "Default Project" to DataAccessLayer and run the following command:
```
   UPDATE-DATABASE
```
   This command should create the database and the tables the project needs. 
  
   
5. **Run the project**

Notice **InitialData** from **appsettings.Development.json** , this object contains the initial data for the database  when the app runs for the first time. It creates the admin user, tests users, post types, mood types and even dummy posts. Feel free to modify this file as needed. 
```
  "InitialData": {
    "AdminUser": {
      "UserName": "admin",
      "Email": "admin@admin.com",
      "Password": "Your_password123",
      "Role": "Administrator"
    },
    "MoodTypes": {
      "Create": "True",
      "MoodTypes": [
        "Happy",
        "Sad",
        "Angry",
        "Excited",
        "Bored"
      ]

    },
    "PostTypes": {
      "Create": "True",
      "PostTypes": [
        "Question",
        "Thought",
        "Opinion",
        "Reflection",
        "Inspirational"
      ]
    },
    "DummyUsers": {
      "Create": "True",
      "Count": 15,
      "DefaultPassword": "Your_password123",
      "DummyNames": [
        "Alice",
        "Bob",
        "Charlie",
        "David",
        "Eve"
      ],
      "DummyLastNames": [
        "Smith",
        "Johnson",
        "Williams",
        "Jones",
        "Brown"
      ]
    },
    "DummyPosts": {
      "Create": "True",
      "Count": 500,
      "DummyPostTitles": [
        "discover",
        "amazing",
        "journey",
        "future",
        "create",
        "ideas",
        "explore",
        "challenge",
        "dream",
        "inspire",
        "learn",
        "share",
        "moment",
        "grow",
        "together",
        "change",
        "vision",
        "success",
        "believe",
        "story"
      ],
      "DummyPostContents": [
        "connect",
        "discoveries",
        "opportunity",
        "motivate",
        "focus",
        "energy",
        "progress",
        "community",
        "support",
        "challenge",
        "growth",
        "passion",
        "goal",
        "achievement",
        "collaborate",
        "imagine",
        "reflect",
        "advance",
        "encourage",
        "potential"
      ]
    }

  },
```   
   
   
Once you do have your "initial data" created, I strongly recommend to comment out line  152 from WebAPI\Program.cs so it will not run again the process when you launch the application next time. 

```
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDataInitializer(); //Create initial data in development mode. Just run once, then comment
}
```

6. **Have fun!**
