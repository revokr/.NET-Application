# MotoRaceAdmin – Client-Server Application in .NET

##  Project Overview

**MotoRaceAdmin** is a client-server application developed in **.NET** for managing motorcycle races within a festival event. The application is designed for **administrators only** (clients), providing functionality to view races, add participants, and search participants by team.

The application uses **JSON** for data exchange between the client and server. Data consistency across clients is maintained through **server-triggered refreshes**.

---

##  Features

-  **Admin-only access**
-  **View all races**
-  **Add new participants to races**
-  **Search and view participants by team**
-  **Automatic client data refreshes triggered by server-side changes**
-  **Communication over TCP using JSON-encoded messages**

---

##  Architecture

- **Client**: Desktop application (e.g., using WPF or Windows Forms) for administrators
- **Server**: .NET backend handling multiple admin clients concurrently
- **Communication**: Custom JSON-based protocol over TCP sockets
- **Concurrency**: Server handles multiple clients using asynchronous sockets

---

##  Technologies Used

- `.NET` (Core / Framework)
- `C#`
- `TCP sockets`
- `JSON` (for serialization)
- `Multithreading` / `async-await` (on the server side)

---

##  Data Flow & Consistency

- All data is serialized to and from **JSON**
- When an admin performs an operation (e.g., adds a participant), the **server notifies all other connected clients** to refresh their local views
- This ensures all clients remain **synchronized and consistent**
