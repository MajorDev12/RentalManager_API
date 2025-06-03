-- ======================================
-- RENTAL MANAGEMENT SYSTEM DATABASE SCHEMA
-- ======================================

-- System Code Table
CREATE TABLE SystemCode (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Code VARCHAR(50) NOT NULL,
    Notes VARCHAR(100),
    CreatedOn DATETIME NOT NULL DEFAULT GETDATE(),
	CreatedBy INT,
	UpdatedBy INT
);

-- User Table
CREATE TABLE [User] (
    Id INT PRIMARY KEY IDENTITY(1,1),
    RoleId INT NOT NULL,
    PropertyId INT,
    FirstName VARCHAR(50) NOT NULL,
    LastName VARCHAR(50) NOT NULL,
    EmailAddress VARCHAR(50),
    MobileNumber VARCHAR(15) NOT NULL,
    AlternativeNumber VARCHAR(15),
    Gender INT,
    PasswordHash VARCHAR(256) NOT NULL,
    LastPasswordChange DATETIME,
    NationalId INT,
    ProfilePhotoUrl VARCHAR(256),
    IsActive BIT NOT NULL DEFAULT 0,
    UserStatus INT,
    CreatedBy INT,
    CreatedOn DATETIME NOT NULL DEFAULT GETDATE(),
    UpdatedOn DATETIME,
    UpdatedBy INT
);

-- System Code Item Table
CREATE TABLE SystemCodeItem (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    SystemCodeId INT NOT NULL,
    Item VARCHAR(50) NOT NULL,
    Notes VARCHAR(100),
    CreatedOn DATETIME NOT NULL DEFAULT GETDATE(),
    CreatedBy INT,
    UpdatedOn DATETIME,
    UpdatedBy INT
);

-- Property Table
CREATE TABLE Property (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name VARCHAR(50) NOT NULL,
    Country VARCHAR(50) NOT NULL,
    County VARCHAR(50) NOT NULL,
    Area VARCHAR(50) NOT NULL,
    PhysicalAddress VARCHAR(50) NOT NULL,
    Longitude DECIMAL(9,6),
    Latitude DECIMAL(9,6),
    Floor INT NOT NULL,
    Notes VARCHAR(100),
    EmailAddress VARCHAR(50) NOT NULL,
    MobileNumber VARCHAR(15) NOT NULL,
    CreatedBy INT,
    CreatedOn DATETIME NOT NULL DEFAULT GETDATE(),
    UpdatedOn DATETIME,
    UpdatedBy INT
);

-- Role Table
CREATE TABLE Role (
    Id INT PRIMARY KEY IDENTITY(1,1),
    PropertyId INT NOT NULL,
    Name VARCHAR(20) NOT NULL,
    IsEnabled BIT NOT NULL DEFAULT 1,
    CreatedBy INT,
    CreatedOn DATETIME NOT NULL DEFAULT GETDATE(),
    UpdatedOn DATETIME,
    UpdatedBy INT
);

-- Unit Type Table
CREATE TABLE UnitType (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name VARCHAR(20) NOT NULL,
    Notes VARCHAR(100),
    Amount MONEY NOT NULL,
    CreatedBy INT,
    CreatedOn DATETIME NOT NULL DEFAULT GETDATE(),
    UpdatedOn DATETIME,
    UpdatedBy INT
);

-- Unit Table
CREATE TABLE Unit (
    Id INT PRIMARY KEY IDENTITY(1,1),
    UnitTypeId INT NOT NULL,
    PropertyId INT NOT NULL,
    Name VARCHAR(50) NOT NULL,
    Status VARCHAR(20),
    Notes VARCHAR(100),
    CreatedBy INT,
    CreatedOn DATETIME NOT NULL DEFAULT GETDATE(),
    UpdatedOn DATETIME,
    UpdatedBy INT
);

-- Utility Bill Table
CREATE TABLE UtilityBill (
    Id INT PRIMARY KEY IDENTITY(1,1),
    PropertyId INT NOT NULL,
    Name VARCHAR(20) NOT NULL,
    Amount MONEY NOT NULL,
    CreatedBy INT,
    CreatedOn DATETIME NOT NULL DEFAULT GETDATE(),
    UpdatedOn DATETIME,
    UpdatedBy INT
);

-- UserLogin Table
CREATE TABLE UserLogin (
    Id INT PRIMARY KEY IDENTITY(1,1),
    UserId INT NOT NULL,
    FailedAttempts INT DEFAULT 0,
    Succeeded BIT NOT NULL,
    IpAddress VARCHAR(20),
    UserToken VARCHAR(20),
    TokenExpiry DATETIME,
    CreatedOn DATETIME NOT NULL DEFAULT GETDATE(),
    IsActive BIT NOT NULL DEFAULT 1
);

-- Tenant Table
CREATE TABLE Tenant (
    Id INT PRIMARY KEY IDENTITY(1,1),
    UserId INT NOT NULL,
    UnitId INT NOT NULL,
    FullName VARCHAR(50) NOT NULL,
    EmailAddress VARCHAR(50),
    MobileNumber VARCHAR(20) NOT NULL,
    Status INT
);

-- Landlord Table
CREATE TABLE Landlord (
    Id INT PRIMARY KEY IDENTITY(1,1),
    UserId INT NOT NULL,
	PropertyId INT NOT NULL,
    FullName VARCHAR(100) NOT NULL,
    MobileNumber VARCHAR(15) NOT NULL,
    Email VARCHAR(50)
);

-- Payment Table
CREATE TABLE Payment (
    Id INT PRIMARY KEY IDENTITY(1,1),
    TenantId INT NOT NULL,
    UtilityBillId INT,
    AmountPaid DECIMAL(10,2) NOT NULL,
    PaymentDate DATETIME NOT NULL,
    PaymentMethod INT,
    TransactionCode VARCHAR(100),
    Notes TEXT NULL,
    CreatedBy INT,
    CreatedOn DATETIME NOT NULL DEFAULT GETDATE(),
    UpdatedOn DATETIME,
    UpdatedBy INT
);

-- System Log Table
CREATE TABLE SystemLog (
    Id INT PRIMARY KEY IDENTITY(1,1),
    UserId INT NOT NULL,
    LogLevelId INT NOT NULL,
    Action VARCHAR(50),
    Notes VARCHAR(50),
    IpAddress VARCHAR(50),
    CreatedOn DATETIME NOT NULL DEFAULT GETDATE(),
    CreatedBy INT,
    UpdatedOn DATETIME,
    UpdatedBy INT
);

-- ===============================
-- Foreign Key Constraints (No Cascading Deletes)
-- ===============================

-- Foreign Keys with NO ACTION (safe option)
ALTER TABLE SystemCode ADD
    FOREIGN KEY (CreatedBy) REFERENCES [User](Id),
	FOREIGN KEY (UpdatedBy) REFERENCES [User](Id);


ALTER TABLE SystemCodeItem ADD
    FOREIGN KEY (SystemCodeId) REFERENCES SystemCode(Id),
    FOREIGN KEY (CreatedBy) REFERENCES [User](Id),
    FOREIGN KEY (UpdatedBy) REFERENCES [User](Id);

ALTER TABLE Property ADD
    FOREIGN KEY (CreatedBy) REFERENCES [User](Id),
    FOREIGN KEY (UpdatedBy) REFERENCES [User](Id);

ALTER TABLE Role ADD
    FOREIGN KEY (PropertyId) REFERENCES Property(Id),
    FOREIGN KEY (CreatedBy) REFERENCES [User](Id),
    FOREIGN KEY (UpdatedBy) REFERENCES [User](Id);

ALTER TABLE [User] ADD
    FOREIGN KEY (RoleId) REFERENCES Role(Id),
    FOREIGN KEY (PropertyId) REFERENCES Property(Id),
    FOREIGN KEY (Gender) REFERENCES SystemCodeItem(Id),
    FOREIGN KEY (UserStatus) REFERENCES SystemCodeItem(Id),
    FOREIGN KEY (CreatedBy) REFERENCES [User](Id),
    FOREIGN KEY (UpdatedBy) REFERENCES [User](Id);

ALTER TABLE Unit ADD
    FOREIGN KEY (UnitTypeId) REFERENCES UnitType(Id),
    FOREIGN KEY (PropertyId) REFERENCES Property(Id),
    FOREIGN KEY (CreatedBy) REFERENCES [User](Id),
    FOREIGN KEY (UpdatedBy) REFERENCES [User](Id);

ALTER TABLE UnitType ADD
    FOREIGN KEY (CreatedBy) REFERENCES [User](Id),
    FOREIGN KEY (UpdatedBy) REFERENCES [User](Id);

ALTER TABLE UtilityBill ADD
    FOREIGN KEY (PropertyId) REFERENCES Property(Id),
    FOREIGN KEY (CreatedBy) REFERENCES [User](Id),
    FOREIGN KEY (UpdatedBy) REFERENCES [User](Id);

ALTER TABLE UserLogin ADD
    FOREIGN KEY (UserId) REFERENCES [User](Id);

ALTER TABLE Tenant ADD
    FOREIGN KEY (UserId) REFERENCES [User](Id),
    FOREIGN KEY (UnitId) REFERENCES Unit(Id),
    FOREIGN KEY (Status) REFERENCES SystemCodeItem(Id);

ALTER TABLE Landlord ADD
    FOREIGN KEY (UserId) REFERENCES [User](Id),
	FOREIGN KEY (PropertyId) REFERENCES Property(Id);

ALTER TABLE Payment ADD
    FOREIGN KEY (TenantId) REFERENCES Tenant(Id),
    FOREIGN KEY (UtilityBillId) REFERENCES UtilityBill(Id),
    FOREIGN KEY (PaymentMethod) REFERENCES SystemCodeItem(Id),
    FOREIGN KEY (CreatedBy) REFERENCES [User](Id),
    FOREIGN KEY (UpdatedBy) REFERENCES [User](Id);

ALTER TABLE SystemLog ADD
    FOREIGN KEY (UserId) REFERENCES [User](Id),
    FOREIGN KEY (LogLevelId) REFERENCES SystemCodeItem(Id),
    FOREIGN KEY (CreatedBy) REFERENCES [User](Id),
    FOREIGN KEY (UpdatedBy) REFERENCES [User](Id);
