-- Create the Person table
CREATE TABLE Person (
    PersonID INT PRIMARY KEY,
    NationalID NVARCHAR(500) NOT NULL,
    FirstName NVARCHAR(500) NOT NULL,
    LastName NVARCHAR(500) NOT NULL,
    Address NVARCHAR(500)
);

-- Create the PaymentMethod table
CREATE TABLE PaymentMethod (
    PaymentID INT PRIMARY KEY,
    PaymentType NVARCHAR(500) NOT NULL
);

-- Create the Account table (linked to Person)
CREATE TABLE Account (
    AccountEmail NVARCHAR(500) PRIMARY KEY,
    Password NVARCHAR(500) NOT NULL,
    PersonID INT,
    FOREIGN KEY (PersonID) REFERENCES Person(PersonID)
);

-- Create the Plans table
CREATE TABLE Plans (
    PlanID INT PRIMARY KEY,
    Revision NVARCHAR(500),
    PlanPrice FLOAT,
    PlanNotes NVARCHAR(500),
    PlanDescription NVARCHAR(500),
    PlanType NVARCHAR(500),
    PlanTitle NVARCHAR(500),
    PlanLocation NVARCHAR(500)
);

-- Create the PlanDetails table (linked to Plans)
CREATE TABLE PlanDetails (
    PlanDetailID INT PRIMARY KEY,
    PlanID INT,
    MotionGraphicType NVARCHAR(500),
    VideoLength INT,
    NumberOfShots INT,
    VoiceOver BIT,
    PlatformManaged BIT,
    PostPerMonth INT,
    DesignType NVARCHAR(500),
    NumberOfRevision INT,
    NumberOfDesign INT,
    FOREIGN KEY (PlanID) REFERENCES Plans(PlanID)
);

-- Create the Form table
CREATE TABLE Form (
    FormID INT PRIMARY KEY,
    FirstName NVARCHAR(500),
    LastName NVARCHAR(500),
    Email NVARCHAR(500),
    Notes NVARCHAR(500)
);

-- Create the CourseForm table (linked to Form)
CREATE TABLE CourseForm (
    CourseFormID INT PRIMARY KEY,
    CourseStatus NVARCHAR(500),
    CourseEnrollmentDate DATE,
    FormID INT,
    FOREIGN KEY (FormID) REFERENCES Form(FormID)
);

-- Create the SEO_SEM Form table (linked to Form)
CREATE TABLE SEO_SEM_Form (
    FormID INT PRIMARY KEY,
    Website NVARCHAR(500),
    CurrentSeoStatus NVARCHAR(500),
    MonthlyBudget FLOAT,
    SEOStartDate DATE,
    FOREIGN KEY (FormID) REFERENCES Form(FormID)
);

-- Create the Event Form table (linked to Form)
CREATE TABLE EventForm (
    EventFormID INT PRIMARY KEY,
    RegistrationDate DATE,
    EventStatus NVARCHAR(500),
    FormID INT,
    FOREIGN KEY (FormID) REFERENCES Form(FormID)
);

-- Create the Event table
CREATE TABLE Event (
    EventID INT PRIMARY KEY,
    EventType NVARCHAR(500),
    EventDescription NVARCHAR(500),
    EventDate DATE,
    EventStartTime TIME,
    EventEndTime TIME,
    EventCapacity INT,
    EventTitle NVARCHAR(500),
    EventLocation NVARCHAR(500),
    EventPrice FLOAT,
    EventInstructor NVARCHAR(500)
);

-- Create the Course table
CREATE TABLE Course (
    CourseID INT PRIMARY KEY,
    CourseStartDate DATE,
    CoursePrice FLOAT,
    CourseInstructor NVARCHAR(500),
    CourseDuration FLOAT,
    CourseLocation NVARCHAR(500),
    CourseDescription NVARCHAR(500),
    CourseTitle NVARCHAR(500)
);

-- Create the Job table
CREATE TABLE Job (
    JobID INT PRIMARY KEY,
    JobTitle NVARCHAR(500),
    JobDescription NVARCHAR(500),
    Requirements NVARCHAR(500)
);

-- Create the Development table
CREATE TABLE Development (
    ProjectName NVARCHAR(500),
    ProjectType NVARCHAR(500),
    Responsive BIT,
    Platform NVARCHAR(500),
    Budget INT,
    Timeline DATE
);

-- Create the PhoneNumber table (linked to Person)
CREATE TABLE PhoneNumber (
    PhoneID INT PRIMARY KEY,
    AreaCode INT,
    PhoneNumber NVARCHAR(500),
    PersonID INT,
    FOREIGN KEY (PersonID) REFERENCES Person(PersonID)
);

-- Create the Departments table
CREATE TABLE Departments (
    DepartmentID INT PRIMARY KEY,
    DepartmentTitle NVARCHAR(500) NOT NULL,
    DepartmentDescription NVARCHAR(500),
    DepartmentImage NVARCHAR(500)
);

-- Create the Service table (linked to Department)
CREATE TABLE Service (
    ServiceID INT PRIMARY KEY,
    ServiceTitle NVARCHAR(500) NOT NULL,
    ServiceDescription NVARCHAR(500),
    ServiceImage NVARCHAR(500),
    DepartmentID INT,
    FOREIGN KEY (DepartmentID) REFERENCES Departments(DepartmentID)
);

-- Create the Transactions table
CREATE TABLE Transactions (
    TransactionID INT PRIMARY KEY,
    TransactionDate DATETIME NOT NULL
);

-- Create the relationships between Plans and Transactions (Transaction may belong to a plan or a form)
CREATE TABLE PlanTransactions (
    TransactionID INT,
    PlanID INT,
    FOREIGN KEY (TransactionID) REFERENCES Transactions(TransactionID),
    FOREIGN KEY (PlanID) REFERENCES Plans(PlanID)
);

-- Create the relationships between Forms and Transactions (Transaction may belong to a form)
CREATE TABLE FormTransactions (
    TransactionID INT,
    FormID INT,
    FOREIGN KEY (TransactionID) REFERENCES Transactions(TransactionID),
    FOREIGN KEY (FormID) REFERENCES Form(FormID)
);

