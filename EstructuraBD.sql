CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `ProductVersion` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
) CHARACTER SET=utf8mb4;

START TRANSACTION;
ALTER DATABASE CHARACTER SET utf8mb4;

CREATE TABLE `Programas_Educativos` (
    `IdPrograma` int NOT NULL AUTO_INCREMENT,
    `NombrePrograma` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_Programas_Educativos` PRIMARY KEY (`IdPrograma`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `Roles` (
    `IdRol` int NOT NULL AUTO_INCREMENT,
    `NombreRol` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_Roles` PRIMARY KEY (`IdRol`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `Secciones_Personalizadas` (
    `IdSeccion` int NOT NULL AUTO_INCREMENT,
    `IdPrograma` int NOT NULL,
    `TituloSeccion` varchar(160) CHARACTER SET utf8mb4 NOT NULL,
    `OrdenAparicion` int NOT NULL,
    CONSTRAINT `PK_Secciones_Personalizadas` PRIMARY KEY (`IdSeccion`),
    CONSTRAINT `FK_Secciones_Personalizadas_Programas_Educativos_IdPrograma` FOREIGN KEY (`IdPrograma`) REFERENCES `Programas_Educativos` (`IdPrograma`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `Usuarios` (
    `IdUsuario` int NOT NULL AUTO_INCREMENT,
    `Correo` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `ContrasenaHash` varchar(265) CHARACTER SET utf8mb4 NOT NULL,
    `IdRol` int NOT NULL,
    CONSTRAINT `PK_Usuarios` PRIMARY KEY (`IdUsuario`),
    CONSTRAINT `FK_Usuarios_Roles_IdRol` FOREIGN KEY (`IdRol`) REFERENCES `Roles` (`IdRol`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

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

CREATE TABLE `Programas_Responsables` (
    `IdProgramaResponsable` int NOT NULL AUTO_INCREMENT,
    `IdPrograma` int NOT NULL,
    `IdUsuario` int NOT NULL,
    `FechaAsignacion` datetime(6) NOT NULL,
    CONSTRAINT `PK_Programas_Responsables` PRIMARY KEY (`IdProgramaResponsable`),
    CONSTRAINT `FK_Programas_Responsables_Programas_Educativos_IdPrograma` FOREIGN KEY (`IdPrograma`) REFERENCES `Programas_Educativos` (`IdPrograma`) ON DELETE CASCADE,
    CONSTRAINT `FK_Programas_Responsables_Usuarios_IdUsuario` FOREIGN KEY (`IdUsuario`) REFERENCES `Usuarios` (`IdUsuario`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `Opciones_Catalogo_Campo` (
    `IdOpcion` int NOT NULL AUTO_INCREMENT,
    `IdCampo` int NOT NULL,
    `ValorOpcion` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `Orden` int NOT NULL,
    CONSTRAINT `PK_Opciones_Catalogo_Campo` PRIMARY KEY (`IdOpcion`),
    CONSTRAINT `FK_Opciones_Catalogo_Campo_Campos_Personalizados_IdCampo` FOREIGN KEY (`IdCampo`) REFERENCES `Campos_Personalizados` (`IdCampo`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `Valores_Campo_Egresados` (
    `IdValor` int NOT NULL AUTO_INCREMENT,
    `IdEgresado` int NOT NULL,
    `IdCampo` int NOT NULL,
    `ValorCapturado` longtext CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_Valores_Campo_Egresados` PRIMARY KEY (`IdValor`),
    CONSTRAINT `FK_Valores_Campo_Egresados_Campos_Personalizados_IdCampo` FOREIGN KEY (`IdCampo`) REFERENCES `Campos_Personalizados` (`IdCampo`) ON DELETE CASCADE,
    CONSTRAINT `FK_Valores_Campo_Egresados_Egresados_IdEgresado` FOREIGN KEY (`IdEgresado`) REFERENCES `Egresados` (`IdEgresados`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE INDEX `IX_Campos_Personalizados_IdSeccion` ON `Campos_Personalizados` (`IdSeccion`);

CREATE INDEX `IX_Egresados_IdPrograma` ON `Egresados` (`IdPrograma`);

CREATE UNIQUE INDEX `IX_Egresados_IdUsuario` ON `Egresados` (`IdUsuario`);

CREATE INDEX `IX_Egresados_IdUsuarioResolvio` ON `Egresados` (`IdUsuarioResolvio`);

CREATE INDEX `IX_Opciones_Catalogo_Campo_IdCampo` ON `Opciones_Catalogo_Campo` (`IdCampo`);

CREATE UNIQUE INDEX `IX_Programas_Responsables_IdPrograma_IdUsuario` ON `Programas_Responsables` (`IdPrograma`, `IdUsuario`);

CREATE INDEX `IX_Programas_Responsables_IdUsuario` ON `Programas_Responsables` (`IdUsuario`);

CREATE INDEX `IX_Secciones_Personalizadas_IdPrograma` ON `Secciones_Personalizadas` (`IdPrograma`);

CREATE UNIQUE INDEX `IX_Usuarios_Correo` ON `Usuarios` (`Correo`);

CREATE INDEX `IX_Usuarios_IdRol` ON `Usuarios` (`IdRol`);

CREATE INDEX `IX_Valores_Campo_Egresados_IdCampo` ON `Valores_Campo_Egresados` (`IdCampo`);

CREATE INDEX `IX_Valores_Campo_Egresados_IdEgresado` ON `Valores_Campo_Egresados` (`IdEgresado`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20261030091206_InicialSeguridadYRutasSaaS', '10.0.14');

COMMIT;

START TRANSACTION;
ALTER TABLE `Egresados` RENAME COLUMN `IdEgresados` TO `IdEgresado`;

ALTER TABLE `Programas_Educativos` ADD `Siglas` varchar(20) CHARACTER SET utf8mb4 NOT NULL DEFAULT '';

ALTER TABLE `Egresados` MODIFY COLUMN `FechaEgreso` int NULL;

ALTER TABLE `Egresados` ADD `MotivoRechazo` longtext CHARACTER SET utf8mb4 NULL;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20261101030336_AgregarMotivoRechazo', '10.0.14');

COMMIT;

START TRANSACTION;
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

CREATE UNIQUE INDEX `IX_PerfilesAcademicos_EgresadoId` ON `PerfilesAcademicos` (`EgresadoId`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20261105232002_AgregarPerfilAcademico', '10.0.14');

COMMIT;

START TRANSACTION;
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

ALTER TABLE `PerfilesAcademicos` DROP FOREIGN KEY `FK_PerfilesAcademicos_Egresados_EgresadoId`;

CALL POMELO_BEFORE_DROP_PRIMARY_KEY(NULL, 'PerfilesAcademicos');
ALTER TABLE `PerfilesAcademicos` DROP PRIMARY KEY;

ALTER TABLE `PerfilesAcademicos` RENAME `Perfiles_Academicos`;

ALTER TABLE `Perfiles_Academicos` RENAME INDEX `IX_PerfilesAcademicos_EgresadoId` TO `IX_Perfiles_Academicos_EgresadoId`;

ALTER TABLE `Perfiles_Academicos` ADD CONSTRAINT `PK_Perfiles_Academicos` PRIMARY KEY (`Id`);
CALL POMELO_AFTER_ADD_PRIMARY_KEY(NULL, 'Perfiles_Academicos', 'Id');

ALTER TABLE `Perfiles_Academicos` ADD CONSTRAINT `FK_Perfiles_Academicos_Egresados_EgresadoId` FOREIGN KEY (`EgresadoId`) REFERENCES `Egresados` (`IdEgresado`) ON DELETE CASCADE;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20261105234852_cambioNombre', '10.0.14');

DROP PROCEDURE `POMELO_BEFORE_DROP_PRIMARY_KEY`;

DROP PROCEDURE `POMELO_AFTER_ADD_PRIMARY_KEY`;

COMMIT;

START TRANSACTION;
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

CREATE INDEX `IX_Experiencias_Laborales_EgresadoId` ON `Experiencias_Laborales` (`EgresadoId`);

CREATE INDEX `IX_Formaciones_Academicas_EgresadoId` ON `Formaciones_Academicas` (`EgresadoId`);

CREATE UNIQUE INDEX `IX_Situaciones_Laborales_EgresadoId` ON `Situaciones_Laborales` (`EgresadoId`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20261106073224_AgregarTrayectoriaEgresado', '10.0.14');

COMMIT;

START TRANSACTION;
ALTER TABLE `Situaciones_Laborales` DROP COLUMN `EgreasdoId`;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20261106075203_CorreccionNombre', '10.0.14');

COMMIT;

START TRANSACTION;
ALTER TABLE `Situaciones_Laborales` RENAME COLUMN `IncorporacionPosgradoñ` TO `IncorporacionPosgrado`;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20261106080803_correccionnombre2', '10.0.14');

COMMIT;

START TRANSACTION;
ALTER TABLE `Formaciones_Academicas` ADD `EsFormacionActual` tinyint(1) NOT NULL DEFAULT FALSE;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20261106083825_formacionActual', '10.0.14');

COMMIT;

START TRANSACTION;
CREATE TABLE `Contribuciones-Sociales` (
    `IdContribucion` int NOT NULL AUTO_INCREMENT,
    `EgresadoId` int NOT NULL,
    `NombreActividad` varchar(200) CHARACTER SET utf8mb4 NOT NULL,
    `TipoContribucion` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `SectorAtendido` varchar(200) CHARACTER SET utf8mb4 NOT NULL,
    `FechaInicio` datetime(6) NULL,
    `FechaFin` datetime(6) NULL,
    `Descripcion` varchar(1000) CHARACTER SET utf8mb4 NOT NULL,
    `Resultados` varchar(1000) CHARACTER SET utf8mb4 NOT NULL,
    `ArchivoEvidencia` varchar(255) CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_Contribuciones-Sociales` PRIMARY KEY (`IdContribucion`),
    CONSTRAINT `FK_Contribuciones-Sociales_Egresados_EgresadoId` FOREIGN KEY (`EgresadoId`) REFERENCES `Egresados` (`IdEgresado`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `Productividades-Cientificas` (
    `IdProductividad` int NOT NULL AUTO_INCREMENT,
    `EgresadoId` int NOT NULL,
    `TipoProducto` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `Titulo` varchar(250) CHARACTER SET utf8mb4 NOT NULL,
    `RevistaOLibro` varchar(200) CHARACTER SET utf8mb4 NULL,
    `Editorial` varchar(150) CHARACTER SET utf8mb4 NULL,
    `AnoPublicacion` int NULL,
    `DoiUrl` varchar(250) CHARACTER SET utf8mb4 NULL,
    `Isbn` varchar(50) CHARACTER SET utf8mb4 NULL,
    `Participacion` varchar(100) CHARACTER SET utf8mb4 NULL,
    `ArchivoPdf` varchar(255) CHARACTER SET utf8mb4 NULL,
    `NombreProfesorMied` varchar(200) CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_Productividades-Cientificas` PRIMARY KEY (`IdProductividad`),
    CONSTRAINT `FK_Productividades-Cientificas_Egresados_EgresadoId` FOREIGN KEY (`EgresadoId`) REFERENCES `Egresados` (`IdEgresado`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `Proyectos-Investigacion` (
    `IdProyecto` int NOT NULL AUTO_INCREMENT,
    `EgresadoId` int NOT NULL,
    `Nombre` varchar(250) CHARACTER SET utf8mb4 NOT NULL,
    `Institucion` varchar(200) CHARACTER SET utf8mb4 NOT NULL,
    `Periodo` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `Funcion` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `Estado` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_Proyectos-Investigacion` PRIMARY KEY (`IdProyecto`),
    CONSTRAINT `FK_Proyectos-Investigacion_Egresados_EgresadoId` FOREIGN KEY (`EgresadoId`) REFERENCES `Egresados` (`IdEgresado`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `Reconocomientos-Academicos` (
    `IdReconomiento` int NOT NULL AUTO_INCREMENT,
    `EgresadoId` int NOT NULL,
    `TipoRegistro` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `Nivel` varchar(100) CHARACTER SET utf8mb4 NULL,
    `AnoInicio` int NULL,
    `AnoFin` int NOT NULL,
    `NombreReconocimiento` varchar(250) CHARACTER SET utf8mb4 NULL,
    `InstitucionOtorgante` varchar(200) CHARACTER SET utf8mb4 NULL,
    `MotivoOCategoria` varchar(250) CHARACTER SET utf8mb4 NULL,
    `TipoParticipacion` varchar(150) CHARACTER SET utf8mb4 NULL,
    `EstadoActual` varchar(50) CHARACTER SET utf8mb4 NULL,
    `ArchivoEvidencia` varchar(255) CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_Reconocomientos-Academicos` PRIMARY KEY (`IdReconomiento`),
    CONSTRAINT `FK_Reconocomientos-Academicos_Egresados_EgresadoId` FOREIGN KEY (`EgresadoId`) REFERENCES `Egresados` (`IdEgresado`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `Resumenes-Productividad` (
    `IdResumen` int NOT NULL AUTO_INCREMENT,
    `EgresadoId` int NOT NULL,
    `TienePremios` tinyint(1) NOT NULL,
    `TieneCertificaciones` tinyint(1) NOT NULL,
    `TieneMembresias` tinyint(1) NOT NULL,
    `TieneContribucionSocial` tinyint(1) NOT NULL,
    CONSTRAINT `PK_Resumenes-Productividad` PRIMARY KEY (`IdResumen`),
    CONSTRAINT `FK_Resumenes-Productividad_Egresados_EgresadoId` FOREIGN KEY (`EgresadoId`) REFERENCES `Egresados` (`IdEgresado`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `Coautores-Productos` (
    `IdCoautor` int NOT NULL AUTO_INCREMENT,
    `ProductividadId` int NOT NULL,
    `Nombres` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `ApellidoPaterno` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `ApellidoMaterno` varchar(100) CHARACTER SET utf8mb4 NULL,
    `Rol` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_Coautores-Productos` PRIMARY KEY (`IdCoautor`),
    CONSTRAINT `FK_Coautores-Productos_Productividades-Cientificas_Productivida~` FOREIGN KEY (`ProductividadId`) REFERENCES `Productividades-Cientificas` (`IdProductividad`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE INDEX `IX_Coautores-Productos_ProductividadId` ON `Coautores-Productos` (`ProductividadId`);

CREATE INDEX `IX_Contribuciones-Sociales_EgresadoId` ON `Contribuciones-Sociales` (`EgresadoId`);

CREATE INDEX `IX_Productividades-Cientificas_EgresadoId` ON `Productividades-Cientificas` (`EgresadoId`);

CREATE INDEX `IX_Proyectos-Investigacion_EgresadoId` ON `Proyectos-Investigacion` (`EgresadoId`);

CREATE INDEX `IX_Reconocomientos-Academicos_EgresadoId` ON `Reconocomientos-Academicos` (`EgresadoId`);

CREATE UNIQUE INDEX `IX_Resumenes-Productividad_EgresadoId` ON `Resumenes-Productividad` (`EgresadoId`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20261110233717_AgregarProductividad', '10.0.14');

COMMIT;

START TRANSACTION;
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

ALTER TABLE `Coautores-Productos` DROP FOREIGN KEY `FK_Coautores-Productos_Productividades-Cientificas_Productivida~`;

ALTER TABLE `Contribuciones-Sociales` DROP FOREIGN KEY `FK_Contribuciones-Sociales_Egresados_EgresadoId`;

ALTER TABLE `Productividades-Cientificas` DROP FOREIGN KEY `FK_Productividades-Cientificas_Egresados_EgresadoId`;

ALTER TABLE `Proyectos-Investigacion` DROP FOREIGN KEY `FK_Proyectos-Investigacion_Egresados_EgresadoId`;

ALTER TABLE `Reconocomientos-Academicos` DROP FOREIGN KEY `FK_Reconocomientos-Academicos_Egresados_EgresadoId`;

ALTER TABLE `Resumenes-Productividad` DROP FOREIGN KEY `FK_Resumenes-Productividad_Egresados_EgresadoId`;

CALL POMELO_BEFORE_DROP_PRIMARY_KEY(NULL, 'Resumenes-Productividad');
ALTER TABLE `Resumenes-Productividad` DROP PRIMARY KEY;

CALL POMELO_BEFORE_DROP_PRIMARY_KEY(NULL, 'Reconocomientos-Academicos');
ALTER TABLE `Reconocomientos-Academicos` DROP PRIMARY KEY;

CALL POMELO_BEFORE_DROP_PRIMARY_KEY(NULL, 'Proyectos-Investigacion');
ALTER TABLE `Proyectos-Investigacion` DROP PRIMARY KEY;

CALL POMELO_BEFORE_DROP_PRIMARY_KEY(NULL, 'Productividades-Cientificas');
ALTER TABLE `Productividades-Cientificas` DROP PRIMARY KEY;

CALL POMELO_BEFORE_DROP_PRIMARY_KEY(NULL, 'Contribuciones-Sociales');
ALTER TABLE `Contribuciones-Sociales` DROP PRIMARY KEY;

CALL POMELO_BEFORE_DROP_PRIMARY_KEY(NULL, 'Coautores-Productos');
ALTER TABLE `Coautores-Productos` DROP PRIMARY KEY;

ALTER TABLE `Resumenes-Productividad` RENAME `Resumenes_Productividad`;

ALTER TABLE `Reconocomientos-Academicos` RENAME `Reconocomientos_Academicos`;

ALTER TABLE `Proyectos-Investigacion` RENAME `Proyectos_Investigacion`;

ALTER TABLE `Productividades-Cientificas` RENAME `Productividades_Cientificas`;

ALTER TABLE `Contribuciones-Sociales` RENAME `Contribuciones_Sociales`;

ALTER TABLE `Coautores-Productos` RENAME `Coautores_Productos`;

ALTER TABLE `Resumenes_Productividad` RENAME INDEX `IX_Resumenes-Productividad_EgresadoId` TO `IX_Resumenes_Productividad_EgresadoId`;

ALTER TABLE `Reconocomientos_Academicos` RENAME INDEX `IX_Reconocomientos-Academicos_EgresadoId` TO `IX_Reconocomientos_Academicos_EgresadoId`;

ALTER TABLE `Proyectos_Investigacion` RENAME INDEX `IX_Proyectos-Investigacion_EgresadoId` TO `IX_Proyectos_Investigacion_EgresadoId`;

ALTER TABLE `Productividades_Cientificas` RENAME INDEX `IX_Productividades-Cientificas_EgresadoId` TO `IX_Productividades_Cientificas_EgresadoId`;

ALTER TABLE `Contribuciones_Sociales` RENAME INDEX `IX_Contribuciones-Sociales_EgresadoId` TO `IX_Contribuciones_Sociales_EgresadoId`;

ALTER TABLE `Coautores_Productos` RENAME INDEX `IX_Coautores-Productos_ProductividadId` TO `IX_Coautores_Productos_ProductividadId`;

ALTER TABLE `Resumenes_Productividad` ADD CONSTRAINT `PK_Resumenes_Productividad` PRIMARY KEY (`IdResumen`);
CALL POMELO_AFTER_ADD_PRIMARY_KEY(NULL, 'Resumenes_Productividad', 'IdResumen');

ALTER TABLE `Reconocomientos_Academicos` ADD CONSTRAINT `PK_Reconocomientos_Academicos` PRIMARY KEY (`IdReconomiento`);
CALL POMELO_AFTER_ADD_PRIMARY_KEY(NULL, 'Reconocomientos_Academicos', 'IdReconomiento');

ALTER TABLE `Proyectos_Investigacion` ADD CONSTRAINT `PK_Proyectos_Investigacion` PRIMARY KEY (`IdProyecto`);
CALL POMELO_AFTER_ADD_PRIMARY_KEY(NULL, 'Proyectos_Investigacion', 'IdProyecto');

ALTER TABLE `Productividades_Cientificas` ADD CONSTRAINT `PK_Productividades_Cientificas` PRIMARY KEY (`IdProductividad`);
CALL POMELO_AFTER_ADD_PRIMARY_KEY(NULL, 'Productividades_Cientificas', 'IdProductividad');

ALTER TABLE `Contribuciones_Sociales` ADD CONSTRAINT `PK_Contribuciones_Sociales` PRIMARY KEY (`IdContribucion`);
CALL POMELO_AFTER_ADD_PRIMARY_KEY(NULL, 'Contribuciones_Sociales', 'IdContribucion');

ALTER TABLE `Coautores_Productos` ADD CONSTRAINT `PK_Coautores_Productos` PRIMARY KEY (`IdCoautor`);
CALL POMELO_AFTER_ADD_PRIMARY_KEY(NULL, 'Coautores_Productos', 'IdCoautor');

ALTER TABLE `Coautores_Productos` ADD CONSTRAINT `FK_Coautores_Productos_Productividades_Cientificas_Productivida~` FOREIGN KEY (`ProductividadId`) REFERENCES `Productividades_Cientificas` (`IdProductividad`) ON DELETE CASCADE;

ALTER TABLE `Contribuciones_Sociales` ADD CONSTRAINT `FK_Contribuciones_Sociales_Egresados_EgresadoId` FOREIGN KEY (`EgresadoId`) REFERENCES `Egresados` (`IdEgresado`) ON DELETE CASCADE;

ALTER TABLE `Productividades_Cientificas` ADD CONSTRAINT `FK_Productividades_Cientificas_Egresados_EgresadoId` FOREIGN KEY (`EgresadoId`) REFERENCES `Egresados` (`IdEgresado`) ON DELETE CASCADE;

ALTER TABLE `Proyectos_Investigacion` ADD CONSTRAINT `FK_Proyectos_Investigacion_Egresados_EgresadoId` FOREIGN KEY (`EgresadoId`) REFERENCES `Egresados` (`IdEgresado`) ON DELETE CASCADE;

ALTER TABLE `Reconocomientos_Academicos` ADD CONSTRAINT `FK_Reconocomientos_Academicos_Egresados_EgresadoId` FOREIGN KEY (`EgresadoId`) REFERENCES `Egresados` (`IdEgresado`) ON DELETE CASCADE;

ALTER TABLE `Resumenes_Productividad` ADD CONSTRAINT `FK_Resumenes_Productividad_Egresados_EgresadoId` FOREIGN KEY (`EgresadoId`) REFERENCES `Egresados` (`IdEgresado`) ON DELETE CASCADE;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20261110233904_AgregarProductividadF', '10.0.14');

DROP PROCEDURE `POMELO_BEFORE_DROP_PRIMARY_KEY`;

DROP PROCEDURE `POMELO_AFTER_ADD_PRIMARY_KEY`;

COMMIT;

START TRANSACTION;
ALTER TABLE `Productividades_Cientificas` ADD `ParticipoProfesorMied` tinyint(1) NOT NULL DEFAULT FALSE;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20261112205957_cambioModelProductividad', '10.0.14');

COMMIT;

