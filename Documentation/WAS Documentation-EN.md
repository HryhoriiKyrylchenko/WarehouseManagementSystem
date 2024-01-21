#Warehouse Management System**

##Documentation**

1. **Introduction:** The developed system is software designed to automate warehouse management processes. It is created with the aim of improving inventory management efficiency, reducing accounting errors, and providing a reliable foundation for future expansion into an online store. The system will be oriented towards the use of technologies such as C#, WPF, Entity Framework Core, and SQL Server.

2. **Problem Statement:**
   1. Engineering Problem Statement:
      - Development of a warehouse management system with a graphical interface based on WPF.
      - Creation of a database for storing information about goods, deliveries, and sales using SQL Server.
      - Implementation of functionality for adding, editing, and deleting goods.
      - Ensuring data security and user authorization.
   2. Mathematical Problem Statement:
      - Definition of a mathematical model for calculating the current state of inventory.
      - Development of algorithms to optimize accounting processes and identify potential errors.

3. **System Functional Requirements:**
   1. Goods Registration:
      - Addition of new goods to the system.
      - Entry of basic information about the product: name, description, units of measurement, etc.
   2. Inventory Management:
      - Tracking current stock levels.
      - Automatic reduction of stock upon shipment of goods.
      - Manual adjustment of stock levels.
   3. Receipt Management:
      - Registration of incoming goods.
      - Association of receipts with suppliers.
      - Ability to add additional information such as receipt date, batch number, etc.
   4. Shipment Tracking:
      - Registration of goods shipments from the warehouse.
      - Linking shipments to customers or orders.
      - Ability to provide additional information such as shipment date, tracking number, etc.
   5. Goods Movement Monitoring:
      - Tracking the history of goods movement within the warehouse.
      - Monitoring the movement of goods between different warehouse zones.
   6. Expiry Date Control:
      - Monitoring the expiration dates of goods.
   7. Support for Various Types of Goods:
      - Ability to work with different types of goods (products, materials, equipment, etc.).
   8. Report Generation:
      - Generation of reports on the current state of the warehouse.
   9. Data Security:
      - Protection of data from unauthorized access.
   10. Multi-User Access Support:
	  - Ability to work with the system for multiple users with different access levels.

4. **Technical Specifications:**
   1. Server-side: Microsoft SQL Server for data storage and management.
   2. Client-side: Development using C# and WPF technologies.
   3. Database access through Entity Framework Core to simplify data operations.

5. **Software Characteristics:**
   1. Development in the C# programming language.
   2. Use of WPF technology for creating the graphical interface.
   3. Integration of Entity Framework Core for working with the SQL Server database.
   4. Data security ensured through authentication and authorization mechanisms.

6. **System Design:**
   1. Interface Design:
      The warehouse management system interface should be intuitive and user-friendly, with key elements including:
      1. Main Window:
         - Navigation panel for quick access to sections: "Goods," "Movements," "Receipts," "Shipments," "Reports," "Settings."
         - Brief summary of the current warehouse state.
      2. Goods Registration:
         - Form for adding new goods with fields for name, description, units of measurement, and other basic information.
      3. Inventory Management:
         - Table displaying current stock levels.
         - Ability to manually adjust stock levels.
      4. Goods Movement Management:
         - Table displaying distributed goods.
         - Form for managing goods movement.
      5. Receipts and Shipments Management:
         - Forms for registering receipts and shipments with relevant information.
      6. Reports and Analytics:
         - Report generation system with parameter and time interval selection capabilities.
		 
   2. Class Diagram Design:
	  The class diagram will include components such as:
	  1. Model classes
	  2. Builder classes
	  3. ViewModel classes
	  4. Service manager classes
	  5. Other auxiliary classes
	  
   3. Database Structure:
	  In order to leverage the capabilities of MS SQL Server for efficient data management, the system's structure will be designed to align with the principles of relational database organization. The database structure may be represented by classes such as:
	  - Address
      - Customer
      - ErrorLog
      - Label
      - Manufacturer
      - MovementHistory
      - Product
      - ProductCategory
      - ProductInZonePosition
      - ProductPhoto
      - Receipt
      - ReceiptItem
      - Report
      - Shipment
      - ShipmentItem
      - Supplier
      - User
      - Warehouse
      - Zone
      - ZoneCategory
      - ZonePosition
	  
   4. System Structure Design:
	  To ensure an organized and modular architecture, the system structure will be designed based on the MVVM (Model-View-ViewModel) pattern. This pattern separates the application components into three key layers: Model, View, and ViewModel, facilitating a clear and maintainable structure.
	  - **Model:**
	    The Model layer will represent the core business logic and data entities. In the context of the warehouse management system, this includes entities such as Goods, Receipts, Shipments, and other relevant tables in the MS SQL Server database.

	  - **ViewModel:**
	    The ViewModel layer will act as an intermediary between the Model and the View, handling the presentation logic and user interaction. It encapsulates the data and operations required for the View, promoting a separation of concerns. This layer will be instrumental in coordinating the flow of data and commands between the user interface and the underlying data model.
	  
	  - **View:**
  	    The View layer will be responsible for presenting the user interface to the end user. Developed using WPF technologies, it will interact with the ViewModel to display data and capture user input. The user interface will be designed to be intuitive and user-friendly, following the principles of the warehouse management system.
	  
	  - **Database Design:**
	    The database structure, implemented in MS SQL Server, will comprise tables reflecting the entities in the system. Relationships between tables will be established using foreign keys to ensure data integrity. Views and stored procedures may also be employed to streamline complex queries and operations.

	  - **MVVM Implementation:**
	    The MVVM pattern will guide the design and development process, promoting a scalable and maintainable codebase. Data binding and commands will be used to establish a seamless connection between the View and the ViewModel, allowing for efficient data presentation and user interaction.

	  By adopting the MVVM pattern, the system's structure will not only enhance code organization but also support testability, extensibility, and collaboration among development teams working on different aspects of the application. This architectural choice aligns with best practices for creating robust and user-friendly applications with WPF and MS SQL Server.

   5. Algorithm Design:
      1. Goods Registration:
         - User enters information about a new product; the system checks for uniqueness and adds it to the database.
      2. Inventory Management:
         - Upon goods shipment, the system automatically reduces the quantity in the database.
      3. Goods Movement Monitoring:
         - The system tracks the movement of goods between zones and updates information about their location.
      4. Report Generation:
         - Reports are generated based on user-selected parameters and queries to the database.
      5. Data Security:
         - Implementation of authentication and authorization mechanisms to protect data.
      6. Multi-User Access Support:
         - The system provides access management for different user levels.

7. **User Guide (Sequence of User Actions):**

   Sequence of actions:

   1. Log in to the system:
      - Enter the username and password.
   2. Work with goods:
      - Add a new product.
      - Edit product information as needed.
   3. Receipts and Shipments:
      - Register the receipt of goods in the warehouse.
      - Register the shipment of goods from the warehouse.
   4. Goods Movement Management:
      - View the locations of goods within the warehouse.
      - Move goods within the warehouse if necessary.
   5. Expiry Date Control:
      - View information about the expiration dates of goods.
      - Edit the expiration date of a product if necessary.
   6. Report Generation:
      - Generate a report on the current state of the warehouse.

8. **References:**
   - Microsoft Docs: WPF Documentation
   - Microsoft Docs: Entity Framework Documentation

