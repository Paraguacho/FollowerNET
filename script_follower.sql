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
        -- Aprobado // Pendiente // Rechazado
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


COMMIT;

