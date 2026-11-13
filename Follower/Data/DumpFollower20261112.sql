CREATE DATABASE  IF NOT EXISTS `follower` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `follower`;
-- MySQL dump 10.13  Distrib 8.0.46, for Win64 (x86_64)
--
-- Host: localhost    Database: follower
-- ------------------------------------------------------
-- Server version	8.0.46

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `__efmigrationshistory`
--

DROP TABLE IF EXISTS `__efmigrationshistory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `__efmigrationshistory` (
  `MigrationId` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ProductVersion` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`MigrationId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `__efmigrationshistory`
--

LOCK TABLES `__efmigrationshistory` WRITE;
/*!40000 ALTER TABLE `__efmigrationshistory` DISABLE KEYS */;
INSERT INTO `__efmigrationshistory` VALUES ('20261030091206_InicialSeguridadYRutasSaaS','10.0.14'),('20261101030336_AgregarMotivoRechazo','10.0.14'),('20261105232002_AgregarPerfilAcademico','10.0.14'),('20261105234852_cambioNombre','10.0.14'),('20261106073224_AgregarTrayectoriaEgresado','10.0.14'),('20261106075203_CorreccionNombre','10.0.14'),('20261106080803_correccionnombre2','10.0.14'),('20261106083825_formacionActual','10.0.14'),('20261110233717_AgregarProductividad','10.0.14'),('20261110233904_AgregarProductividadF','10.0.14');
/*!40000 ALTER TABLE `__efmigrationshistory` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `campos_personalizados`
--

DROP TABLE IF EXISTS `campos_personalizados`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `campos_personalizados` (
  `IdCampo` int NOT NULL AUTO_INCREMENT,
  `IdSeccion` int NOT NULL,
  `NombreEtiqueta` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `TipoDato` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `EsObligatorio` tinyint(1) NOT NULL,
  `Orden` int NOT NULL,
  PRIMARY KEY (`IdCampo`),
  KEY `IX_Campos_Personalizados_IdSeccion` (`IdSeccion`),
  CONSTRAINT `FK_Campos_Personalizados_Secciones_Personalizadas_IdSeccion` FOREIGN KEY (`IdSeccion`) REFERENCES `secciones_personalizadas` (`IdSeccion`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `campos_personalizados`
--

LOCK TABLES `campos_personalizados` WRITE;
/*!40000 ALTER TABLE `campos_personalizados` DISABLE KEYS */;
/*!40000 ALTER TABLE `campos_personalizados` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `coautores_productos`
--

DROP TABLE IF EXISTS `coautores_productos`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `coautores_productos` (
  `IdCoautor` int NOT NULL AUTO_INCREMENT,
  `ProductividadId` int NOT NULL,
  `Nombres` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ApellidoPaterno` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ApellidoMaterno` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Rol` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`IdCoautor`),
  KEY `IX_Coautores_Productos_ProductividadId` (`ProductividadId`),
  CONSTRAINT `FK_Coautores_Productos_Productividades_Cientificas_Productivida~` FOREIGN KEY (`ProductividadId`) REFERENCES `productividades_cientificas` (`IdProductividad`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `coautores_productos`
--

LOCK TABLES `coautores_productos` WRITE;
/*!40000 ALTER TABLE `coautores_productos` DISABLE KEYS */;
/*!40000 ALTER TABLE `coautores_productos` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `contribuciones_sociales`
--

DROP TABLE IF EXISTS `contribuciones_sociales`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `contribuciones_sociales` (
  `IdContribucion` int NOT NULL AUTO_INCREMENT,
  `EgresadoId` int NOT NULL,
  `NombreActividad` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `TipoContribucion` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `SectorAtendido` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `FechaInicio` datetime(6) DEFAULT NULL,
  `FechaFin` datetime(6) DEFAULT NULL,
  `Descripcion` varchar(1000) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Resultados` varchar(1000) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ArchivoEvidencia` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  PRIMARY KEY (`IdContribucion`),
  KEY `IX_Contribuciones_Sociales_EgresadoId` (`EgresadoId`),
  CONSTRAINT `FK_Contribuciones_Sociales_Egresados_EgresadoId` FOREIGN KEY (`EgresadoId`) REFERENCES `egresados` (`IdEgresado`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `contribuciones_sociales`
--

LOCK TABLES `contribuciones_sociales` WRITE;
/*!40000 ALTER TABLE `contribuciones_sociales` DISABLE KEYS */;
/*!40000 ALTER TABLE `contribuciones_sociales` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `egresados`
--

DROP TABLE IF EXISTS `egresados`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `egresados` (
  `IdEgresado` int NOT NULL AUTO_INCREMENT,
  `IdUsuario` int NOT NULL,
  `IdPrograma` int NOT NULL,
  `EstadoSolicitud` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `PrivacidadPerfil` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Nombres` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ApellidoPaterno` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ApellidoMaterno` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Telefono` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `FechaNacimiento` datetime(6) DEFAULT NULL,
  `FechaEgreso` int DEFAULT NULL,
  `Ciudad` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Estado` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Pais` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `FotoPerfilUrl` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `AutorizaDirectorio` tinyint(1) NOT NULL,
  `FechaSolicitud` datetime(6) DEFAULT NULL,
  `FechaAceptacionPrivacidad` datetime(6) NOT NULL,
  `FechaResolucion` datetime(6) DEFAULT NULL,
  `IdUsuarioResolvio` int DEFAULT NULL,
  `FechaUltimaActualizacion` datetime(6) NOT NULL,
  `MotivoRechazo` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  PRIMARY KEY (`IdEgresado`),
  UNIQUE KEY `IX_Egresados_IdUsuario` (`IdUsuario`),
  KEY `IX_Egresados_IdPrograma` (`IdPrograma`),
  KEY `IX_Egresados_IdUsuarioResolvio` (`IdUsuarioResolvio`),
  CONSTRAINT `FK_Egresados_Programas_Educativos_IdPrograma` FOREIGN KEY (`IdPrograma`) REFERENCES `programas_educativos` (`IdPrograma`) ON DELETE CASCADE,
  CONSTRAINT `FK_Egresados_Usuarios_IdUsuario` FOREIGN KEY (`IdUsuario`) REFERENCES `usuarios` (`IdUsuario`) ON DELETE CASCADE,
  CONSTRAINT `FK_Egresados_Usuarios_IdUsuarioResolvio` FOREIGN KEY (`IdUsuarioResolvio`) REFERENCES `usuarios` (`IdUsuario`) ON DELETE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `egresados`
--

LOCK TABLES `egresados` WRITE;
/*!40000 ALTER TABLE `egresados` DISABLE KEYS */;
INSERT INTO `egresados` VALUES (1,2,1,'Aprobado','Privado','Sebastián','Martínez','Soqui',NULL,NULL,NULL,NULL,NULL,NULL,NULL,0,NULL,'2026-11-11 20:49:10.000000',NULL,NULL,'2026-11-11 20:49:10.000000',NULL),(2,3,2,'Aprobado','Privado','David Elier','Campa','Chaparro',NULL,NULL,2027,NULL,NULL,NULL,NULL,0,'2026-11-13 01:32:27.108181','2026-11-13 01:32:27.108215','2026-11-12 18:35:52.651874',1000,'2026-11-13 01:32:27.108246',NULL);
/*!40000 ALTER TABLE `egresados` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `experiencias_laborales`
--

DROP TABLE IF EXISTS `experiencias_laborales`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `experiencias_laborales` (
  `IdExperienciaLaboral` int NOT NULL AUTO_INCREMENT,
  `EgresadoId` int NOT NULL,
  `Institucion` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Puesto` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Sector` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `FechaInicio` datetime(6) DEFAULT NULL,
  `FechaFin` datetime(6) DEFAULT NULL,
  `EsEmpleoActual` tinyint(1) NOT NULL,
  `ActividadesPrincipales` varchar(1000) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `NivelEducativo` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `RelacionConFormacion` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  PRIMARY KEY (`IdExperienciaLaboral`),
  KEY `IX_Experiencias_Laborales_EgresadoId` (`EgresadoId`),
  CONSTRAINT `FK_Experiencias_Laborales_Egresados_EgresadoId` FOREIGN KEY (`EgresadoId`) REFERENCES `egresados` (`IdEgresado`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `experiencias_laborales`
--

LOCK TABLES `experiencias_laborales` WRITE;
/*!40000 ALTER TABLE `experiencias_laborales` DISABLE KEYS */;
/*!40000 ALTER TABLE `experiencias_laborales` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `formaciones_academicas`
--

DROP TABLE IF EXISTS `formaciones_academicas`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `formaciones_academicas` (
  `IdFormacionAcademica` int NOT NULL AUTO_INCREMENT,
  `EgresadoId` int NOT NULL,
  `NombrePrograma` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Grado` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Institucion` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Pais` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `EstadoPrograma` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `FechaInicio` datetime(6) DEFAULT NULL,
  `FechaFin` datetime(6) DEFAULT NULL,
  `RecibioBeca` tinyint(1) NOT NULL,
  `TipoApoyo` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `InstitucionOtorgante` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `AnoOtorgamiento` int DEFAULT NULL,
  `MontoAsignado` decimal(18,2) DEFAULT NULL,
  `EsFormacionActual` tinyint(1) NOT NULL DEFAULT '0',
  PRIMARY KEY (`IdFormacionAcademica`),
  KEY `IX_Formaciones_Academicas_EgresadoId` (`EgresadoId`),
  CONSTRAINT `FK_Formaciones_Academicas_Egresados_EgresadoId` FOREIGN KEY (`EgresadoId`) REFERENCES `egresados` (`IdEgresado`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `formaciones_academicas`
--

LOCK TABLES `formaciones_academicas` WRITE;
/*!40000 ALTER TABLE `formaciones_academicas` DISABLE KEYS */;
/*!40000 ALTER TABLE `formaciones_academicas` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `opciones_catalogo_campo`
--

DROP TABLE IF EXISTS `opciones_catalogo_campo`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `opciones_catalogo_campo` (
  `IdOpcion` int NOT NULL AUTO_INCREMENT,
  `IdCampo` int NOT NULL,
  `ValorOpcion` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Orden` int NOT NULL,
  PRIMARY KEY (`IdOpcion`),
  KEY `IX_Opciones_Catalogo_Campo_IdCampo` (`IdCampo`),
  CONSTRAINT `FK_Opciones_Catalogo_Campo_Campos_Personalizados_IdCampo` FOREIGN KEY (`IdCampo`) REFERENCES `campos_personalizados` (`IdCampo`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `opciones_catalogo_campo`
--

LOCK TABLES `opciones_catalogo_campo` WRITE;
/*!40000 ALTER TABLE `opciones_catalogo_campo` DISABLE KEYS */;
/*!40000 ALTER TABLE `opciones_catalogo_campo` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `perfiles_academicos`
--

DROP TABLE IF EXISTS `perfiles_academicos`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `perfiles_academicos` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `EgresadoId` int NOT NULL,
  `Orcid` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `GoogleScholar` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `ResearchGate` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `AcademiaEdu` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `ScopusId` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `LinkedIn` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IX_Perfiles_Academicos_EgresadoId` (`EgresadoId`),
  CONSTRAINT `FK_Perfiles_Academicos_Egresados_EgresadoId` FOREIGN KEY (`EgresadoId`) REFERENCES `egresados` (`IdEgresado`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `perfiles_academicos`
--

LOCK TABLES `perfiles_academicos` WRITE;
/*!40000 ALTER TABLE `perfiles_academicos` DISABLE KEYS */;
INSERT INTO `perfiles_academicos` VALUES (1,2,NULL,NULL,NULL,NULL,NULL,NULL);
/*!40000 ALTER TABLE `perfiles_academicos` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `productividades_cientificas`
--

DROP TABLE IF EXISTS `productividades_cientificas`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `productividades_cientificas` (
  `IdProductividad` int NOT NULL AUTO_INCREMENT,
  `EgresadoId` int NOT NULL,
  `TipoProducto` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Titulo` varchar(250) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `RevistaOLibro` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Editorial` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `AnoPublicacion` int DEFAULT NULL,
  `DoiUrl` varchar(250) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Isbn` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Participacion` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `ArchivoPdf` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `NombreProfesorMied` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  PRIMARY KEY (`IdProductividad`),
  KEY `IX_Productividades_Cientificas_EgresadoId` (`EgresadoId`),
  CONSTRAINT `FK_Productividades_Cientificas_Egresados_EgresadoId` FOREIGN KEY (`EgresadoId`) REFERENCES `egresados` (`IdEgresado`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `productividades_cientificas`
--

LOCK TABLES `productividades_cientificas` WRITE;
/*!40000 ALTER TABLE `productividades_cientificas` DISABLE KEYS */;
/*!40000 ALTER TABLE `productividades_cientificas` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `programas_educativos`
--

DROP TABLE IF EXISTS `programas_educativos`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `programas_educativos` (
  `IdPrograma` int NOT NULL AUTO_INCREMENT,
  `NombrePrograma` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Siglas` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL DEFAULT '',
  PRIMARY KEY (`IdPrograma`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `programas_educativos`
--

LOCK TABLES `programas_educativos` WRITE;
/*!40000 ALTER TABLE `programas_educativos` DISABLE KEYS */;
INSERT INTO `programas_educativos` VALUES (1,'Maestría en Investigación Educativa','MIED'),(2,'Ingeniero de software','ISW');
/*!40000 ALTER TABLE `programas_educativos` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `programas_responsables`
--

DROP TABLE IF EXISTS `programas_responsables`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `programas_responsables` (
  `IdProgramaResponsable` int NOT NULL AUTO_INCREMENT,
  `IdPrograma` int NOT NULL,
  `IdUsuario` int NOT NULL,
  `FechaAsignacion` datetime(6) NOT NULL,
  PRIMARY KEY (`IdProgramaResponsable`),
  UNIQUE KEY `IX_Programas_Responsables_IdPrograma_IdUsuario` (`IdPrograma`,`IdUsuario`),
  KEY `IX_Programas_Responsables_IdUsuario` (`IdUsuario`),
  CONSTRAINT `FK_Programas_Responsables_Programas_Educativos_IdPrograma` FOREIGN KEY (`IdPrograma`) REFERENCES `programas_educativos` (`IdPrograma`) ON DELETE CASCADE,
  CONSTRAINT `FK_Programas_Responsables_Usuarios_IdUsuario` FOREIGN KEY (`IdUsuario`) REFERENCES `usuarios` (`IdUsuario`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=1001 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `programas_responsables`
--

LOCK TABLES `programas_responsables` WRITE;
/*!40000 ALTER TABLE `programas_responsables` DISABLE KEYS */;
INSERT INTO `programas_responsables` VALUES (1,1,1,'2026-11-11 20:49:10.000000'),(1000,2,1000,'2026-11-12 00:00:00.000000');
/*!40000 ALTER TABLE `programas_responsables` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `proyectos_investigacion`
--

DROP TABLE IF EXISTS `proyectos_investigacion`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `proyectos_investigacion` (
  `IdProyecto` int NOT NULL AUTO_INCREMENT,
  `EgresadoId` int NOT NULL,
  `Nombre` varchar(250) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Institucion` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Periodo` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Funcion` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Estado` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`IdProyecto`),
  KEY `IX_Proyectos_Investigacion_EgresadoId` (`EgresadoId`),
  CONSTRAINT `FK_Proyectos_Investigacion_Egresados_EgresadoId` FOREIGN KEY (`EgresadoId`) REFERENCES `egresados` (`IdEgresado`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `proyectos_investigacion`
--

LOCK TABLES `proyectos_investigacion` WRITE;
/*!40000 ALTER TABLE `proyectos_investigacion` DISABLE KEYS */;
/*!40000 ALTER TABLE `proyectos_investigacion` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `reconocomientos_academicos`
--

DROP TABLE IF EXISTS `reconocomientos_academicos`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `reconocomientos_academicos` (
  `IdReconomiento` int NOT NULL AUTO_INCREMENT,
  `EgresadoId` int NOT NULL,
  `TipoRegistro` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Nivel` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `AnoInicio` int DEFAULT NULL,
  `AnoFin` int NOT NULL,
  `NombreReconocimiento` varchar(250) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `InstitucionOtorgante` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `MotivoOCategoria` varchar(250) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `TipoParticipacion` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `EstadoActual` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `ArchivoEvidencia` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  PRIMARY KEY (`IdReconomiento`),
  KEY `IX_Reconocomientos_Academicos_EgresadoId` (`EgresadoId`),
  CONSTRAINT `FK_Reconocomientos_Academicos_Egresados_EgresadoId` FOREIGN KEY (`EgresadoId`) REFERENCES `egresados` (`IdEgresado`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `reconocomientos_academicos`
--

LOCK TABLES `reconocomientos_academicos` WRITE;
/*!40000 ALTER TABLE `reconocomientos_academicos` DISABLE KEYS */;
/*!40000 ALTER TABLE `reconocomientos_academicos` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `resumenes_productividad`
--

DROP TABLE IF EXISTS `resumenes_productividad`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `resumenes_productividad` (
  `IdResumen` int NOT NULL AUTO_INCREMENT,
  `EgresadoId` int NOT NULL,
  `TienePremios` tinyint(1) NOT NULL,
  `TieneCertificaciones` tinyint(1) NOT NULL,
  `TieneMembresias` tinyint(1) NOT NULL,
  `TieneContribucionSocial` tinyint(1) NOT NULL,
  PRIMARY KEY (`IdResumen`),
  UNIQUE KEY `IX_Resumenes_Productividad_EgresadoId` (`EgresadoId`),
  CONSTRAINT `FK_Resumenes_Productividad_Egresados_EgresadoId` FOREIGN KEY (`EgresadoId`) REFERENCES `egresados` (`IdEgresado`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `resumenes_productividad`
--

LOCK TABLES `resumenes_productividad` WRITE;
/*!40000 ALTER TABLE `resumenes_productividad` DISABLE KEYS */;
INSERT INTO `resumenes_productividad` VALUES (1,2,0,0,0,0);
/*!40000 ALTER TABLE `resumenes_productividad` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `roles`
--

DROP TABLE IF EXISTS `roles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `roles` (
  `IdRol` int NOT NULL AUTO_INCREMENT,
  `NombreRol` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`IdRol`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `roles`
--

LOCK TABLES `roles` WRITE;
/*!40000 ALTER TABLE `roles` DISABLE KEYS */;
INSERT INTO `roles` VALUES (1,'ResponsablePrograma'),(2,'Egresado');
/*!40000 ALTER TABLE `roles` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `secciones_personalizadas`
--

DROP TABLE IF EXISTS `secciones_personalizadas`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `secciones_personalizadas` (
  `IdSeccion` int NOT NULL AUTO_INCREMENT,
  `IdPrograma` int NOT NULL,
  `TituloSeccion` varchar(160) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `OrdenAparicion` int NOT NULL,
  PRIMARY KEY (`IdSeccion`),
  KEY `IX_Secciones_Personalizadas_IdPrograma` (`IdPrograma`),
  CONSTRAINT `FK_Secciones_Personalizadas_Programas_Educativos_IdPrograma` FOREIGN KEY (`IdPrograma`) REFERENCES `programas_educativos` (`IdPrograma`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `secciones_personalizadas`
--

LOCK TABLES `secciones_personalizadas` WRITE;
/*!40000 ALTER TABLE `secciones_personalizadas` DISABLE KEYS */;
/*!40000 ALTER TABLE `secciones_personalizadas` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `situaciones_laborales`
--

DROP TABLE IF EXISTS `situaciones_laborales`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `situaciones_laborales` (
  `IdSituacionLaboral` int NOT NULL AUTO_INCREMENT,
  `EgresadoId` int NOT NULL,
  `EstadoActual` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `NuevoEmpleo` tinyint(1) NOT NULL,
  `Promocion` tinyint(1) NOT NULL,
  `CambioInstitucion` tinyint(1) NOT NULL,
  `NuevasResponsabilidades` tinyint(1) NOT NULL,
  `IncorporacionPosgrado` tinyint(1) NOT NULL,
  `EmprendimientoPropio` tinyint(1) NOT NULL,
  PRIMARY KEY (`IdSituacionLaboral`),
  UNIQUE KEY `IX_Situaciones_Laborales_EgresadoId` (`EgresadoId`),
  CONSTRAINT `FK_Situaciones_Laborales_Egresados_EgresadoId` FOREIGN KEY (`EgresadoId`) REFERENCES `egresados` (`IdEgresado`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `situaciones_laborales`
--

LOCK TABLES `situaciones_laborales` WRITE;
/*!40000 ALTER TABLE `situaciones_laborales` DISABLE KEYS */;
INSERT INTO `situaciones_laborales` VALUES (1,2,NULL,0,0,0,0,0,0);
/*!40000 ALTER TABLE `situaciones_laborales` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `usuarios`
--

DROP TABLE IF EXISTS `usuarios`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `usuarios` (
  `IdUsuario` int NOT NULL AUTO_INCREMENT,
  `Correo` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ContrasenaHash` varchar(265) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `IdRol` int NOT NULL,
  PRIMARY KEY (`IdUsuario`),
  UNIQUE KEY `IX_Usuarios_Correo` (`Correo`),
  KEY `IX_Usuarios_IdRol` (`IdRol`),
  CONSTRAINT `FK_Usuarios_Roles_IdRol` FOREIGN KEY (`IdRol`) REFERENCES `roles` (`IdRol`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=1001 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `usuarios`
--

LOCK TABLES `usuarios` WRITE;
/*!40000 ALTER TABLE `usuarios` DISABLE KEYS */;
INSERT INTO `usuarios` VALUES (1,'responsable@itson.edu.mx','admin123',1),(2,'egresado@ejemplo.com','egresado123',2),(3,'david@gmail.com','xr471112',2),(1000,'isw@itson.edu.mx','asd',1);
/*!40000 ALTER TABLE `usuarios` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `valores_campo_egresados`
--

DROP TABLE IF EXISTS `valores_campo_egresados`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `valores_campo_egresados` (
  `IdValor` int NOT NULL AUTO_INCREMENT,
  `IdEgresado` int NOT NULL,
  `IdCampo` int NOT NULL,
  `ValorCapturado` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  PRIMARY KEY (`IdValor`),
  KEY `IX_Valores_Campo_Egresados_IdCampo` (`IdCampo`),
  KEY `IX_Valores_Campo_Egresados_IdEgresado` (`IdEgresado`),
  CONSTRAINT `FK_Valores_Campo_Egresados_Campos_Personalizados_IdCampo` FOREIGN KEY (`IdCampo`) REFERENCES `campos_personalizados` (`IdCampo`) ON DELETE CASCADE,
  CONSTRAINT `FK_Valores_Campo_Egresados_Egresados_IdEgresado` FOREIGN KEY (`IdEgresado`) REFERENCES `egresados` (`IdEgresado`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `valores_campo_egresados`
--

LOCK TABLES `valores_campo_egresados` WRITE;
/*!40000 ALTER TABLE `valores_campo_egresados` DISABLE KEYS */;
/*!40000 ALTER TABLE `valores_campo_egresados` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2026-11-12 20:45:31
