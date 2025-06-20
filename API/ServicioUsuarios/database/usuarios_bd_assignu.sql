CREATE DATABASE  IF NOT EXISTS `usuarios_bd_assignu` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `usuarios_bd_assignu`;
-- MySQL dump 10.13  Distrib 8.0.41, for Win64 (x86_64)
--
-- Host: localhost    Database: usuarios_bd_assignu
-- ------------------------------------------------------
-- Server version	8.4.4

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
-- Table structure for table `alumno`
--

DROP TABLE IF EXISTS `alumno`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `alumno` (
  `idAlumno` int NOT NULL AUTO_INCREMENT,
  `nombreCompleto` varchar(135) DEFAULT NULL,
  `nombreUsuario` varchar(45) DEFAULT NULL,
  `correo` varchar(45) DEFAULT NULL,
  `contrasenia` varchar(64) DEFAULT NULL,
  `idGradoEstudios` int DEFAULT NULL,
  PRIMARY KEY (`idAlumno`),
  UNIQUE KEY `correo_UNIQUE` (`correo`),
  UNIQUE KEY `nombreUsuario_UNIQUE` (`nombreUsuario`),
  KEY `alumno-grado_idx` (`idGradoEstudios`),
  CONSTRAINT `alumno-grado` FOREIGN KEY (`idGradoEstudios`) REFERENCES `grado_estudios` (`idGradoEstudios`)
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `alumno`
--

LOCK TABLES `alumno` WRITE;
/*!40000 ALTER TABLE `alumno` DISABLE KEYS */;
/*!40000 ALTER TABLE `alumno` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `docente`
--

DROP TABLE IF EXISTS `docente`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `docente` (
  `idDocente` int NOT NULL AUTO_INCREMENT,
  `nombreCompleto` varchar(135) DEFAULT NULL,
  `nombreUsuario` varchar(45) DEFAULT NULL,
  `correo` varchar(45) DEFAULT NULL,
  `contrasenia` varchar(64) DEFAULT NULL,
  `idGradoProfesional` int DEFAULT NULL,
  PRIMARY KEY (`idDocente`),
  UNIQUE KEY `correo_UNIQUE` (`correo`),
  UNIQUE KEY `nombreUsuario_UNIQUE` (`nombreUsuario`),
  KEY `docente-grado_idx` (`idGradoProfesional`),
  CONSTRAINT `docente-grado` FOREIGN KEY (`idGradoProfesional`) REFERENCES `grado_profesional` (`idGradoProfesional`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `docente`
--

LOCK TABLES `docente` WRITE;
/*!40000 ALTER TABLE `docente` DISABLE KEYS */;
/*!40000 ALTER TABLE `docente` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `grado_estudios`
--

DROP TABLE IF EXISTS `grado_estudios`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `grado_estudios` (
  `idGradoEstudios` int NOT NULL AUTO_INCREMENT,
  `nombre` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`idGradoEstudios`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `grado_estudios`
--

LOCK TABLES `grado_estudios` WRITE;
/*!40000 ALTER TABLE `grado_estudios` DISABLE KEYS */;
/*!40000 ALTER TABLE `grado_estudios` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `grado_profesional`
--

DROP TABLE IF EXISTS `grado_profesional`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `grado_profesional` (
  `idGradoProfesional` int NOT NULL AUTO_INCREMENT,
  `nombre` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`idGradoProfesional`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `grado_profesional`
--

LOCK TABLES `grado_profesional` WRITE;
/*!40000 ALTER TABLE `grado_profesional` DISABLE KEYS */;

/*!40000 ALTER TABLE `grado_profesional` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-06-20  2:56:25

-- Insertar datos
INSERT INTO `grado_estudios` VALUES (1,'Primaria'),(2,'Secundaria'),(3,'Bachillerato'),(4,'Universidad'),(5,'Maestría'),(6,'Doctorado'),(7,'Postdoctorado');
INSERT INTO `grado_profesional` VALUES (1,'Licenciatura'),(2,'Maestría'),(3,'Doctorado');

-- Crear usuario
CREATE USER 'usuarios_assignu'@'%' IDENTIFIED BY 'usuario123';

-- Otorgar privilegios sobre la base de datos
GRANT ALL PRIVILEGES ON usuarios_bd_assignu.* TO 'usuarios_assignu'@'%';

-- Aplicar cambios de privilegios
FLUSH PRIVILEGES;