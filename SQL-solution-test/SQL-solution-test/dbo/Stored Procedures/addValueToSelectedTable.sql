CREATE PROCEDURE addValueToSelectedTable (
@tableName VARCHAR(20),
@ID INT, 
@Nev VARCHAR(30), 
@Elhelyezkedes CHAR(20),
@evfolyam INT,
@Neme VARCHAR(20), 
@osztalyID INT,
@lakhely VARCHAR(20),
@szuldatum DATETIME,
@Azonosito VARCHAR(30),
@Name VARCHAR(20),
@TantargyID INT,
@Tanulo INT,
@jegy INT,
@datum DATETIME)
AS
BEGIN 
IF @tableName = 'Osztaly'
BEGIN
INSERT INTO Osztaly(
ID, 
Nev, 
Elhelyezkedes, 
evfolyam)
VALUES(
@ID,
@Nev,
@Elhelyezkedes,
@evfolyam)
END
ELSE IF @tableName = 'Tanulo'
BEGIN
INSERT INTO Tanulo(
ID, 
Nev, 
Neme, 
osztalyID,
lakhely,
szuldatum,
Azonosito) 
VALUES(
@ID, 
@Nev, 
@Neme, 
@osztalyID,
@lakhely,
@szuldatum,
@Azonosito) 
END
ELSE IF @tableName = 'Tantargy'
BEGIN
INSERT INTO Tantargy(
ID,
Name)
VALUES(
@ID,
@Name)
END
ELSE IF @tableName = 'Naplo'
BEGIN
INSERT INTO Naplo(
ID,
TantargyID,
Tanulo,
jegy,
datum)
VALUES(
@ID,
@TantargyID,
@Tanulo,
@jegy,
@datum)
END
END
