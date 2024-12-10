# Note Taking App Web Project

## About
### This note-taking application was developed as my personal project for the Project Defense at Software University (SoftUni) in Sofia, Bulgaria, where I applied the software development knowledge I gained at the university while also learning a great deal during the development process.
This app helps users organize their thoughts and information with ease. Users can create, edit, and manage notebooks, folders, notes, and tags, with support for infinitely nested folders to handle complex organizational structures. Key features include searching notes by name, filtering by tags, and using a rich text editor for detailed notes. Additionally, each notebook and folder includes an overview page where users can write clear and simple summaries of their contents, providing context and making navigation through their work easier.

## The application has 2 roles:
### User Role

The **User** role is designed for regular users of the app who have basic functionality to interact with their notebooks, folders, and notes. A **User** can:

- Create, edit, and delete notebooks, folders, notes, and tags.
- Tag and filter notes by tags.
- Search notes by name.
- Use a rich text editor for detailed notes.
- Access overview pages to summarize notebooks and folders.

A **User's** access is restricted to their personal data, meaning they can only interact with the content they have created and do not have any access to other users' notebooks or administrative settings.

### Admin Role

The **Admin** role has elevated privileges for managing the app and overseeing its content. An **Admin** can:

- Perform all the actions available to **Users**, such as creating, editing, and deleting notebooks, folders, notes, and tags.
- Assign the Admin role to grant users administrative privileges.
- Remove the Admin role to restrict users' administrative privileges.
- Delete users and all their associated data from the system.

## Database diagram

![image](https://github.com/user-attachments/assets/fe23f24d-4046-4300-893d-08761505b9ab)
![image](https://github.com/user-attachments/assets/29e682fa-f3c4-48ec-8d2f-2504ff55cd18)

## 🛠️ Technologies Used

### Backend
- **ASP.NET Core**: A cross-platform, high-performance web framework for building modern applications.
- **Entity Framework Core**: A lightweight ORM for .NET that helps with database management and queries.
- **Microsoft SQL Server**: Relational database management system.
- **Git**: Version control system.
### Frontend
- **Bootstrap 5**: Responsive and modern UI components.
- **HTML**: Markup language for structuring web pages.
- **CSS**: Styling language for web pages.
### Testing
- **NUnit**: Comprehensive testing framework.
