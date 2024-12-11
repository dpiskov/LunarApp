# LunarApp: A Note-Taking Web Application

## 📑 Table of Contents
- [✨ Overview](#-overview)
- [🔧 Roles and Permissions](#-roles-and-permissions)
  - [🧑‍💻 User Role](#-user-role)
  - [🛡️ Admin Role](#%EF%B8%8F-admin-role)
- [💾 Database Diagram](#-database-diagram)
- [🧾 User Permissions](#-user-permissions)
  - [🟢 Authenticated Users](#-authenticated-users)
  - [🔴 Unauthenticated Users](#-unauthenticated-users)
- [🗂 Pages](#-pages)
- [🔑 Test Accounts](#-test-accounts)
- [🌱 Database Seeding](#-database-seeding)
- [🛠️ Technologies Used](#%EF%B8%8F-technologies-used)
  - [Backend](#backend)
  - [Frontend](#frontend)
  - [Testing](#testing)
- [🚀 How to Run the App](#-how-to-run-the-app)
  - [🌟 Additional Notes](#-additional-notes)

## ✨ Overview
### LunarApp is a feature-rich note-taking application developed for my Project Defense at Software University (SoftUni) in Sofia, Bulgaria. It showcases the knowledge and skills I acquired at the university, as well as new concepts I learned during development.
This app helps users organize their thoughts and information with ease. Users can create, edit, and manage notebooks, folders, notes, and tags, with support for infinitely nested folders to handle complex organizational structures. Key features include searching notes by name, filtering by tags, and using a rich text editor for detailed notes. Additionally, each notebook and folder includes an overview page where users can write clear and simple summaries of their contents, providing context and making navigation through their work easier.

## 🔧 Roles and Permissions
### 🧑‍💻 User Role

The **User** role is designed for regular users of the app who have basic functionality to interact with their notebooks, folders, notes and tags. A **User** can:

- Create, edit, and delete notebooks, folders, notes, and tags.
- Tag and filter notes by tags.
- Search notes by name.
- Use a rich text editor for detailed notes.
- Access overview pages to summarize notebooks and folders.

| ⚠️ Data Privacy: Users can only interact with their own content and have no access to other users’ data or administrative settings.

### 🛡️ Admin Role

The **Admin** role has elevated privileges for managing the app. An **Admin** can:

- Perform all the actions available to **Users**, such as creating, editing, and deleting notebooks, folders, notes, and tags.
- Assign the Admin role to grant users administrative privileges.
- Remove the Admin role to restrict users' administrative privileges.
- Delete users and all their associated data from the system.

## 💾 Database diagram

![image](https://github.com/user-attachments/assets/fe23f24d-4046-4300-893d-08761505b9ab)
![image](https://github.com/user-attachments/assets/29e682fa-f3c4-48ec-8d2f-2504ff55cd18)

## 🧾 User Permissions

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
- 🔐 **Register Page**
- 🔒 **Login Page**


## 🗂 Pages

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
![image](https://github.com/user-attachments/assets/f74c621c-87a5-4fa0-a4a8-ec344bb7b0d8)

This is the User Management page:
![image](https://github.com/user-attachments/assets/64f2dee6-16d9-40d2-aa6c-8e471317c6af)

## 🔑 Test accounts:
- **Admin**
  - **Username**: admin@lunarapp.com
  - **Password**: admin123
- **User1**
  - **Username**: user1@lunarapp.com
  - **Password**: user123
- **User2**
  - **Username**: user2@lunarapp.com
  - **Password**: user123

## 🌱 Database Seeding:
Test users (**User1** and **User2**) are pre-seeded with unique test data, including:
- Notebooks
- Folders (with nested folders)
- Notes
- Tags

## 🛠️ Technologies Used

### Backend
- **ASP.NET Core**: High-performance web framework.
- **Entity Framework Core**: ORM for database management.
- **Microsoft SQL Server**: Relational database.
- **Git**: Version control.
### Frontend
- **Bootstrap 5**: Responsive and modern UI components.
- **HTML**: Web page structure and styling.
- **CSS**: Styling language for web pages.
- **TinyMCE**: Rich text editor for detailed and formatted notes.
### Testing
- **NUnit**: Comprehensive testing framework.

## 🚀 How to Run the App
1. Download the project.
2. Extract the files.
3. Open **LunarApp.sln** in Visual Studio.
4. Add your connection string to **appsettings.json**.
5. Ensure the startup project is set to **LunarApp.Web**.
6. Run the project.

### 🌟 Additional Notes
If migrations do not apply automatically:
1. Open the Package Manager Console in Visual Studio.
2. Run the command ```Update-Database``` to apply migrations manually.
3. Start the application.

---

Enjoy using LunarApp! 🌌
