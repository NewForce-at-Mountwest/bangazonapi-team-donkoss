 
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
 