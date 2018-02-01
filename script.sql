CREATE TABLE Users
(
  name         VARCHAR(100)     NOT NULL,
  lastname     VARCHAR(100)     NOT NULL,
  age          INT              NOT NULL,
  neighborhood VARCHAR(100)     NOT NULL,
  gender       VARCHAR(100)     NOT NULL,
  mail         VARCHAR(100)     NOT NULL,
  password     VARCHAR(100)     NOT NULL,
  state        INT              NOT NULL,
  salt         VARCHAR(100)     NOT NULL,
  type         VARCHAR(100)     NOT NULL,
  date         DATETIME DEFAULT getdate(),
  id           UNIQUEIDENTIFIER NOT NULL
    CONSTRAINT Users_id_pk
    PRIMARY KEY
)
GO

CREATE TABLE Tokens
(
  token   VARCHAR(100)     NOT NULL,
  date   DATETIME DEFAULT getdate(),
  type   VARCHAR(100)     NOT NULL,
  id     UNIQUEIDENTIFIER NOT NULL
    CONSTRAINT Tokens_id_pk
    PRIMARY KEY,
  idUser UNIQUEIDENTIFIER NOT NULL
    CONSTRAINT Tokens_Users_id_fk
    REFERENCES Users
)
GO

CREATE TABLE Products
(
  name       VARCHAR(100)     NOT NULL,
  barcode    VARCHAR(100)     NOT NULL,
  state      INT              NOT NULL,
  image_name VARCHAR(100)     NOT NULL,
  route      VARCHAR(100)     NOT NULL,
  date       DATETIME DEFAULT getdate(),
  id         UNIQUEIDENTIFIER NOT NULL
    CONSTRAINT Products_id_pk
    PRIMARY KEY
)
GO

CREATE TABLE Searches
(
  date   DATETIME DEFAULT getdate(),
  id     UNIQUEIDENTIFIER NOT NULL
    CONSTRAINT Searches_id_pk
    PRIMARY KEY,
  idUser UNIQUEIDENTIFIER NOT NULL
    CONSTRAINT Searches_Users_id_fk
    REFERENCES Users,
  idProduct UNIQUEIDENTIFIER NOT NULL
    CONSTRAINT Searches_Products_id_fk
    REFERENCES Products
)
GO

CREATE TABLE Notifications
(
  date    DATETIME DEFAULT getdate(),
  id      UNIQUEIDENTIFIER NOT NULL
    CONSTRAINT Notifications_id_pk
    PRIMARY KEY,
  idUser  UNIQUEIDENTIFIER NOT NULL
    CONSTRAINT Notifications_Users_id_fk
    REFERENCES Users,
  idPrice UNIQUEIDENTIFIER NOT NULL
)
GO

CREATE TABLE Stores
(
  name      VARCHAR(100)     NOT NULL,
  address   VARCHAR(100)     NOT NULL,
  latitude  VARCHAR(100)     NOT NULL,
  longitude VARCHAR(100)     NOT NULL,
  state     INT              NOT NULL,
  date      DATETIME DEFAULT getdate(),
  id        UNIQUEIDENTIFIER NOT NULL
    CONSTRAINT Stores_id_pk
    PRIMARY KEY
)
GO

CREATE TABLE Prices
(
  price     DECIMAL(18)      NOT NULL,
  date      DATETIME DEFAULT getdate(),
  report    VARCHAR(100)     NOT NULL,
  id        UNIQUEIDENTIFIER NOT NULL
    CONSTRAINT Prices_id_pk
    PRIMARY KEY,
  idProduct UNIQUEIDENTIFIER NOT NULL
    CONSTRAINT Prices_Products_id_fk
    REFERENCES Products,
  idStore   UNIQUEIDENTIFIER NOT NULL
    CONSTRAINT Prices_Stores_id_fk
    REFERENCES Stores
)
GO

ALTER TABLE Notifications
  ADD CONSTRAINT Notifications_Prices_id_fk
FOREIGN KEY (idPrice) REFERENCES Prices
GO

