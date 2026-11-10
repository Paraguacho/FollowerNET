DROP PROCEDURE IF EXISTS `POMELO_BEFORE_DROP_PRIMARY_KEY`;
DELIMITER //
CREATE PROCEDURE `POMELO_BEFORE_DROP_PRIMARY_KEY`(IN `SCHEMA_NAME_ARGUMENT` VARCHAR(255), IN `TABLE_NAME_ARGUMENT` VARCHAR(255))
BEGIN
	DECLARE HAS_AUTO_INCREMENT_ID TINYINT(1);
	DECLARE PRIMARY_KEY_COLUMN_NAME VARCHAR(255);
	DECLARE PRIMARY_KEY_TYPE VARCHAR(255);
	DECLARE SQL_EXP VARCHAR(1000);
	SELECT COUNT(*)
		INTO HAS_AUTO_INCREMENT_ID
		FROM `information_schema`.`COLUMNS`
		WHERE `TABLE_SCHEMA` = (SELECT IFNULL(SCHEMA_NAME_ARGUMENT, SCHEMA()))
			AND `TABLE_NAME` = TABLE_NAME_ARGUMENT
			AND `Extra` = 'auto_increment'
			AND `COLUMN_KEY` = 'PRI'
			LIMIT 1;
	IF HAS_AUTO_INCREMENT_ID THEN
		SELECT `COLUMN_TYPE`
			INTO PRIMARY_KEY_TYPE
			FROM `information_schema`.`COLUMNS`
			WHERE `TABLE_SCHEMA` = (SELECT IFNULL(SCHEMA_NAME_ARGUMENT, SCHEMA()))
				AND `TABLE_NAME` = TABLE_NAME_ARGUMENT
				AND `COLUMN_KEY` = 'PRI'
			LIMIT 1;
		SELECT `COLUMN_NAME`
			INTO PRIMARY_KEY_COLUMN_NAME
			FROM `information_schema`.`COLUMNS`
			WHERE `TABLE_SCHEMA` = (SELECT IFNULL(SCHEMA_NAME_ARGUMENT, SCHEMA()))
				AND `TABLE_NAME` = TABLE_NAME_ARGUMENT
				AND `COLUMN_KEY` = 'PRI'
			LIMIT 1;
		SET SQL_EXP = CONCAT('ALTER TABLE `', (SELECT IFNULL(SCHEMA_NAME_ARGUMENT, SCHEMA())), '`.`', TABLE_NAME_ARGUMENT, '` MODIFY COLUMN `', PRIMARY_KEY_COLUMN_NAME, '` ', PRIMARY_KEY_TYPE, ' NOT NULL;');
		SET @SQL_EXP = SQL_EXP;
		PREPARE SQL_EXP_EXECUTE FROM @SQL_EXP;
		EXECUTE SQL_EXP_EXECUTE;
		DEALLOCATE PREPARE SQL_EXP_EXECUTE;
	END IF;
END //
DELIMITER ;

DROP PROCEDURE IF EXISTS `POMELO_AFTER_ADD_PRIMARY_KEY`;
DELIMITER //
CREATE PROCEDURE `POMELO_AFTER_ADD_PRIMARY_KEY`(IN `SCHEMA_NAME_ARGUMENT` VARCHAR(255), IN `TABLE_NAME_ARGUMENT` VARCHAR(255), IN `COLUMN_NAME_ARGUMENT` VARCHAR(255))
BEGIN
	DECLARE HAS_AUTO_INCREMENT_ID INT(11);
	DECLARE PRIMARY_KEY_COLUMN_NAME VARCHAR(255);
	DECLARE PRIMARY_KEY_TYPE VARCHAR(255);
	DECLARE SQL_EXP VARCHAR(1000);
	SELECT COUNT(*)
		INTO HAS_AUTO_INCREMENT_ID
		FROM `information_schema`.`COLUMNS`
		WHERE `TABLE_SCHEMA` = (SELECT IFNULL(SCHEMA_NAME_ARGUMENT, SCHEMA()))
			AND `TABLE_NAME` = TABLE_NAME_ARGUMENT
			AND `COLUMN_NAME` = COLUMN_NAME_ARGUMENT
			AND `COLUMN_TYPE` LIKE '%int%'
			AND `COLUMN_KEY` = 'PRI';
	IF HAS_AUTO_INCREMENT_ID THEN
		SELECT `COLUMN_TYPE`
			INTO PRIMARY_KEY_TYPE
			FROM `information_schema`.`COLUMNS`
			WHERE `TABLE_SCHEMA` = (SELECT IFNULL(SCHEMA_NAME_ARGUMENT, SCHEMA()))
				AND `TABLE_NAME` = TABLE_NAME_ARGUMENT
				AND `COLUMN_NAME` = COLUMN_NAME_ARGUMENT
				AND `COLUMN_TYPE` LIKE '%int%'
				AND `COLUMN_KEY` = 'PRI';
		SELECT `COLUMN_NAME`
			INTO PRIMARY_KEY_COLUMN_NAME
			FROM `information_schema`.`COLUMNS`
			WHERE `TABLE_SCHEMA` = (SELECT IFNULL(SCHEMA_NAME_ARGUMENT, SCHEMA()))
				AND `TABLE_NAME` = TABLE_NAME_ARGUMENT
				AND `COLUMN_NAME` = COLUMN_NAME_ARGUMENT
				AND `COLUMN_TYPE` LIKE '%int%'
				AND `COLUMN_KEY` = 'PRI';
		SET SQL_EXP = CONCAT('ALTER TABLE `', (SELECT IFNULL(SCHEMA_NAME_ARGUMENT, SCHEMA())), '`.`', TABLE_NAME_ARGUMENT, '` MODIFY COLUMN `', PRIMARY_KEY_COLUMN_NAME, '` ', PRIMARY_KEY_TYPE, ' NOT NULL AUTO_INCREMENT;');
		SET @SQL_EXP = SQL_EXP;
		PREPARE SQL_EXP_EXECUTE FROM @SQL_EXP;
		EXECUTE SQL_EXP_EXECUTE;
		DEALLOCATE PREPARE SQL_EXP_EXECUTE;
	END IF;
END //
DELIMITER ;

CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `ProductVersion` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
) CHARACTER SET=utf8mb4;

START TRANSACTION;
DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261030091206_InicialSeguridadYRutasSaaS') THEN

    ALTER DATABASE CHARACTER SET utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261030091206_InicialSeguridadYRutasSaaS') THEN

    CREATE TABLE `Programas_Educativos` (
        `IdPrograma` int NOT NULL AUTO_INCREMENT,
        `NombrePrograma` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
        CONSTRAINT `PK_Programas_Educativos` PRIMARY KEY (`IdPrograma`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261030091206_InicialSeguridadYRutasSaaS') THEN

    CREATE TABLE `Roles` (
        `IdRol` int NOT NULL AUTO_INCREMENT,
        `NombreRol` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
        CONSTRAINT `PK_Roles` PRIMARY KEY (`IdRol`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261030091206_InicialSeguridadYRutasSaaS') THEN

    CREATE TABLE `Secciones_Personalizadas` (
        `IdSeccion` int NOT NULL AUTO_INCREMENT,
        `IdPrograma` int NOT NULL,
        `TituloSeccion` varchar(160) CHARACTER SET utf8mb4 NOT NULL,
        `OrdenAparicion` int NOT NULL,
        CONSTRAINT `PK_Secciones_Personalizadas` PRIMARY KEY (`IdSeccion`),
        CONSTRAINT `FK_Secciones_Personalizadas_Programas_Educativos_IdPrograma` FOREIGN KEY (`IdPrograma`) REFERENCES `Programas_Educativos` (`IdPrograma`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261030091206_InicialSeguridadYRutasSaaS') THEN

    CREATE TABLE `Usuarios` (
        `IdUsuario` int NOT NULL AUTO_INCREMENT,
        `Correo` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
        `ContrasenaHash` varchar(265) CHARACTER SET utf8mb4 NOT NULL,
        `IdRol` int NOT NULL,
        CONSTRAINT `PK_Usuarios` PRIMARY KEY (`IdUsuario`),
        CONSTRAINT `FK_Usuarios_Roles_IdRol` FOREIGN KEY (`IdRol`) REFERENCES `Roles` (`IdRol`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261030091206_InicialSeguridadYRutasSaaS') THEN

    CREATE TABLE `Campos_Personalizados` (
        `IdCampo` int NOT NULL AUTO_INCREMENT,
        `IdSeccion` int NOT NULL,
        `NombreEtiqueta` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
        `TipoDato` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
        `EsObligatorio` tinyint(1) NOT NULL,
        `Orden` int NOT NULL,
        CONSTRAINT `PK_Campos_Personalizados` PRIMARY KEY (`IdCampo`),
        CONSTRAINT `FK_Campos_Personalizados_Secciones_Personalizadas_IdSeccion` FOREIGN KEY (`IdSeccion`) REFERENCES `Secciones_Personalizadas` (`IdSeccion`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261030091206_InicialSeguridadYRutasSaaS') THEN

    CREATE TABLE `Egresados` (
        `IdEgresados` int NOT NULL AUTO_INCREMENT,
        `IdUsuario` int NOT NULL,
        `IdPrograma` int NOT NULL,
        `EstadoSolicitud` varchar(20) CHARACTER SET utf8mb4 NOT NULL,
        `PrivacidadPerfil` varchar(20) CHARACTER SET utf8mb4 NOT NULL,
        `Nombres` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
        `ApellidoPaterno` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
        `ApellidoMaterno` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
        `Telefono` varchar(20) CHARACTER SET utf8mb4 NULL,
        `FechaNacimiento` datetime(6) NULL,
        `FechaEgreso` datetime(6) NULL,
        `Ciudad` varchar(100) CHARACTER SET utf8mb4 NULL,
        `Estado` varchar(100) CHARACTER SET utf8mb4 NULL,
        `Pais` varchar(100) CHARACTER SET utf8mb4 NULL,
        `FotoPerfilUrl` varchar(255) CHARACTER SET utf8mb4 NULL,
        `AutorizaDirectorio` tinyint(1) NOT NULL,
        `FechaSolicitud` datetime(6) NULL,
        `FechaAceptacionPrivacidad` datetime(6) NOT NULL,
        `FechaResolucion` datetime(6) NULL,
        `IdUsuarioResolvio` int NULL,
        `FechaUltimaActualizacion` datetime(6) NOT NULL,
        CONSTRAINT `PK_Egresados` PRIMARY KEY (`IdEgresados`),
        CONSTRAINT `FK_Egresados_Programas_Educativos_IdPrograma` FOREIGN KEY (`IdPrograma`) REFERENCES `Programas_Educativos` (`IdPrograma`) ON DELETE CASCADE,
        CONSTRAINT `FK_Egresados_Usuarios_IdUsuario` FOREIGN KEY (`IdUsuario`) REFERENCES `Usuarios` (`IdUsuario`) ON DELETE CASCADE,
        CONSTRAINT `FK_Egresados_Usuarios_IdUsuarioResolvio` FOREIGN KEY (`IdUsuarioResolvio`) REFERENCES `Usuarios` (`IdUsuario`) ON DELETE RESTRICT
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261030091206_InicialSeguridadYRutasSaaS') THEN

    CREATE TABLE `Programas_Responsables` (
        `IdProgramaResponsable` int NOT NULL AUTO_INCREMENT,
        `IdPrograma` int NOT NULL,
        `IdUsuario` int NOT NULL,
        `FechaAsignacion` datetime(6) NOT NULL,
        CONSTRAINT `PK_Programas_Responsables` PRIMARY KEY (`IdProgramaResponsable`),
        CONSTRAINT `FK_Programas_Responsables_Programas_Educativos_IdPrograma` FOREIGN KEY (`IdPrograma`) REFERENCES `Programas_Educativos` (`IdPrograma`) ON DELETE CASCADE,
        CONSTRAINT `FK_Programas_Responsables_Usuarios_IdUsuario` FOREIGN KEY (`IdUsuario`) REFERENCES `Usuarios` (`IdUsuario`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261030091206_InicialSeguridadYRutasSaaS') THEN

    CREATE TABLE `Opciones_Catalogo_Campo` (
        `IdOpcion` int NOT NULL AUTO_INCREMENT,
        `IdCampo` int NOT NULL,
        `ValorOpcion` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
        `Orden` int NOT NULL,
        CONSTRAINT `PK_Opciones_Catalogo_Campo` PRIMARY KEY (`IdOpcion`),
        CONSTRAINT `FK_Opciones_Catalogo_Campo_Campos_Personalizados_IdCampo` FOREIGN KEY (`IdCampo`) REFERENCES `Campos_Personalizados` (`IdCampo`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261030091206_InicialSeguridadYRutasSaaS') THEN

    CREATE TABLE `Valores_Campo_Egresados` (
        `IdValor` int NOT NULL AUTO_INCREMENT,
        `IdEgresado` int NOT NULL,
        `IdCampo` int NOT NULL,
        `ValorCapturado` longtext CHARACTER SET utf8mb4 NULL,
        CONSTRAINT `PK_Valores_Campo_Egresados` PRIMARY KEY (`IdValor`),
        CONSTRAINT `FK_Valores_Campo_Egresados_Campos_Personalizados_IdCampo` FOREIGN KEY (`IdCampo`) REFERENCES `Campos_Personalizados` (`IdCampo`) ON DELETE CASCADE,
        CONSTRAINT `FK_Valores_Campo_Egresados_Egresados_IdEgresado` FOREIGN KEY (`IdEgresado`) REFERENCES `Egresados` (`IdEgresados`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261030091206_InicialSeguridadYRutasSaaS') THEN

    CREATE INDEX `IX_Campos_Personalizados_IdSeccion` ON `Campos_Personalizados` (`IdSeccion`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261030091206_InicialSeguridadYRutasSaaS') THEN

    CREATE INDEX `IX_Egresados_IdPrograma` ON `Egresados` (`IdPrograma`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261030091206_InicialSeguridadYRutasSaaS') THEN

    CREATE UNIQUE INDEX `IX_Egresados_IdUsuario` ON `Egresados` (`IdUsuario`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261030091206_InicialSeguridadYRutasSaaS') THEN

    CREATE INDEX `IX_Egresados_IdUsuarioResolvio` ON `Egresados` (`IdUsuarioResolvio`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261030091206_InicialSeguridadYRutasSaaS') THEN

    CREATE INDEX `IX_Opciones_Catalogo_Campo_IdCampo` ON `Opciones_Catalogo_Campo` (`IdCampo`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261030091206_InicialSeguridadYRutasSaaS') THEN

    CREATE UNIQUE INDEX `IX_Programas_Responsables_IdPrograma_IdUsuario` ON `Programas_Responsables` (`IdPrograma`, `IdUsuario`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261030091206_InicialSeguridadYRutasSaaS') THEN

    CREATE INDEX `IX_Programas_Responsables_IdUsuario` ON `Programas_Responsables` (`IdUsuario`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261030091206_InicialSeguridadYRutasSaaS') THEN

    CREATE INDEX `IX_Secciones_Personalizadas_IdPrograma` ON `Secciones_Personalizadas` (`IdPrograma`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261030091206_InicialSeguridadYRutasSaaS') THEN

    CREATE UNIQUE INDEX `IX_Usuarios_Correo` ON `Usuarios` (`Correo`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261030091206_InicialSeguridadYRutasSaaS') THEN

    CREATE INDEX `IX_Usuarios_IdRol` ON `Usuarios` (`IdRol`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261030091206_InicialSeguridadYRutasSaaS') THEN

    CREATE INDEX `IX_Valores_Campo_Egresados_IdCampo` ON `Valores_Campo_Egresados` (`IdCampo`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261030091206_InicialSeguridadYRutasSaaS') THEN

    CREATE INDEX `IX_Valores_Campo_Egresados_IdEgresado` ON `Valores_Campo_Egresados` (`IdEgresado`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261030091206_InicialSeguridadYRutasSaaS') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20261030091206_InicialSeguridadYRutasSaaS', '10.0.14');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

COMMIT;

START TRANSACTION;
DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261101030336_AgregarMotivoRechazo') THEN

    ALTER TABLE `Egresados` RENAME COLUMN `IdEgresados` TO `IdEgresado`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261101030336_AgregarMotivoRechazo') THEN

    ALTER TABLE `Programas_Educativos` ADD `Siglas` varchar(20) CHARACTER SET utf8mb4 NOT NULL DEFAULT '';

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261101030336_AgregarMotivoRechazo') THEN

    ALTER TABLE `Egresados` MODIFY COLUMN `FechaEgreso` int NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261101030336_AgregarMotivoRechazo') THEN

    ALTER TABLE `Egresados` ADD `MotivoRechazo` longtext CHARACTER SET utf8mb4 NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261101030336_AgregarMotivoRechazo') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20261101030336_AgregarMotivoRechazo', '10.0.14');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

COMMIT;

START TRANSACTION;
DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261105232002_AgregarPerfilAcademico') THEN

    CREATE TABLE `PerfilesAcademicos` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `EgresadoId` int NOT NULL,
        `Orcid` varchar(255) CHARACTER SET utf8mb4 NULL,
        `GoogleScholar` varchar(255) CHARACTER SET utf8mb4 NULL,
        `ResearchGate` varchar(255) CHARACTER SET utf8mb4 NULL,
        `AcademiaEdu` varchar(255) CHARACTER SET utf8mb4 NULL,
        `ScopusId` varchar(255) CHARACTER SET utf8mb4 NULL,
        `LinkedIn` varchar(255) CHARACTER SET utf8mb4 NULL,
        CONSTRAINT `PK_PerfilesAcademicos` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_PerfilesAcademicos_Egresados_EgresadoId` FOREIGN KEY (`EgresadoId`) REFERENCES `Egresados` (`IdEgresado`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261105232002_AgregarPerfilAcademico') THEN

    CREATE UNIQUE INDEX `IX_PerfilesAcademicos_EgresadoId` ON `PerfilesAcademicos` (`EgresadoId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261105232002_AgregarPerfilAcademico') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20261105232002_AgregarPerfilAcademico', '10.0.14');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

COMMIT;

START TRANSACTION;
DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261105234852_cambioNombre') THEN

    ALTER TABLE `PerfilesAcademicos` DROP FOREIGN KEY `FK_PerfilesAcademicos_Egresados_EgresadoId`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261105234852_cambioNombre') THEN

    CALL POMELO_BEFORE_DROP_PRIMARY_KEY(NULL, 'PerfilesAcademicos');
    ALTER TABLE `PerfilesAcademicos` DROP PRIMARY KEY;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261105234852_cambioNombre') THEN

    ALTER TABLE `PerfilesAcademicos` RENAME `Perfiles_Academicos`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261105234852_cambioNombre') THEN

    ALTER TABLE `Perfiles_Academicos` RENAME INDEX `IX_PerfilesAcademicos_EgresadoId` TO `IX_Perfiles_Academicos_EgresadoId`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261105234852_cambioNombre') THEN

    ALTER TABLE `Perfiles_Academicos` ADD CONSTRAINT `PK_Perfiles_Academicos` PRIMARY KEY (`Id`);
    CALL POMELO_AFTER_ADD_PRIMARY_KEY(NULL, 'Perfiles_Academicos', 'Id');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261105234852_cambioNombre') THEN

    ALTER TABLE `Perfiles_Academicos` ADD CONSTRAINT `FK_Perfiles_Academicos_Egresados_EgresadoId` FOREIGN KEY (`EgresadoId`) REFERENCES `Egresados` (`IdEgresado`) ON DELETE CASCADE;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261105234852_cambioNombre') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20261105234852_cambioNombre', '10.0.14');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

COMMIT;

START TRANSACTION;
DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261106073224_AgregarTrayectoriaEgresado') THEN

    CREATE TABLE `Experiencias_Laborales` (
        `IdExperienciaLaboral` int NOT NULL AUTO_INCREMENT,
        `EgresadoId` int NOT NULL,
        `Institucion` varchar(200) CHARACTER SET utf8mb4 NOT NULL,
        `Puesto` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
        `Sector` varchar(100) CHARACTER SET utf8mb4 NULL,
        `FechaInicio` datetime(6) NULL,
        `FechaFin` datetime(6) NULL,
        `EsEmpleoActual` tinyint(1) NOT NULL,
        `ActividadesPrincipales` varchar(1000) CHARACTER SET utf8mb4 NULL,
        `NivelEducativo` varchar(150) CHARACTER SET utf8mb4 NULL,
        `RelacionConFormacion` varchar(100) CHARACTER SET utf8mb4 NULL,
        CONSTRAINT `PK_Experiencias_Laborales` PRIMARY KEY (`IdExperienciaLaboral`),
        CONSTRAINT `FK_Experiencias_Laborales_Egresados_EgresadoId` FOREIGN KEY (`EgresadoId`) REFERENCES `Egresados` (`IdEgresado`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261106073224_AgregarTrayectoriaEgresado') THEN

    CREATE TABLE `Formaciones_Academicas` (
        `IdFormacionAcademica` int NOT NULL AUTO_INCREMENT,
        `EgresadoId` int NOT NULL,
        `NombrePrograma` varchar(200) CHARACTER SET utf8mb4 NOT NULL,
        `Grado` varchar(100) CHARACTER SET utf8mb4 NULL,
        `Institucion` varchar(200) CHARACTER SET utf8mb4 NULL,
        `Pais` varchar(100) CHARACTER SET utf8mb4 NULL,
        `EstadoPrograma` varchar(100) CHARACTER SET utf8mb4 NULL,
        `FechaInicio` datetime(6) NULL,
        `FechaFin` datetime(6) NULL,
        `RecibioBeca` tinyint(1) NOT NULL,
        `TipoApoyo` varchar(150) CHARACTER SET utf8mb4 NULL,
        `InstitucionOtorgante` varchar(200) CHARACTER SET utf8mb4 NULL,
        `AnoOtorgamiento` int NULL,
        `MontoAsignado` decimal(18,2) NULL,
        CONSTRAINT `PK_Formaciones_Academicas` PRIMARY KEY (`IdFormacionAcademica`),
        CONSTRAINT `FK_Formaciones_Academicas_Egresados_EgresadoId` FOREIGN KEY (`EgresadoId`) REFERENCES `Egresados` (`IdEgresado`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261106073224_AgregarTrayectoriaEgresado') THEN

    CREATE TABLE `Situaciones_Laborales` (
        `IdSituacionLaboral` int NOT NULL AUTO_INCREMENT,
        `EgreasdoId` int NOT NULL,
        `EgresadoId` int NOT NULL,
        `EstadoActual` varchar(150) CHARACTER SET utf8mb4 NULL,
        `NuevoEmpleo` tinyint(1) NOT NULL,
        `Promocion` tinyint(1) NOT NULL,
        `CambioInstitucion` tinyint(1) NOT NULL,
        `NuevasResponsabilidades` tinyint(1) NOT NULL,
        `IncorporacionPosgradoñ` tinyint(1) NOT NULL,
        `EmprendimientoPropio` tinyint(1) NOT NULL,
        CONSTRAINT `PK_Situaciones_Laborales` PRIMARY KEY (`IdSituacionLaboral`),
        CONSTRAINT `FK_Situaciones_Laborales_Egresados_EgresadoId` FOREIGN KEY (`EgresadoId`) REFERENCES `Egresados` (`IdEgresado`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261106073224_AgregarTrayectoriaEgresado') THEN

    CREATE INDEX `IX_Experiencias_Laborales_EgresadoId` ON `Experiencias_Laborales` (`EgresadoId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261106073224_AgregarTrayectoriaEgresado') THEN

    CREATE INDEX `IX_Formaciones_Academicas_EgresadoId` ON `Formaciones_Academicas` (`EgresadoId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261106073224_AgregarTrayectoriaEgresado') THEN

    CREATE UNIQUE INDEX `IX_Situaciones_Laborales_EgresadoId` ON `Situaciones_Laborales` (`EgresadoId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261106073224_AgregarTrayectoriaEgresado') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20261106073224_AgregarTrayectoriaEgresado', '10.0.14');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

COMMIT;

START TRANSACTION;
DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261106075203_CorreccionNombre') THEN

    ALTER TABLE `Situaciones_Laborales` DROP COLUMN `EgreasdoId`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261106075203_CorreccionNombre') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20261106075203_CorreccionNombre', '10.0.14');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

COMMIT;

START TRANSACTION;
DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261106080803_correccionnombre2') THEN

    ALTER TABLE `Situaciones_Laborales` RENAME COLUMN `IncorporacionPosgradoñ` TO `IncorporacionPosgrado`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261106080803_correccionnombre2') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20261106080803_correccionnombre2', '10.0.14');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

COMMIT;

START TRANSACTION;
DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261106083825_formacionActual') THEN

    ALTER TABLE `Formaciones_Academicas` ADD `EsFormacionActual` tinyint(1) NOT NULL DEFAULT FALSE;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261106083825_formacionActual') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20261106083825_formacionActual', '10.0.14');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

COMMIT;

DROP PROCEDURE `POMELO_BEFORE_DROP_PRIMARY_KEY`;

DROP PROCEDURE `POMELO_AFTER_ADD_PRIMARY_KEY`;

START TRANSACTION;


-- Crear los Roles
INSERT INTO `Roles` (`NombreRol`) 
VALUES 
    ('ResponsablePrograma'), 
    ('Egresado');

-- Crear los Programas Educativos
INSERT INTO `Programas_Educativos` (`NombrePrograma`, `Siglas`) 
VALUES 
    ('Maestría en Investigación Educativa', 'MIED'),
    ('Ingeniero de software', 'ISW');
    
-- 3. Insertar usuarios de prueba
-- El usuario 1 tiene el IdRol 1 (ResponsablePrograma) y el usuario 2 tiene el IdRol 2 (Egresado)
INSERT INTO `Usuarios` (`IdUsuario`, `Correo`, `ContrasenaHash`, `IdRol`) 
VALUES 
    (1, 'responsable@itson.edu.mx', 'admin123', 1),
    (2, 'egresado@ejemplo.com', 'egresado123', 2);

-- 4. Crear el registro base del egresado
-- Se vincula al IdUsuario 2 y al IdPrograma 1 (MIED)
INSERT INTO `Egresados` (`IdEgresado`, `EstadoSolicitud`,`PrivacidadPerfil`,`AutorizaDirectorio`,`IdUsuario`, `IdPrograma`, `Nombres`, `ApellidoPaterno`, `ApellidoMaterno`,`FechaAceptacionPrivacidad`,`FechaUltimaActualizacion`) 
VALUES 
    (1,"Aprobado","Privado",0, 2, 1, 'Sebastián', 'Martínez', 'Soqui',NOW(),NOW());
    
--  Vincular reponsable con programa educativo
INSERT INTO programas_responsables (`IdPrograma`,`IdUsuario`,`FechaAsignacion`) values (1,1,now());
COMMIT;


