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



## Functionality
## User Permissions

### 🟢 Authenticated Users
Authenticated users can access the following:
- 🏠 **Home Page**
- 📔 **Notebook**: Index, Create, Edit, Remove, Details
- 📂 **Folder**: Index, Create, Edit, Remove, Details
- 📝 **Note**: Create, Edit, Remove
- 🏷️ **Tag**: Index, Create, Edit, Remove

### 🔴 Unauthenticated Users
Unauthenticated users can access:
- 🏠 **Home Page**
- 🔑 **Register Page**
- 🔒 **Login Page**



This is the Home page:
![image](https://github.com/user-attachments/assets/1c7eb8c7-30fc-4745-ba54-b0eca21c90b8)

This is the Index Notebook page:
![image](https://github.com/user-attachments/assets/87c17550-5cde-412f-84e0-2d44aec51dd1)

This is the Index Folder page:
![image](https://github.com/user-attachments/assets/1e61ced0-a15b-436d-a23d-067d38649897)

This is the Note Create page, take note that you can go to the Index Tag page from the "View All Tags" button in the bottom right, where you can see all tags:
![image](https://github.com/user-attachments/assets/475c77ba-7dc9-47af-a873-99a302d62e76)

This is the Index Tag page:
![image](https://github.com/user-attachments/assets/38d654ee-e323-4acf-865d-79da2785735d)

Pages for users authorized with the **Admin** role:

This is the Home page:
![image](https://github.com/user-attachments/assets/40779645-f17b-4ccf-a200-33263de0d963)

This is the Index Admin page:
![image](https://github.com/user-attachments/assets/732e0235-7057-410d-8e04-cd151c161e71)

This is the User Management page:
![image](https://github.com/user-attachments/assets/20dee58b-65f0-4a47-9307-de85e641ff0b)

## Test accounts:
- **Admin**
  - **Username**: admin@lunarapp.com
  - **Password**: admin123
- **User1**
  - **Username**: user1@lunarapp.com
  - **Password**: user123
- **User2**
  - **Username**: user2@lunarapp.com
  - **Password**: user123

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
- **TinyMCE**: A rich text editor for creating detailed and formatted notes.
### Testing
- **NUnit**: Comprehensive testing framework.
