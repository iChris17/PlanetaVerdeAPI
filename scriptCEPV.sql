CREATE DATABASE PLANETAVERDEWEB
USE PLANETAVERDEWEB

CREATE TABLE CATEGORIA(ID_CATEGORIA INT NOT NULL IDENTITY(0,1),NB_CATEGORIA VARCHAR(50) NOT NULL,DE_CATEGORIA VARCHAR(200) DEFAULT '',TP_CATEGORIA VARCHAR(30),FH_REGISTRO DATETIME DEFAULT GETDATE(),US_REGISTRO VARCHAR (30) DEFAULT '',CONSTRAINT CHK_TP_CAT CHECK (TP_CATEGORIA='ARTICULO' OR TP_CATEGORIA='NOTICIA'),CONSTRAINT PK_CATEGORIA PRIMARY KEY (ID_CATEGORIA))

ALTER TABLE CATEGORIA ADD NB_CATEGORIA_HEADER VARCHAR(50) NOT NULL

CREATE TABLE NOTICIA(
ID_NOTICIA_HEADER VARCHAR(200) NOT NULL, 
ID_CATEGORIA INT,
NB_NOTICIA VARCHAR (200) NOT NULL,
DE_NOTICIA VARCHAR(MAX) NOT NULL,
VL_IMAGE VARCHAR(MAX) NOT NULL,
FH_REGISTRO DATETIME DEFAULT GETDATE(),
US_REGISTRO VARCHAR (30) DEFAULT '',
CONSTRAINT PK_NOTICIA PRIMARY KEY (ID_NOTICIA_HEADER)
)

CREATE TABLE NOTICIA_DETALLE(
ID_NOTICIA_HEADER VARCHAR(200) NOT NULL,
TX_NOTICIA VARCHAR(MAX) NOT NULL,
VL_IMAGE VARCHAR(MAX) NOT NULL,
FH_REGISTRO DATETIME DEFAULT GETDATE(),
US_REGISTRO VARCHAR (30) DEFAULT '',
CONSTRAINT PK_NOTICIA_DETALLE PRIMARY KEY (ID_NOTICIA_HEADER),
CONSTRAINT FK_NOTICIA_DETALLE FOREIGN KEY (ID_NOTICIA_HEADER) REFERENCES NOTICIA(ID_NOTICIA_HEADER)
)

INSERT INTO CATEGORIA ([NB_CATEGORIA],[DE_CATEGORIA],[TP_CATEGORIA],[FH_REGISTRO],[US_REGISTRO],[NB_CATEGORIA_HEADER]) VALUES ('RECIENTES','','NOTICIA',GETDATE(),'CACEVEDO','recientes-noticia')

INSERT INTO CATEGORIA ([NB_CATEGORIA],[DE_CATEGORIA],[TP_CATEGORIA],[FH_REGISTRO],[US_REGISTRO],[NB_CATEGORIA_HEADER]) 
VALUES ('COVID-19','','NOTICIA',GETDATE(),'CACEVEDO','covid-noti')

INSERT INTO CATEGORIA ([NB_CATEGORIA],[DE_CATEGORIA],[TP_CATEGORIA],[FH_REGISTRO],[US_REGISTRO],[NB_CATEGORIA_HEADER]) 
VALUES ('ACAD�MICAS','','NOTICIA',GETDATE(),'CACEVEDO','academica')

INSERT INTO CATEGORIA ([NB_CATEGORIA],[DE_CATEGORIA],[TP_CATEGORIA],[FH_REGISTRO],[US_REGISTRO],[NB_CATEGORIA_HEADER]) 
VALUES ('RECIENTES','','ARTICULO',GETDATE(),'CACEVEDO','recientes-art')

INSERT INTO CATEGORIA ([NB_CATEGORIA],[DE_CATEGORIA],[TP_CATEGORIA],[FH_REGISTRO],[US_REGISTRO],[NB_CATEGORIA_HEADER]) 
VALUES ('NOVEDADES','','ARTICULO',GETDATE(),'CACEVEDO','novedades')

INSERT INTO CATEGORIA ([NB_CATEGORIA],[DE_CATEGORIA],[TP_CATEGORIA],[FH_REGISTRO],[US_REGISTRO],[NB_CATEGORIA_HEADER]) 
VALUES ('SALUD','','ARTICULO',GETDATE(),'CACEVEDO','salud')

CREATE TABLE NOTICIA_CATEGORIA(
ID_CATEGORIA INT NOT NULL,
ID_NOTICIA_HEADER VARCHAR(200) NOT NULL,
FH_REGISTRO DATETIME DEFAULT GETDATE(),
US_REGISTRO VARCHAR(30) DEFAULT '',
CONSTRAINT PK_NOTICIA_CATEGORIA PRIMARY KEY (ID_CATEGORIA,ID_NOTICIA_HEADER),
CONSTRAINT FK_NOTICIACAT_CATEGORIA FOREIGN KEY (ID_CATEGORIA) REFERENCES CATEGORIA(ID_CATEGORIA),
CONSTRAINT FK_NOTICIACAT_NOTICIA FOREIGN KEY (ID_NOTICIA_HEADER) REFERENCES NOTICIA(ID_NOTICIA_HEADER),
)

ALTER TABLE NOTICIA_CATEGORIA ADD IN_PRINCIPAL BIT DEFAULT 0
ALTER TABLE NOTICIA_CATEGORIA ADD IN_ACTIVO BIT DEFAULT 1

create PROCEDURE SP_GETNOTICIA_BY_CATEGORIA
(
@NB_CATEGORIA_HEADER VARCHAR(50),
@VL_TOP INTEGER,
@IDNOTICIA VARCHAR(200)
)
AS
BEGIN 
IF(@VL_TOP=0)
BEGIN
IF(@IDNOTICIA!='')
BEGIN
SELECT N.* FROM NOTICIA N 
INNER JOIN NOTICIA_CATEGORIA NC
ON NC.ID_NOTICIA_HEADER=N.ID_NOTICIA_HEADER
INNER JOIN CATEGORIA C
ON C.ID_CATEGORIA=NC.ID_CATEGORIA
WHERE C.NB_CATEGORIA_HEADER=@NB_CATEGORIA_HEADER AND N.ID_NOTICIA_HEADER!=@IDNOTICIA
ORDER BY N.FH_REGISTRO DESC
END

ELSE
BEGIN
SELECT N.* FROM NOTICIA N 
INNER JOIN NOTICIA_CATEGORIA NC
ON NC.ID_NOTICIA_HEADER=N.ID_NOTICIA_HEADER
INNER JOIN CATEGORIA C
ON C.ID_CATEGORIA=NC.ID_CATEGORIA
WHERE C.NB_CATEGORIA_HEADER=@NB_CATEGORIA_HEADER
ORDER BY N.FH_REGISTRO DESC
END
END

ELSE
BEGIN
IF(@IDNOTICIA!='')
BEGIN
SELECT TOP (@VL_TOP) N.* FROM NOTICIA N 
INNER JOIN NOTICIA_CATEGORIA NC
ON NC.ID_NOTICIA_HEADER=N.ID_NOTICIA_HEADER
INNER JOIN CATEGORIA C
ON C.ID_CATEGORIA=NC.ID_CATEGORIA
WHERE C.NB_CATEGORIA_HEADER=@NB_CATEGORIA_HEADER AND NC.IN_ACTIVO=1 AND N.ID_NOTICIA_HEADER!=@IDNOTICIA
ORDER BY N.FH_REGISTRO DESC
END

ELSE
BEGIN
SELECT TOP (@VL_TOP) N.* FROM NOTICIA N 
INNER JOIN NOTICIA_CATEGORIA NC
ON NC.ID_NOTICIA_HEADER=N.ID_NOTICIA_HEADER
INNER JOIN CATEGORIA C
ON C.ID_CATEGORIA=NC.ID_CATEGORIA
WHERE C.NB_CATEGORIA_HEADER=@NB_CATEGORIA_HEADER AND NC.IN_ACTIVO=1
ORDER BY N.FH_REGISTRO DESC
END
END

END

SP_GETNOTICIA_BY_CATEGORIA 'recientes-noticias',0,'nicaragua-supera-5000-casos'

create PROCEDURE SP_GETCATEGORIA_BY_NBNOTICIA
(
@NB_NOTICIA_HEADER VARCHAR(200)
)
AS
BEGIN

SELECT C.* FROM CATEGORIA C
INNER JOIN NOTICIA_CATEGORIA NC
ON NC.ID_CATEGORIA=C.ID_CATEGORIA
INNER JOIN NOTICIA N
ON N.ID_NOTICIA_HEADER=NC.ID_NOTICIA_HEADER
WHERE N.ID_NOTICIA_HEADER=@NB_NOTICIA_HEADER AND NC.IN_PRINCIPAL=1 AND NC.IN_ACTIVO=1

END

SP_GETCATEGORIA_BY_NBNOTICIA 'medicos-alertan-auto-medicacion'


CREATE PROCEDURE SP_BUSCARNOTICIA(
@VLBUSQUEDA VARCHAR (200)
)
AS

BEGIN

 DECLARE @FilterTable TABLE (Data VARCHAR(512))

 INSERT INTO @FilterTable (Data)
 SELECT DISTINCT S.Data
 FROM fnSplit(' ', @VLBUSQUEDA) S -- busqueda por palabras

 SELECT DISTINCT
      T.*
 FROM
      NOTICIA T
      INNER JOIN @FilterTable F1 ON T.NB_NOTICIA LIKE '%' + F1.Data + '%'
      LEFT JOIN @FilterTable F2 ON T.NB_NOTICIA NOT LIKE '%' + F2.Data + '%'
 WHERE
      F2.Data IS NULL

END

SP_BUSCARNOTICIA 'COVID-19 nicaragua'

--------------------------------------------------------------------------

CREATE FUNCTION [dbo].[fnSplit] ( @sep CHAR(1), @str VARCHAR(512) )
 RETURNS TABLE AS
 RETURN (
           WITH pieces(pn, start, stop) AS (
           SELECT 1, 1, CHARINDEX(@sep, @str)
           UNION ALL
           SELECT pn + 1, stop + 1, CHARINDEX(@sep, @str, stop + 1)
           FROM pieces
           WHERE stop > 0
      )

      SELECT
           pn AS Id,
           SUBSTRING(@str, start, CASE WHEN stop > 0 THEN stop - start ELSE 512 END) AS Data
      FROM
           pieces
 )

 ------------------------------------------------------------------------
 
 CREATE PROCEDURE SP_GETNOTICIAS_PAGINATION(
 @CATEGORIA VARCHAR(50),
 @INI INT,
 @CANT INT
 )
 AS
 BEGIN

 SELECT N.* FROM NOTICIA N
 INNER JOIN NOTICIA_CATEGORIA NC ON NC.ID_NOTICIA_HEADER=N.ID_NOTICIA_HEADER
 INNER JOIN CATEGORIA C ON C.ID_CATEGORIA=NC.ID_CATEGORIA
 WHERE C.NB_CATEGORIA_HEADER=@CATEGORIA
 ORDER BY N.FH_REGISTRO DESC 
 OFFSET @INI ROWS FETCH NEXT @CANT ROWS ONLY

 END

 SP_GETNOTICIAS_PAGINATION 'recientes-noticias', 0,4

 -----------------------------------------------------------------------------------------

 CREATE TABLE USUARIOS(
 ID_USUARIO INT IDENTITY(0,1),
 NB_USUARIO VARCHAR(30) NOT NULL,
 VL_CONTRASEñA VARCHAR(30) NOT NULL,
 FH_REGISTRO DATETIME DEFAULT GETDATE(),
 CONSTRAINT PK_USUARIO PRIMARY KEY(ID_USUARIO),
 CONSTRAINT UN_USUARIO UNIQUE(NB_USUARIO)
 ) 

 INSERT INTO USUARIOS ([NB_USUARIO],[VL_CONTRASEñA]) VALUES ('admin','abc123')

 ------------------------------cambios 19/06/20 -----------------------------------------------

 CREATE PROCEDURE SP_ADD_NOTICIA 
 (
 @NOTICIA_HEADER VARCHAR(200),
 @NOMBRE_NOTICIA VARCHAR(200),
 @DE_NOTICIA VARCHAR(MAX),
 @VL_IMAGE VARCHAR(MAX),
 @TX_NOTICIA VARCHAR(MAX),
 @US_REGISTRO VARCHAR(30)
 )
 AS
 BEGIN

 INSERT INTO NOTICIA([ID_NOTICIA_HEADER],[NB_NOTICIA],[DE_NOTICIA],[VL_IMAGE],[US_REGISTRO])
 VALUES (@NOTICIA_HEADER,@NOMBRE_NOTICIA,@DE_NOTICIA,@VL_IMAGE,@US_REGISTRO)

 INSERT INTO NOTICIA_DETALLE ([ID_NOTICIA_HEADER],[TX_NOTICIA],[US_REGISTRO])
 VALUES (@NOTICIA_HEADER,@TX_NOTICIA,@US_REGISTRO)

 END

 CREATE TRIGGER TG_NOTICIA_CATEGORIA
 ON NOTICIA
 AFTER INSERT
 AS
 BEGIN

 DECLARE @NOTICIA_HEADER VARCHAR(200),@USUARIO VARCHAR(50),@CATEGORIA VARCHAR(200)

 SELECT @NOTICIA_HEADER=I.ID_NOTICIA_HEADER,@USUARIO=I.US_REGISTRO FROM inserted I

 SELECT @CATEGORIA=C.ID_CATEGORIA FROM CATEGORIA C WHERE C.NB_CATEGORIA_HEADER='recientes-noticias'

 INSERT INTO NOTICIA_CATEGORIA ([ID_CATEGORIA],[ID_NOTICIA_HEADER],[US_REGISTRO],[IN_PRINCIPAL])
 VALUES (@CATEGORIA,@NOTICIA_HEADER,@USUARIO,0)

 END

 ALTER TABLE NOTICIA_CATEGORIA ADD IN_ACTIVO BIT DEFAULT 1
 UPDATE NOTICIA_CATEGORIA SET IN_ACTIVO =1

 CREATE PROCEDURE SP_ADD_CATEGORIA_NOTICIA(
 @ACCION VARCHAR(30),
 @IDCAT INT,
 @NOTICIA VARCHAR(200),
 @USUARIO VARCHAR(30)
 )
 AS
 BEGIN

 IF(@ACCION='ADD')
 BEGIN
 INSERT INTO NOTICIA_CATEGORIA ([ID_CATEGORIA],[ID_NOTICIA_HEADER],[US_REGISTRO])
 VALUES (@IDCAT,@NOTICIA,@USUARIO)
 END

  IF(@ACCION='ADD_DEFAULT')
 BEGIN
 INSERT INTO NOTICIA_CATEGORIA ([ID_CATEGORIA],[ID_NOTICIA_HEADER],[US_REGISTRO],[IN_PRINCIPAL])
 VALUES (@IDCAT,@NOTICIA,@USUARIO,1)
 END

  IF(@ACCION='DELETE')
 BEGIN
 UPDATE NOTICIA_CATEGORIA 
 SET IN_ACTIVO=0
 WHERE ID_CATEGORIA=@IDCAT AND ID_NOTICIA_HEADER=@NOTICIA
 END

 END


 
ALTER PROCEDURE SP_GETNOTICIA_BY_CATEGORIA
(
@NB_CATEGORIA_HEADER VARCHAR(50),
@VL_TOP INTEGER
)
AS
BEGIN 
IF(@VL_TOP=0)BEGIN
SELECT N.* FROM NOTICIA N 
INNER JOIN NOTICIA_CATEGORIA NC
ON NC.ID_NOTICIA_HEADER=N.ID_NOTICIA_HEADER
INNER JOIN CATEGORIA C
ON C.ID_CATEGORIA=NC.ID_CATEGORIA
WHERE C.NB_CATEGORIA_HEADER=@NB_CATEGORIA_HEADER
ORDER BY N.FH_REGISTRO DESC
END
ELSE
BEGIN
SELECT TOP (@VL_TOP) N.* FROM NOTICIA N 
INNER JOIN NOTICIA_CATEGORIA NC
ON NC.ID_NOTICIA_HEADER=N.ID_NOTICIA_HEADER
INNER JOIN CATEGORIA C
ON C.ID_CATEGORIA=NC.ID_CATEGORIA
WHERE C.NB_CATEGORIA_HEADER=@NB_CATEGORIA_HEADER AND NC.IN_ACTIVO=1
ORDER BY N.FH_REGISTRO DESC
END
END


ALTER PROCEDURE SP_GETCATEGORIA_BY_NBNOTICIA
(
@NB_NOTICIA_HEADER VARCHAR(200)
)
AS
BEGIN

SELECT C.* FROM CATEGORIA C
INNER JOIN NOTICIA_CATEGORIA NC
ON NC.ID_CATEGORIA=C.ID_CATEGORIA
INNER JOIN NOTICIA N
ON N.ID_NOTICIA_HEADER=NC.ID_NOTICIA_HEADER
WHERE N.ID_NOTICIA_HEADER=@NB_NOTICIA_HEADER AND NC.IN_PRINCIPAL=1 AND NC.IN_ACTIVO=1

END


