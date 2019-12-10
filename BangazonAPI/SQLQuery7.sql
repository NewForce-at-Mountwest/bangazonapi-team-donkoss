DELETE FROM OrderProduct;
DELETE FROM ComputerEmployee;
DELETE FROM EmployeeTraining;
DELETE FROM Employee;
DELETE FROM TrainingProgram;
DELETE FROM Computer;
DELETE FROM Department;
DELETE FROM [Order];
DELETE FROM PaymentType;
DELETE FROM Product;
DELETE FROM ProductType;
DELETE FROM Customer;
 
 
ALTER TABLE Employee DROP CONSTRAINT [FK_EmployeeDepartment];
ALTER TABLE ComputerEmployee DROP CONSTRAINT [FK_ComputerEmployee_Employee];
ALTER TABLE ComputerEmployee DROP CONSTRAINT [FK_ComputerEmployee_Computer];
ALTER TABLE EmployeeTraining DROP CONSTRAINT [FK_EmployeeTraining_Employee];
ALTER TABLE EmployeeTraining DROP CONSTRAINT [FK_EmployeeTraining_Training];
ALTER TABLE Product DROP CONSTRAINT [FK_Product_ProductType];
ALTER TABLE Product DROP CONSTRAINT [FK_Product_Customer];
ALTER TABLE PaymentType DROP CONSTRAINT [FK_PaymentType_Customer];
ALTER TABLE [Order] DROP CONSTRAINT [FK_Order_Customer];
ALTER TABLE [Order] DROP CONSTRAINT [FK_Order_Payment];
ALTER TABLE OrderProduct DROP CONSTRAINT [FK_OrderProduct_Product];
ALTER TABLE OrderProduct DROP CONSTRAINT [FK_OrderProduct_Order];
 
 
DROP TABLE IF EXISTS OrderProduct;
DROP TABLE IF EXISTS ComputerEmployee;
DROP TABLE IF EXISTS EmployeeTraining;
DROP TABLE IF EXISTS Employee;
DROP TABLE IF EXISTS TrainingProgram;
DROP TABLE IF EXISTS Computer;
DROP TABLE IF EXISTS Department;
DROP TABLE IF EXISTS [Order];
DROP TABLE IF EXISTS PaymentType;
DROP TABLE IF EXISTS Product;
DROP TABLE IF EXISTS ProductType;
DROP TABLE IF EXISTS Customer;
 
 
CREATE TABLE Department (
    Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
    [Name] VARCHAR(55) NOT NULL,
    Budget     INTEGER NOT NULL,
    Archived BIT NOT NULL DEFAULT(0)
);
 
CREATE TABLE Employee (
    Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
    FirstName VARCHAR(55) NOT NULL,
    LastName VARCHAR(55) NOT NULL,
    DepartmentId INTEGER NOT NULL,
    IsSuperVisor BIT NOT NULL DEFAULT(0),
    Archived BIT NOT NULL DEFAULT(0),
    CONSTRAINT FK_EmployeeDepartment FOREIGN KEY(DepartmentId) REFERENCES Department(Id)
);
 
CREATE TABLE Computer (
    Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
    PurchaseDate DATETIME NOT NULL,
    DecomissionDate DATETIME,
    Make VARCHAR(55) NOT NULL,
    Manufacturer VARCHAR(55) NOT NULL,
    Archived BIT NOT NULL DEFAULT(0)
);
 
CREATE TABLE ComputerEmployee (
    Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
    EmployeeId INTEGER NOT NULL,
    ComputerId INTEGER NOT NULL,
    AssignDate DATETIME NOT NULL,
    UnassignDate DATETIME,
    CONSTRAINT FK_ComputerEmployee_Employee FOREIGN KEY(EmployeeId) REFERENCES Employee(Id),
    CONSTRAINT FK_ComputerEmployee_Computer FOREIGN KEY(ComputerId) REFERENCES Computer(Id)
);
 
 
CREATE TABLE TrainingProgram (
    Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
    [Name] VARCHAR(255) NOT NULL,
    StartDate DATETIME NOT NULL,
    EndDate DATETIME NOT NULL,
    MaxAttendees INTEGER NOT NULL,
    Archived BIT NOT NULL DEFAULT(0)
);
 
CREATE TABLE EmployeeTraining (
    Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
    EmployeeId INTEGER NOT NULL,
    TrainingProgramId INTEGER NOT NULL,
    CONSTRAINT FK_EmployeeTraining_Employee FOREIGN KEY(EmployeeId) REFERENCES Employee(Id),
    CONSTRAINT FK_EmployeeTraining_Training FOREIGN KEY(TrainingProgramId) REFERENCES TrainingProgram(Id)
);
 
CREATE TABLE ProductType (
    Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
    [Name] VARCHAR(55) NOT NULL,
    Archived BIT NOT NULL DEFAULT(0)
);
 
CREATE TABLE Customer (
    Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
    FirstName VARCHAR(55) NOT NULL,
    LastName VARCHAR(55) NOT NULL,
    Archived BIT NOT NULL DEFAULT(0)
);
 
CREATE TABLE Product (
    Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
    ProductTypeId INTEGER NOT NULL,
    CustomerId INTEGER NOT NULL,
    Price MONEY NOT NULL,
    Title VARCHAR(255) NOT NULL,
    [Description] VARCHAR(255) NOT NULL,
    Quantity INTEGER NOT NULL,
    Archived BIT NOT NULL DEFAULT(0),
    CONSTRAINT FK_Product_ProductType FOREIGN KEY(ProductTypeId) REFERENCES ProductType(Id),
    CONSTRAINT FK_Product_Customer FOREIGN KEY(CustomerId) REFERENCES Customer(Id)
);
 
 
CREATE TABLE PaymentType (
    Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
    AcctNumber INTEGER NOT NULL,
    [Name] VARCHAR(55) NOT NULL,
    CustomerId INTEGER NOT NULL,
    Archived BIT NOT NULL DEFAULT(0),
    CONSTRAINT FK_PaymentType_Customer FOREIGN KEY(CustomerId) REFERENCES Customer(Id)
);
 
CREATE TABLE [Order] (
    Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
    CustomerId INTEGER NOT NULL,
    PaymentTypeId INTEGER,
    Archived BIT NOT NULL DEFAULT(0),
    CONSTRAINT FK_Order_Customer FOREIGN KEY(CustomerId) REFERENCES Customer(Id),
    CONSTRAINT FK_Order_Payment FOREIGN KEY(PaymentTypeId) REFERENCES PaymentType(Id)
);
 
CREATE TABLE OrderProduct (
    Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
    OrderId INTEGER NOT NULL,
    ProductId INTEGER NOT NULL,
    CONSTRAINT FK_OrderProduct_Product FOREIGN KEY(ProductId) REFERENCES Product(Id),
    CONSTRAINT FK_OrderProduct_Order FOREIGN KEY(OrderId) REFERENCES [Order](Id)
);
 
 
INSERT INTO Computer (PurchaseDate, DecomissionDate, Make, Manufacturer, Archived) VALUES ('20120618 10:34:09 AM', '20190922 12:22:55 AM', 'Mac', 'Apple', 'true');
INSERT INTO Computer (PurchaseDate, DecomissionDate, Make, Manufacturer, Archived) VALUES ('20130512 11:11:11 AM', '20190513 05:22:33 AM', 'Windows', 'Microsoft', 'false');
INSERT INTO Computer (PurchaseDate, DecomissionDate, Make, Manufacturer, Archived) VALUES ('20120618 09:09:09 PM', '20190922 06:12:11 PM', 'Mac', 'Apple', 'false');
INSERT INTO Computer (PurchaseDate, DecomissionDate, Make, Manufacturer, Archived) VALUES ('20140322 06:09:06 PM', '20190907 04:44:22 PM', 'Windows', 'Microsoft', 'true');
 
INSERT INTO Customer (FirstName, LastName, Archived) VALUES ('Frank', 'Blank', 'true');
INSERT INTO Customer (FirstName, LastName, Archived) VALUES ('Steve', 'Reves', 'false');
INSERT INTO Customer (FirstName, LastName, Archived) VALUES ('Terry', 'Berry', 'true');
INSERT INTO Customer (FirstName, LastName, Archived) VALUES ('Hugh', 'Blue', 'false');
 
INSERT INTO Department (Name, Budget, Archived) VALUES ('Sales', '999999', 'false');
INSERT INTO Department (Name, Budget, Archived) VALUES ('Accounting', '1', 'false');
INSERT INTO Department (Name, Budget, Archived) VALUES ('Human Resources', '55555', 'true');
INSERT INTO Department (Name, Budget, Archived) VALUES ('Marketing', '333333', 'false');
 
INSERT INTO TrainingProgram ([Name], StartDate, EndDate, MaxAttendees, Archived) VALUES ('Sexual Harrassment 101', '20190618 10:34:09 AM', '20200618 10:34:09 AM', '25', 'false');
INSERT INTO TrainingProgram ([Name], StartDate, EndDate, MaxAttendees, Archived) VALUES ('HIPPA', '20190618 10:34:09 AM', '20200618 10:34:09 AM', '25', 'false');
INSERT INTO TrainingProgram ([Name], StartDate, EndDate, MaxAttendees, Archived) VALUES ('Compliance', '20190618 10:34:09 AM', '20200618 10:34:09 AM', '25', 'false');
INSERT INTO TrainingProgram ([Name], StartDate, EndDate, MaxAttendees, Archived) VALUES ('OSHA 30', '20190618 10:34:09 AM', '20200618 10:34:09 AM', '25', 'false');
 
INSERT INTO ProductType ([Name], Archived) VALUES ('Electronics', 'false');
INSERT INTO ProductType ([Name], Archived) VALUES ('Toys', 'false');
INSERT INTO ProductType ([Name], Archived) VALUES ('Clothing', 'false');
INSERT INTO ProductType ([Name], Archived) VALUES ('Furniture', 'false');
 
INSERT INTO Employee (FirstName, LastName, DepartmentId, IsSupervisor, Archived) VALUES ('John', 'Smith', '1', 'false', 'false');
INSERT INTO Employee (FirstName, LastName, DepartmentId, IsSupervisor, Archived) VALUES ('Alice', 'Johnson', '2', 'true', 'false');
INSERT INTO Employee (FirstName, LastName, DepartmentId, IsSupervisor, Archived) VALUES ('Louise', 'Allen', '1', 'false', 'false');
INSERT INTO Employee (FirstName, LastName, DepartmentId, IsSupervisor, Archived) VALUES ('Steven', 'Michaelson', '1', 'true', 'false');
 
INSERT INTO EmployeeTraining (EmployeeId, TrainingProgramId) VALUES ('1', '2');
INSERT INTO EmployeeTraining (EmployeeId, TrainingProgramId) VALUES ('2', '1');
INSERT INTO EmployeeTraining (EmployeeId, TrainingProgramId) VALUES ('3', '2');
INSERT INTO EmployeeTraining (EmployeeId, TrainingProgramId) VALUES ('4', '1');
 
INSERT INTO ComputerEmployee (EmployeeId, ComputerId, AssignDate, UnassignDate) VALUES ('1', '1', '20120618 10:34:09 AM', '20190922 12:22:55 AM');
INSERT INTO ComputerEmployee (EmployeeId, ComputerId, AssignDate, UnassignDate) VALUES ('2', '2', '20130512 11:11:11 AM', '20190513 05:22:33 AM');
INSERT INTO ComputerEmployee (EmployeeId, ComputerId, AssignDate, UnassignDate) VALUES ('3', '3', '20120618 09:09:09 PM', '20190922 06:12:11 PM');
INSERT INTO ComputerEmployee (EmployeeId, ComputerId, AssignDate, UnassignDate) VALUES ('4', '4', '20140322 06:09:06 PM', '20190907 04:44:22 PM');
 
INSERT INTO PaymentType (AcctNumber, [Name], CustomerId, Archived) VALUES ('123', 'Credit Card', '1', 'false');
INSERT INTO PaymentType (AcctNumber, [Name], CustomerId, Archived) VALUES ('124', 'Check', '2', 'false');
INSERT INTO PaymentType (AcctNumber, [Name], CustomerId, Archived) VALUES ('125', 'Cash', '3', 'false');
INSERT INTO PaymentType (AcctNumber, [Name], CustomerId, Archived) VALUES ('126', 'Paypal', '4', 'false');
 
INSERT INTO Product (ProductTypeId, CustomerId, Price, Title, [Description], Quantity, Archived) VALUES ('1', '1', '300.00', 'TV', '42" LCD TV', '50', 'false');
INSERT INTO Product (ProductTypeId, CustomerId, Price, Title, [Description], Quantity, Archived) VALUES ('1', '2', '350.00', 'TV', '46" LCD TV', '50', 'false');
INSERT INTO Product (ProductTypeId, CustomerId, Price, Title, [Description], Quantity, Archived) VALUES ('2', '3', '150.00', 'Bicycle', 'Huffy 12 Speed', '50', 'false');
INSERT INTO Product (ProductTypeId, CustomerId, Price, Title, [Description], Quantity, Archived) VALUES ('1', '4', '500.00', 'Computer', 'Dell Computer', '50', 'false');
 
INSERT INTO [Order] (CustomerId, PaymentTypeId, Archived) VALUES ('1', '1', 'false');
INSERT INTO [Order] (CustomerId, PaymentTypeId, Archived) VALUES ('2', '2', 'false');
INSERT INTO [Order] (CustomerId, PaymentTypeId, Archived) VALUES ('3', '2', 'false');
INSERT INTO [Order] (CustomerId, PaymentTypeId, Archived) VALUES ('4', '3', 'false');
 
INSERT INTO OrderProduct (OrderId, ProductId) VALUES ('1', '1');
INSERT INTO OrderProduct (OrderId, ProductId) VALUES ('2', '2');
INSERT INTO OrderProduct (OrderId, ProductId) VALUES ('3', '2');
INSERT INTO OrderProduct (OrderId, ProductId) VALUES ('4', '1');